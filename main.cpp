#include "time.h"
#include <WebServer.h>
#include <ESPmDNS.h>
#include <Update.h>
#include <ESP32-HUB75-MatrixPanel-I2S-DMA.h>
#include <FontMaker.h>
#include <MyFontMaker.h>
#include <esp_ota_ops.h>
#include <esp_partition.h>
#include "SPIFFS.h"
#include <ArduinoJson.h>
#include "AsyncWebServer_ESP32_ENC.h"
#include "AsyncTCP.h"
#include <AsyncHTTPRequest_Generic.h>
#include <Ticker.h>
#include "esp_system.h"
#include "EEPROM.h"
#include "FS.h"
#include <HTTPClient.h>
#include <MD5Builder.h>

void updateTextAlignment();
void saveConfigToSPIFFS();
void sendHeartbeat();
void setupServerEndpoints();
void loadConfigFromSPIFFS();

// Định nghĩa chân ENC28J60
#define MOSI_GPIO 23
#define MISO_GPIO 19
#define CS_GPIO 5
#define SCK_GPIO 18
#define INT_GPIO 22
#define SPI_CLOCK_MHZ 20
#define ETH_SPI_HOST SPI_HOST
// Định nghĩa chân của LED Matrix
#define R1_PIN 32 // 19
#define G1_PIN 33
#define B1_PIN 25 // 18
#define R2_PIN 26 // 5
#define G2_PIN 27
#define B2_PIN 14
#define A_PIN 12
#define B_PIN 13
#define C_PIN 17
#define D_PIN 15
#define E_PIN -1
#define LAT_PIN 16
#define OE_PIN 4
#define CLK_PIN 2
#define PANEL_RES_X 64
#define PANEL_RES_Y 32
#define PANEL_CHAIN 2

#define RESET_BUTTON_PIN 35 // Chân GPIO35 cho nút nhấn reset

bool ipDisplayed = false; // Biến cờ để kiểm tra xem IP đã hiển thị chưa

// Biến toàn cục để chống dội phím
unsigned long lastDebounceTime = 0;
const unsigned long debounceDelay = 50; // Thời gian chống dội (ms)
int lastButtonState = HIGH;             // Giả định ban đầu là HIGH (có thể thay đổi)

// Biến toàn cục

uint16_t colorLine1 = 0;
uint16_t colorLine2 = 0;
uint8_t brightness = 100; // Độ sáng mặc định
uint8_t rotation = 0;     // 0: 0°, 1: 90°, 2: 180°, 3: 270°
uint8_t alignment = 1;    // 0: Left, 1: Center, 2: Right
bool useDHCP = true;      // Mặc định DHCP
String staticIP = "192.168.1.100";
String gateway = "192.168.1.1";
String subnet = "255.255.255.0";
uint16_t serverPort = 80; // Cổng mặc định
bool enableHeartbeat = false;
uint32_t heartbeatInterval = 10000; // 10 giây (ms)
String heartbeatServer = "192.168.1.10";
Ticker heartbeatTicker;
int panelWidth = PANEL_RES_X * PANEL_CHAIN; // Chiều rộng bảng LED (64 * 2 = 128 pixel)

String mainTextLine1 = "AIoT JSC SYSTECH"; // Nội dung dòng 1
String mainTextLine2 = "123456";           // Nội dung dòng 2
int textX1 = panelWidth;                   // Vị trí X của dòng 1
int textY1 = 0;                            // Vị trí Y của dòng 1 (mặc định ở trên cùng)
int textX2 = panelWidth;                   // Vị trí X của dòng 2
int textY2 = 17;                           // Vị trí Y của dòng 2 (mặc định ở giữa)
int textWidth1;                            // Chiều rộng dòng 1
int textWidth2;                            // Chiều rộng dòng 2
const int charWidth = 8;                   // Chiều rộng của mỗi ký tự
// Biến toàn cục
uint8_t displayMode = 1; // 0: 1 dòng, 1: 2 dòng (mặc định là 2 dòng)
const char *eth_config_file_path = "/eth_config.json";
String ipStr;
String SubnetSrt;
String GWStr;
StaticJsonDocument<1000> doc_config;
int flag_connect_eth = 0;

AsyncWebServer server(80);
MatrixPanel_I2S_DMA *dma_display = nullptr;
const int brightnessText = 100;

// Global biến dùng để lưu dữ liệu nhận được từ client
String gFontName = "";
int gFontSize = 0;
String gFontMap = "";
String gFontDataRaw = "";
String gUTF8Map = "";

// Các con trỏ dùng để chứa font được tạo từ dữ liệu JSON (sẽ cấp phát tại runtime)
uint8_t *g_customFontData = nullptr;
uint16_t *g_customFontMap = nullptr;
// / Thêm biến toàn cục để lưu version
String currentVersion = "1.0.0"; // Version mặc định
// Hàm callback vẽ pixel trên LED Matrix
void setpx(int16_t x, int16_t y, uint16_t color)
{
  if (dma_display != nullptr)
    dma_display->drawPixel(x, y, color);
}
MakeFont myfont(&setpx); // hàm callback setpx được định nghĩa bên dưới
String generateFirmwareHash(const uint8_t *data, size_t len)
{
  MD5Builder md5;
  md5.begin();
  md5.add((uint8_t *)data, len);
  md5.calculate();
  return md5.toString();
}
// Hàm phục vụ file tĩnh từ SPIFFS (ví dụ HTML, jpg…)
// Hàm phục vụ file tĩnh từ SPIFFS
void serveFile(AsyncWebServerRequest *request, String path)
{
  if (SPIFFS.exists(path))
  {
    File file = SPIFFS.open(path, "r");
    String content = file.readString();
    Serial.println("Serving file: " + path);
    Serial.println("Content preview: " + content.substring(0, 100));
    request->send(200, "text/html", content);
    file.close();
  }
  else
  {
    request->send(404, "text/plain", "File not found");
  }
}

// Hàm cập nhật font từ dữ liệu JSON đã lưu vào biến toàn cục
//  – fontDataStr chứa nội dung của mảng uint8_t (định nghĩa f_name)
//  – fontMapStr chứa nội dung của mảng uint16_t (định nghĩa f_map)
void updateFontFromData(String fontDataStr, String fontMapStr)
{
  Serial.println("Updating custom font...");
  // Lấy phần nội dung bên trong dấu { } của fontData
  int start = fontDataStr.indexOf('{');
  int end = fontDataStr.lastIndexOf('}');
  if (start == -1 || end == -1)
  {
    Serial.println("Font data format error.");
    return;
  }
  String dataContent = fontDataStr.substring(start + 1, end);

  // Tương tự cho fontMap
  int mStart = fontMapStr.indexOf('{');
  int mEnd = fontMapStr.lastIndexOf('}');
  if (mStart == -1 || mEnd == -1)
  {
    Serial.println("Font map format error.");
    return;
  }
  String mapContent = fontMapStr.substring(mStart + 1, mEnd);

  // Hàm phụ: đếm số token (số) trong chuỗi, phân tách bởi dấu phẩy
  auto countTokens = [](const String &s) -> int
  {
    int count = 0;
    int pos = 0;
    while (true)
    {
      int commaPos = s.indexOf(',', pos);
      if (commaPos == -1)
      {
        String token = s.substring(pos);
        token.trim();
        if (token.length() > 0)
          count++;
        break;
      }
      else
      {
        String token = s.substring(pos, commaPos);
        token.trim();
        if (token.length() > 0)
          count++;
        pos = commaPos + 1;
      }
    }
    return count;
  };

  int countData = countTokens(dataContent);
  int countMap = countTokens(mapContent);

  // Cấp phát bộ nhớ cho 2 mảng mới
  uint8_t *newFontData = new uint8_t[countData];
  uint16_t *newFontMap = new uint16_t[countMap];

  // Hàm phụ: chuyển chuỗi thành mảng số (cho uint8_t)
  auto parseArray8 = [&](const String &s, uint8_t *arr)
  {
    int pos = 0;
    int idx = 0;
    while (true)
    {
      int commaPos = s.indexOf(',', pos);
      String token;
      if (commaPos == -1)
      {
        token = s.substring(pos);
        token.trim();
        if (token.length() > 0)
        {
          long val = strtol(token.c_str(), NULL, token.startsWith("0x") ? 16 : 10);
          arr[idx++] = (uint8_t)val;
        }
        break;
      }
      else
      {
        token = s.substring(pos, commaPos);
        token.trim();
        if (token.length() > 0)
        {
          long val = strtol(token.c_str(), NULL, token.startsWith("0x") ? 16 : 10);
          arr[idx++] = (uint8_t)val;
        }
        pos = commaPos + 1;
      }
    }
  };

  // Hàm phụ: chuyển chuỗi thành mảng số (cho uint16_t)
  auto parseArray16 = [&](const String &s, uint16_t *arr)
  {
    int pos = 0;
    int idx = 0;
    while (true)
    {
      int commaPos = s.indexOf(',', pos);
      String token;
      if (commaPos == -1)
      {
        token = s.substring(pos);
        token.trim();
        if (token.length() > 0)
        {
          long val = strtol(token.c_str(), NULL, token.startsWith("0x") ? 16 : 10);
          arr[idx++] = (uint16_t)val;
        }
        break;
      }
      else
      {
        token = s.substring(pos, commaPos);
        token.trim();
        if (token.length() > 0)
        {
          long val = strtol(token.c_str(), NULL, token.startsWith("0x") ? 16 : 10);
          arr[idx++] = (uint16_t)val;
        }
        pos = commaPos + 1;
      }
    }
  };

  parseArray8(dataContent, newFontData);
  parseArray16(mapContent, newFontMap);

  // Giải phóng bộ nhớ cũ nếu có
  if (g_customFontData != nullptr)
  {
    delete[] g_customFontData;
  }
  if (g_customFontMap != nullptr)
  {
    delete[] g_customFontMap;
  }
  g_customFontData = newFontData;
  g_customFontMap = newFontMap;

  // Tạo cấu trúc MyFont_typedef mới và cập nhật cho myfont
  MyFont_typedef customFont;
  customFont.f_name = g_customFontData;
  customFont.f_map = g_customFontMap;
  myfont.set_font(customFont);
  Serial.println("Custom font updated successfully.");
}

void loadFontFromSPIFFS()
{
  if (SPIFFS.exists("/font.json"))
  {
    File fontFile = SPIFFS.open("/font.json", "r");
    String jsonData = fontFile.readString();
    fontFile.close();

    DynamicJsonDocument doc(16384);
    DeserializationError error = deserializeJson(doc, jsonData);
    if (error)
    {

      Serial.print("Failed to parse stored font JSON: ");
      Serial.println(error.f_str());
      return;
    }

    if (doc.is<JsonObject>() && doc.size() == 1)
    {
      for (JsonPair kv : doc.as<JsonObject>())
      {
        String fontName = kv.key().c_str();
        JsonObject fontObj = kv.value().as<JsonObject>();
        gFontName = fontName;

        JsonArray fontDataArray = fontObj["font_name"];
        JsonArray fontMapArray = fontObj["font_map"];

        if (!fontDataArray || !fontMapArray)
        {
          Serial.println("Font data format error: Missing font_name or font_map.");
          return;
        }

        if (g_customFontData != nullptr)
          delete[] g_customFontData;
        if (g_customFontMap != nullptr)
          delete[] g_customFontMap;

        int dataSize = fontDataArray.size();
        g_customFontData = new uint8_t[dataSize];
        for (int i = 0; i < dataSize; i++)
        {
          g_customFontData[i] = fontDataArray[i];
        }

        int mapSize = fontMapArray.size();
        g_customFontMap = new uint16_t[mapSize];
        for (int i = 0; i < mapSize; i++)
        {
          g_customFontMap[i] = fontMapArray[i];
        }

        MyFont_typedef customFont = {g_customFontData, g_customFontMap};
        myfont.set_font(customFont);
        Serial.println("Loaded custom font from SPIFFS.");
      }
    }
    else
    {
      Serial.println("Invalid JSON format in SPIFFS: Expected a single font object.");
    }
  }
}

void loadTextFromSPIFFS()
{
  if (SPIFFS.exists("/text.json"))
  {
    File textFile = SPIFFS.open("/text.json", "r");
    String jsonData = textFile.readString();
    textFile.close();

    DynamicJsonDocument doc(512);
    DeserializationError error = deserializeJson(doc, jsonData);
    if (!error)
    {
      if (doc.containsKey("line1"))
        mainTextLine1 = doc["line1"].as<String>();
      if (doc.containsKey("line2"))
        mainTextLine2 = doc["line2"].as<String>();
      if (doc.containsKey("textX1"))
        textX1 = doc["textX1"].as<int>();
      if (doc.containsKey("textY1"))
        textY1 = doc["textY1"].as<int>();
      if (doc.containsKey("textX2"))
        textX2 = doc["textX2"].as<int>();
      if (doc.containsKey("textY2"))
        textY2 = doc["textY2"].as<int>();

      Serial.println("Loaded text Line 1: " + mainTextLine1);
      Serial.println("Loaded text Line 2: " + mainTextLine2);
      Serial.println("Loaded textX1: " + String(textX1) + ", textY1: " + String(textY1));
      Serial.println("Loaded textX2: " + String(textX2) + ", textY2: " + String(textY2));

      textWidth1 = mainTextLine1.length() * charWidth;
      textWidth2 = mainTextLine2.length() * charWidth;

      if (textWidth1 <= panelWidth && textX1 == panelWidth)
        textX1 = (panelWidth - textWidth1) / 2;
      if (displayMode == 1 && textWidth2 <= panelWidth && textX2 == panelWidth)
        textX2 = (panelWidth - textWidth2) / 2;

      // Nếu ở chế độ 1 dòng, đặt Y1 ở giữa
      if (displayMode == 0 && textY1 == 0)
        textY1 = 16; // Giữa màn hình (32/2)
    }
    else
    {
      Serial.println("Failed to parse text JSON or invalid format.");
    }
  }
  else
  {
    Serial.println("No text file found in SPIFFS. Using default text and positions.");
    textWidth1 = mainTextLine1.length() * charWidth;
    textWidth2 = mainTextLine2.length() * charWidth;
    if (textWidth1 <= panelWidth)
      textX1 = (panelWidth - textWidth1) / 2;
    if (displayMode == 1 && textWidth2 <= panelWidth)
      textX2 = (panelWidth - textWidth2) / 2;
    if (displayMode == 0)
      textY1 = 16; // Mặc định giữa màn hình cho 1 dòng
  }
}

void printCurrentPartition()
{
  const esp_partition_t *partition = esp_ota_get_running_partition();
  Serial.printf("Running from partition: %s\n", partition->label);
}

void rollbackFirmware()
{
  Serial.println("Rolling back firmware...");

  if (SPIFFS.exists("/version.json"))
  {
    Serial.println("Removing /version.json...");
    SPIFFS.remove("/version.json");
  }

  const esp_partition_t *running = esp_ota_get_running_partition();
  const esp_partition_t *next_partition = esp_ota_get_next_update_partition(running);

  if (next_partition != NULL)
  {
    Serial.printf("Rolling back to partition: %s\n", next_partition->label);
    Serial.println("Restarting in 10 seconds...");
    delay(10000); // Chờ 10 giây để kiểm tra log
    esp_ota_set_boot_partition(next_partition);
    ESP.restart();
  }
  else
  {
    Serial.println("No backup firmware found! Continuing to run...");
  }
}

uint32_t getChipID()
{
  uint64_t mac = ESP.getEfuseMac();
  Serial.print("MAC Address: ");
  Serial.println(mac, HEX);

  uint32_t chipId = 0;
  for (int i = 0; i < 17; i = i + 8)
  {
    chipId |= ((mac >> (40 - i)) & 0xff) << i;
  }
  Serial.print("Computed Chip ID: ");
  Serial.println(chipId);
  return chipId;
}

// Nhúng danh sách Chip ID hợp lệ vào section đặc biệt trong firmware
const char *validChipIDsJson = "CHIPID_MARKER:[2021749, 2021748, 1234567]";
const char *versionString = "VERSION_MARKER:1.0.9"; // Nhúng version tĩnh

bool isValidDevice()
{
  String jsonData = String(validChipIDsJson);
  int markerPos = jsonData.indexOf("CHIPID_MARKER:");
  if (markerPos == -1)
  {
    Serial.println("No CHIPID_MARKER found in firmware!");
    return false;
  }
  jsonData = jsonData.substring(markerPos + String("CHIPID_MARKER:").length());

  DynamicJsonDocument doc(512);
  DeserializationError error = deserializeJson(doc, jsonData);
  if (error)
  {
    Serial.println("Failed to parse embedded validChipIDs JSON: " + String(error.c_str()));
    return false;
  }

  JsonArray validChipIDs = doc.as<JsonArray>();
  String deviceChipID = String(getChipID());

  Serial.print("Device Chip ID: ");
  Serial.println(deviceChipID);
  Serial.print("Valid Chip IDs: ");
  for (JsonVariant chipId : validChipIDs)
  {
    Serial.print(chipId.as<uint32_t>());
    Serial.print(", ");
  }
  Serial.println();

  for (JsonVariant chipId : validChipIDs)
  {
    if (deviceChipID == String(chipId.as<uint32_t>()))
    {
      Serial.println("Chip ID matched!");
      return true;
    }
  }
  Serial.println("Chip ID not matched!");
  return false;
}

// Trong hàm saveVersionToSPIFFS
void saveVersionToSPIFFS(const String &firmwareHash)
{
  File versionFile = SPIFFS.open("/version.json", FILE_WRITE);
  if (versionFile)
  {
    DynamicJsonDocument doc(512);
    doc["version"] = currentVersion;
    doc["firmware_hash"] = firmwareHash; // Thêm hash của firmware
    serializeJson(doc, versionFile);
    versionFile.close();
    Serial.println("Version saved to SPIFFS: " + currentVersion + " with hash: " + firmwareHash);
  }
  else
  {
    Serial.println("Failed to save version to SPIFFS!");
  }
}

// Trong hàm loadVersionFromSPIFFS
String loadFirmwareHashFromSPIFFS()
{
  String firmwareHash = "";
  if (SPIFFS.exists("/version.json"))
  {
    File versionFile = SPIFFS.open("/version.json", "r");
    String jsonData = versionFile.readString();
    versionFile.close();

    DynamicJsonDocument doc(512);
    DeserializationError error = deserializeJson(doc, jsonData);
    if (!error && doc.containsKey("version") && doc.containsKey("firmware_hash"))
    {
      currentVersion = doc["version"].as<String>();
      firmwareHash = doc["firmware_hash"].as<String>();
      Serial.println("Loaded version: " + currentVersion + " with hash: " + firmwareHash);
    }
    else
    {
      Serial.println("Failed to parse version JSON or invalid format.");
    }
  }
  else
  {
    saveVersionToSPIFFS("initial_hash"); // Khởi tạo nếu chưa có
  }
  return firmwareHash;
}

void incrementVersion()
{
  int major = 0, minor = 0, patch = 0;
  sscanf(currentVersion.c_str(), "%d.%d.%d", &major, &minor, &patch);
  patch++; // Chỉ tăng patch
  currentVersion = String(major) + "." + String(minor) + "." + String(patch);
  Serial.println("Incremented version to: " + currentVersion);
}
// Task khởi tạo Ethernet
void EthTaskCode(void *pvParameters)
{
  Serial.println("Starting Ethernet in task...");

  Serial.println("Initializing Ethernet...");
  ESP32_ENC_onEvent();
  if (!ETH.begin(MISO_GPIO, MOSI_GPIO, SCK_GPIO, CS_GPIO, INT_GPIO, SPI_CLOCK_MHZ, ETH_SPI_HOST))
  {
    Serial.println("ETH.begin failed!");
    vTaskDelete(NULL);
  }

  Serial.println("Waiting for Ethernet connection...");
  ESP32_ENC_waitForConnect();
  if (ESP32_ENC_isConnected())
  {
    ipStr = ETH.localIP().toString();
    GWStr = ETH.gatewayIP().toString();
    SubnetSrt = ETH.subnetMask().toString();
    flag_connect_eth = 1;
    Serial.println("Ethernet connected! IP: " + ipStr);
  }
  else
  {
    Serial.println("Ethernet connection failed!");
    vTaskDelete(NULL);
  }

  while (1)
    vTaskDelay(1000 / portTICK_PERIOD_MS);
}

void updateTextAlignment()
{
  textWidth1 = mainTextLine1.length() * charWidth;
  textWidth2 = mainTextLine2.length() * charWidth;

  if (alignment == 0) // Left
  {
    textX1 = 0;
    textX2 = 0;
  }
  else if (alignment == 1) // Center
  {
    textX1 = (textWidth1 <= panelWidth) ? ((panelWidth - textWidth1) / 2) : panelWidth;
    textX2 = (textWidth2 <= panelWidth) ? ((panelWidth - textWidth2) / 2) : panelWidth;
  }
  else if (alignment == 2) // Right
  {
    textX1 = (textWidth1 <= panelWidth) ? (panelWidth - textWidth1) : panelWidth;
    textX2 = (textWidth2 <= panelWidth) ? (panelWidth - textWidth2) : panelWidth;
  }
}
// Hàm lưu lịch sử version
void saveVersionToHistory(const String &version, const String &hash)
{
  DynamicJsonDocument doc(2048);
  if (SPIFFS.exists("/version_history.json"))
  {
    File historyFile = SPIFFS.open("/version_history.json", "r");
    if (historyFile)
    {
      DeserializationError error = deserializeJson(doc, historyFile);
      if (error)
      {
        Serial.println("Failed to parse version history: " + String(error.c_str()));
      }
      historyFile.close();
    }
    else
    {
      Serial.println("Failed to open version_history.json for reading!");
    }
  }
  doc[version]["hash"] = hash; // Ghi đè nếu version đã tồn tại
  File historyFile = SPIFFS.open("/version_history.json", FILE_WRITE);
  if (historyFile)
  {
    serializeJson(doc, historyFile);
    historyFile.close();
    Serial.println("Saved to history - Version: " + version + ", Hash: " + hash);
  }
  else
  {
    Serial.println("Failed to save version history to SPIFFS!");
  }
}
void setupServerEndpoints()
{
  server.on("/chipid", HTTP_GET, [](AsyncWebServerRequest *request)
            {
        Serial.println("Received request to /chipid at: " + String(millis()) + "ms");
        DynamicJsonDocument doc(256);
        uint32_t chipId = getChipID();
        doc["chip_id"] = chipId;
        String response;
        serializeJson(doc, response);
        Serial.print("Response sent to /chipid: ");
        Serial.println(response);
        request->send(200, "application/json", response); });

  server.on("/version", HTTP_GET, [](AsyncWebServerRequest *request)
            {
        DynamicJsonDocument doc(256);
        doc["version"] = currentVersion; // Trả về version hiện tại của thiết bị
        String response;
        serializeJson(doc, response);
        request->send(200, "application/json", response); });

  server.on("/update", HTTP_POST, [](AsyncWebServerRequest *request)
            {
      if (!isValidDevice()) {
        request->send(403, "text/plain", "Invalid device! OTA update denied.");
        return;
      } }, [](AsyncWebServerRequest *request, String filename, size_t index, uint8_t *data, size_t len, bool final)
            {
      static MD5Builder md5; // Dùng để tính hash toàn bộ dữ liệu
      static String newFirmwareHash = "";
      static String oldFirmwareHash = loadFirmwareHashFromSPIFFS();

      if (!isValidDevice()) return;

      if (!index) {
        Serial.printf("OTA Update Start: %s\n", filename.c_str());
        if (!Update.begin(UPDATE_SIZE_UNKNOWN)) {
          Update.printError(Serial);
          request->send(500, "text/plain", "OTA could not begin");
          return;
        }
        md5.begin(); // Khởi tạo MD5 cho toàn bộ dữ liệu
      }

      if (len) {
        if (Update.write(data, len) != len) {
          Update.printError(Serial);
          request->send(500, "text/plain", "OTA write failed");
          return;
        }
        md5.add(data, len); // Thêm từng khối dữ liệu vào MD5
      }

      if (final) {
        if (Update.end(true)) {
          md5.calculate(); // Tính hash trên toàn bộ dữ liệu
          newFirmwareHash = md5.toString();
          Serial.printf("OTA Update Success: %u bytes, Hash: %s\n", index + len, newFirmwareHash.c_str());
          Serial.println("Old Hash: " + oldFirmwareHash);

          if (newFirmwareHash == oldFirmwareHash) {
            Serial.println("Firmware unchanged, keeping version: " + currentVersion);
          } else {
            bool foundInHistory = false;
            if (SPIFFS.exists("/version_history.json")) {
              File historyFile = SPIFFS.open("/version_history.json", "r");
              if (historyFile) {
                DynamicJsonDocument doc(2048);
                DeserializationError error = deserializeJson(doc, historyFile);
                if (!error) {
                  for (JsonPair kv : doc.as<JsonObject>()) {
                    if (kv.value()["hash"] == newFirmwareHash) {
                      currentVersion = kv.key().c_str();
                      Serial.println("Restoring old version from history: " + currentVersion);
                      foundInHistory = true;
                      break;
                    }
                  }
                  if (!foundInHistory) {
                    Serial.println("New firmware detected (not in history), incrementing version");
                    incrementVersion();
                    saveVersionToHistory(currentVersion, newFirmwareHash);
                  }
                } else {
                  Serial.println("Failed to parse version history: " + String(error.c_str()));
                }
                historyFile.close();
              } else {
                Serial.println("Failed to open version_history.json!");
              }
            } else {
              Serial.println("No version history found, treating as new firmware");
              incrementVersion();
              saveVersionToHistory(currentVersion, newFirmwareHash);
            }
            saveVersionToSPIFFS(newFirmwareHash);
          }
          request->send(200, "text/plain", "OK");
          delay(1000);
          ESP.restart();
        } else {
          Update.printError(Serial);
          request->send(500, "text/plain", "OTA update failed");
        }
      } });

  server.on("/upload", HTTP_POST, [](AsyncWebServerRequest *request)
            {
Serial.println("Received POST request on /upload");
Serial.println("Content-Length: " + String(request->contentLength()));
Serial.println("Has param 'plain': " + String(request->hasParam("plain", true)));

if (!request->hasParam("plain", true)) {
  Serial.println("No 'plain' parameter found in request");
  request->send(400, "application/json", "{\"error\":\"Empty JSON\"}");
  return;
}

String jsonData = request->getParam("plain", true)->value();
Serial.println("Received JSON (first 500 chars):");
Serial.println(jsonData.substring(0, 500)); // Chỉ in 500 ký tự đầu tiên để tránh tràn Serial

DynamicJsonDocument doc(50000);
DeserializationError error = deserializeJson(doc, jsonData);
if (error) {
  Serial.print("JSON parse error: ");
  Serial.println(error.f_str());
  request->send(400, "application/json", "{\"status\":\"error\",\"message\":\"JSON parse error\"}");
  return;
}

if (doc.is<JsonObject>() && doc.size() == 1) {
  for (JsonPair kv : doc.as<JsonObject>()) {
      String fontName = kv.key().c_str();
      JsonObject fontObj = kv.value().as<JsonObject>();
      gFontName = fontName;

      JsonArray fontDataArray = fontObj["font_name"];
      JsonArray fontMapArray = fontObj["font_map"];

      if (!fontDataArray || !fontMapArray) {
          Serial.println("Font data format error.");
          request->send(400, "application/json", "{\"status\":\"error\",\"message\":\"Missing font_name or font_map\"}");
          return;
      }

      if (g_customFontData != nullptr) delete[] g_customFontData;
      if (g_customFontMap != nullptr) delete[] g_customFontMap;

      int dataSize = fontDataArray.size();
      g_customFontData = new uint8_t[dataSize];
      for (int i = 0; i < dataSize; i++) {
          g_customFontData[i] = fontDataArray[i];
      }

      int mapSize = fontMapArray.size();
      g_customFontMap = new uint16_t[mapSize];
      for (int i = 0; i < mapSize; i++) {
          g_customFontMap[i] = fontMapArray[i];
      }

      MyFont_typedef customFont = {g_customFontData, g_customFontMap};
      myfont.set_font(customFont);

      File fontFile = SPIFFS.open("/font.json", FILE_WRITE);
      if (fontFile) {
          fontFile.print(jsonData);
          fontFile.close();
          Serial.println("JSON saved to SPIFFS.");
      }

      Serial.println("Custom font updated successfully.");
      request->send(200, "application/json", "{\"status\":\"ok\"}");
  }
} else {
  request->send(400, "application/json", "{\"status\":\"error\",\"message\":\"Invalid JSON format\"}");
} });
  server.on("/setText", HTTP_POST, [](AsyncWebServerRequest *request)
            {
    Serial.println("Received POST request on /setText");

    if (!request->hasParam("line1", true) || !request->hasParam("line2", true))
    {
        Serial.println("Missing 'line1' or 'line2' parameter in request");
        request->send(400, "application/json", "{\"error\":\"Missing line1 or line2 parameter\"}");
        return;
    }

    String newTextLine1 = request->getParam("line1", true)->value();
    String newTextLine2 = request->getParam("line2", true)->value();
    Serial.println("Received Line 1: " + newTextLine1);
    Serial.println("Received Line 2: " + newTextLine2);

    // Nhận vị trí X và Y từ request (nếu có)
    int newTextX1 = panelWidth;  // Giá trị mặc định nếu không gửi
    int newTextY1 = 0;           // Giá trị mặc định cho Y1
    int newTextX2 = panelWidth;
    int newTextY2 = 17;          // Giá trị mặc định cho Y2
    if (request->hasParam("textX1", true))
    {
        newTextX1 = request->getParam("textX1", true)->value().toInt();
        Serial.println("Received textX1: " + String(newTextX1));
    }
    if (request->hasParam("textY1", true))
    {
        newTextY1 = request->getParam("textY1", true)->value().toInt();
        Serial.println("Received textY1: " + String(newTextY1));
    }
    if (request->hasParam("textX2", true))
    {
        newTextX2 = request->getParam("textX2", true)->value().toInt();
        Serial.println("Received textX2: " + String(newTextX2));
    }
    if (request->hasParam("textY2", true))
    {
        newTextY2 = request->getParam("textY2", true)->value().toInt();
        Serial.println("Received textY2: " + String(newTextY2));
    }

    // Cập nhật văn bản và vị trí
    mainTextLine1 = newTextLine1;
    mainTextLine2 = newTextLine2;
    textX1 = newTextX1;  // Áp dụng vị trí X1
    textY1 = newTextY1;  // Áp dụng vị trí Y1
    textX2 = newTextX2;  // Áp dụng vị trí X2
    textY2 = newTextY2;  // Áp dụng vị trí Y2

    // Xóa toàn bộ màn hình ngay khi text hoặc vị trí mới được cập nhật
    if (dma_display)
    {
        dma_display->clearScreen();
    }

    // Tính lại textWidth cho cả hai dòng
    textWidth1 = mainTextLine1.length() * charWidth;
    textWidth2 = mainTextLine2.length() * charWidth;

    // Nếu không gửi textX1/textX2 hoặc giá trị mặc định, tự động căn giữa khi text ngắn
    if (newTextX1 == panelWidth && textWidth1 <= panelWidth)
        textX1 = (panelWidth - textWidth1) / 2;
    if (newTextX2 == panelWidth && textWidth2 <= panelWidth)
        textX2 = (panelWidth - textWidth2) / 2;

    // Lưu vào SPIFFS
    File textFile = SPIFFS.open("/text.json", FILE_WRITE);
    if (textFile)
    {
        DynamicJsonDocument doc(512);
        doc["line1"] = mainTextLine1;
        doc["line2"] = mainTextLine2;
        doc["textX1"] = textX1;  // Lưu vị trí X1
        doc["textY1"] = textY1;  // Lưu vị trí Y1
        doc["textX2"] = textX2;  // Lưu vị trí X2
        doc["textY2"] = textY2;  // Lưu vị trí Y2
        serializeJson(doc, textFile);
        textFile.close();
        Serial.println("Text and positions saved to SPIFFS.");
    }
    else
    {
        Serial.println("Failed to save text to SPIFFS!");
    }

    request->send(200, "application/json", "{\"status\":\"ok\",\"message\":\"Text and positions updated\"}"); });

  server.on("/setConfig", HTTP_POST, [](AsyncWebServerRequest *request)
            {
        // Xóa màn hình trước khi áp dụng thay đổi
        if (dma_display)
        {
            dma_display->clearScreen();
        }
    
        // Không bắt buộc line1 và line2, chỉ cập nhật nếu có
        if (request->hasParam("line1", true))
        {
            mainTextLine1 = request->getParam("line1", true)->value();
        }
        if (request->hasParam("line2", true))
        {
            mainTextLine2 = request->getParam("line2", true)->value();
        }
        if (request->hasParam("color1", true))
        {
            String color1 = request->getParam("color1", true)->value();
            int r1, g1, b1;
            sscanf(color1.c_str(), "%d,%d,%d", &r1, &g1, &b1);
            colorLine1 = dma_display->color565(r1, g1, b1);
        }
        if (request->hasParam("color2", true))
        {
            String color2 = request->getParam("color2", true)->value();
            int r2, g2, b2;
            sscanf(color2.c_str(), "%d,%d,%d", &r2, &g2, &b2);
            colorLine2 = dma_display->color565(r2, g2, b2);
        }
        if (request->hasParam("brightness", true))
        {
            brightness = request->getParam("brightness", true)->value().toInt();
            dma_display->setBrightness8(brightness);
        }
        if (request->hasParam("rotation", true))
        {
            rotation = request->getParam("rotation", true)->value().toInt();
            dma_display->setRotation(rotation);
        }
        if (request->hasParam("alignment", true))
        {
            alignment = request->getParam("alignment", true)->value().toInt();
        }
        if (request->hasParam("networkMode", true))
        {
            useDHCP = (request->getParam("networkMode", true)->value().toInt() == 0);
        }
        if (request->hasParam("staticIP", true)) staticIP = request->getParam("staticIP", true)->value();
        if (request->hasParam("gateway", true)) gateway = request->getParam("gateway", true)->value();
        if (request->hasParam("subnet", true)) subnet = request->getParam("subnet", true)->value();
        if (request->hasParam("port", true))
        {
            serverPort = request->getParam("port", true)->value().toInt();
            server.end();
            server = AsyncWebServer(serverPort);
            setupServerEndpoints();
        }
        if (request->hasParam("heartbeat", true)) enableHeartbeat = (request->getParam("heartbeat", true)->value().toInt() == 1);
        if (request->hasParam("heartbeatInterval", true)) heartbeatInterval = request->getParam("heartbeatInterval", true)->value().toInt() * 1000;
        if (request->hasParam("heartbeatServer", true)) heartbeatServer = request->getParam("heartbeatServer", true)->value();
        if (request->hasParam("displayMode", true)) // Thêm xử lý displayMode
        {
            displayMode = request->getParam("displayMode", true)->value().toInt();
        }
    
        updateTextAlignment();
        saveConfigToSPIFFS();
    
        if (enableHeartbeat)
        {
            heartbeatTicker.detach();
            heartbeatTicker.attach_ms(heartbeatInterval, sendHeartbeat);
        }
        else
        {
            heartbeatTicker.detach();
        }
    
        request->send(200, "application/json", "{\"status\":\"ok\"}"); });

  server.on("/version", HTTP_GET, [](AsyncWebServerRequest *request)
            {
      DynamicJsonDocument doc(256);
      doc["version"] = currentVersion;
      String response;
      serializeJson(doc, response);
      request->send(200, "application/json", response); });
}

void saveConfigToSPIFFS()
{
  File configFile = SPIFFS.open("/config.json", FILE_WRITE);
  if (configFile)
  {
    DynamicJsonDocument doc(1024);
    doc["line1"] = mainTextLine1;
    doc["line2"] = mainTextLine2;
    doc["color1"] = String(colorLine1, HEX);
    doc["color2"] = String(colorLine2, HEX);
    doc["brightness"] = brightness;
    doc["rotation"] = rotation;
    doc["alignment"] = alignment;
    doc["networkMode"] = useDHCP ? 0 : 1;
    doc["staticIP"] = staticIP;
    doc["gateway"] = gateway;
    doc["subnet"] = subnet;
    doc["port"] = serverPort;
    doc["heartbeat"] = enableHeartbeat ? 1 : 0;
    doc["heartbeatInterval"] = heartbeatInterval / 1000;
    doc["heartbeatServer"] = heartbeatServer;
    doc["displayMode"] = displayMode; // Lưu displayMode
    serializeJson(doc, configFile);
    configFile.close();
  }
}

void sendHeartbeat()
{
  HTTPClient http;
  String url = "http://" + heartbeatServer + "/heartbeat";

  http.begin(url);                                    // Khởi tạo yêu cầu
  http.addHeader("Content-Type", "application/json"); // Thêm header

  DynamicJsonDocument doc(256);
  doc["chip_id"] = getChipID();
  doc["status"] = "alive";
  String json;
  serializeJson(doc, json);

  int httpCode = http.POST(json); // Gửi yêu cầu POST
  if (httpCode > 0)
  {
    Serial.println("Heartbeat sent to " + url + " - Code: " + String(httpCode));
  }
  else
  {
    Serial.println("Failed to send heartbeat: " + String(http.getString()));
  }

  http.end(); // Giải phóng tài nguyên
}

void loadConfigFromSPIFFS()
{
  if (SPIFFS.exists("/config.json"))
  {
    File configFile = SPIFFS.open("/config.json", "r");
    DynamicJsonDocument doc(1024);
    deserializeJson(doc, configFile);
    configFile.close();

    mainTextLine1 = doc["line1"] | "AIoT JSC SYSTECH";
    mainTextLine2 = doc["line2"] | "123456";
    colorLine1 = strtol(doc["color1"] | "FF00FF", NULL, 16); // Mặc định magenta
    colorLine2 = strtol(doc["color2"] | "FF0000", NULL, 16); // Mặc định đỏ
    brightness = doc["brightness"] | 100;
    rotation = doc["rotation"] | 0;
    alignment = doc["alignment"] | 1;
    useDHCP = (doc["networkMode"] | 0) == 0;
    staticIP = doc["staticIP"] | "192.168.1.100";
    gateway = doc["gateway"] | "192.168.1.1";
    subnet = doc["subnet"] | "255.255.255.0";
    serverPort = doc["port"] | 80;
    enableHeartbeat = (doc["heartbeat"] | 0) == 1;
    heartbeatInterval = (doc["heartbeatInterval"] | 10) * 1000;
    heartbeatServer = doc["heartbeatServer"] | "192.168.1.10";
    displayMode = doc["displayMode"] | 1; // Mặc định 2 dòng

    dma_display->setBrightness8(brightness);
    dma_display->setRotation(rotation);
    updateTextAlignment();
  }
}

void setup(void)
{
  Serial.begin(115200);
  if (!SPIFFS.begin(true))
  {
    Serial.println("SPIFFS Mount Failed!");
    return;
  }
  pinMode(RESET_BUTTON_PIN, INPUT);
  if (isValidDevice())
  {
    HUB75_I2S_CFG::i2s_pins _pins = {R1_PIN, G1_PIN, B1_PIN, R2_PIN, G2_PIN, B2_PIN,
                                     A_PIN, B_PIN, C_PIN, D_PIN, E_PIN, LAT_PIN, OE_PIN, CLK_PIN};
    HUB75_I2S_CFG mxconfig(PANEL_RES_X, PANEL_RES_Y, PANEL_CHAIN, _pins);
    mxconfig.i2sspeed = HUB75_I2S_CFG::HZ_10M;
    dma_display = new MatrixPanel_I2S_DMA(mxconfig);
    if (!dma_display->begin())
    {
      Serial.println("Failed to initialize LED Matrix!");
      return;
    }
    dma_display->setBrightness8(brightnessText);
    colorLine1 = dma_display->color565(255, 0, 255); // Cập nhật sau khi dma_display sẵn sàng
    colorLine2 = dma_display->color565(255, 0, 0);
    myfont.set_font(HUYDZZ);
    loadFontFromSPIFFS();
    loadTextFromSPIFFS();
    loadConfigFromSPIFFS();
  }

  xTaskCreatePinnedToCore(EthTaskCode, "EthTask", 20000, NULL, 1, NULL, 1);

  loadFirmwareHashFromSPIFFS();
  setupServerEndpoints();
  server.begin();
  if (enableHeartbeat)
    heartbeatTicker.attach_ms(heartbeatInterval, sendHeartbeat);

  if (!isValidDevice())
  {
    Serial.println("Invalid device! Only OTA allowed.");
    rollbackFirmware();
  }
  Serial.println(getChipID());
  Serial.println(ETH.macAddress());
}

void loop()
{
  int buttonState = digitalRead(RESET_BUTTON_PIN);

  // Xử lý nút reset
  if (buttonState != lastButtonState)
  {
    lastDebounceTime = millis();
  }
  if ((millis() - lastDebounceTime) > debounceDelay)
  {
    if (buttonState == LOW)
    {
      Serial.println("Reset button pressed! Restarting ESP32...");
      ESP.restart();
    }
  }
  lastButtonState = buttonState;

  if (!dma_display)
    return;

  // Nếu chưa kết nối Ethernet, hiển thị "Connecting"
  if (!flag_connect_eth)
  {
    dma_display->clearScreen();
    myfont.print(0, 0, (unsigned char *)"Connecting...", dma_display->color565(255, 255, 255), dma_display->color565(0, 0, 0));
    delay(500);
    Serial.println("Waiting for Ethernet...");
    return;
  }

  // Khi đã kết nối Ethernet
  if (!ipDisplayed && flag_connect_eth)
  {
    dma_display->clearScreen();
    String ipDisplay = "IP: " + ipStr;
    myfont.print(5, 6, (unsigned char *)ipDisplay.c_str(), dma_display->color565(255, 255, 255), dma_display->color565(0, 0, 0));
    delay(5000);
    ipDisplayed = true;
    dma_display->clearScreen();
  }
  else
  {
    bool text1Moved = false;
    bool text2Moved = false;

    // Xử lý dòng 1
    if (textWidth1 > panelWidth)
    {
      textX1 -= 1;
      if (textX1 < -textWidth1)
        textX1 = panelWidth;
      text1Moved = true;
    }

    // Xử lý dòng 2 (chỉ khi ở chế độ 2 dòng)
    if (displayMode == 1 && textWidth2 > panelWidth)
    {
      textX2 -= 1;
      if (textX2 < -textWidth2)
        textX2 = panelWidth;
      text2Moved = true;
    }

    // Xóa vùng hiển thị nếu có thay đổi
    if (text1Moved)
      dma_display->fillRect(0, textY1, panelWidth, 16, dma_display->color565(0, 0, 0));
    if (displayMode == 1 && text2Moved)
      dma_display->fillRect(0, textY2, panelWidth, 16, dma_display->color565(0, 0, 0));

    // Hiển thị văn bản
    myfont.print(textX1, textY1, (unsigned char *)mainTextLine1.c_str(), colorLine1, dma_display->color565(0, 0, 0));
    if (displayMode == 1) // Chỉ hiển thị dòng 2 nếu ở chế độ 2 dòng
      myfont.print(textX2, textY2, (unsigned char *)mainTextLine2.c_str(), colorLine2, dma_display->color565(0, 0, 0));

    delay(50);
  }
}
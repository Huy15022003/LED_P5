using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AIoTFontMaker
{
    public partial class UpdateFirmware : Form
    {
        private string selectedBinFilePath = "";
        private Label statusLabel; // Thêm label để hiển thị trạng thái
        private bool isProcessing = false; // Flag để kiểm soát trạng thái xử lý
        private string loadedFirmwareVersion = "";
        private bool isDeviceCompatible = false;


        public UpdateFirmware()
        {
            InitializeComponent();
            btnUpdateFirm.Enabled = false; // Ban đầu disable nút Update Firmware
            btnLoadFirm.Enabled = true;
            btnCheckDevice.Enabled = true;
            LoadVersion(); // Gọi hàm để lấy version

            // Khởi tạo status label
            InitializeStatusLabel();
        }

        private void InitializeStatusLabel()
        {
            statusLabel = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(20, 380),
                Name = "statusLabel",
                Text = "Trạng thái: Sẵn sàng"
            };
            this.Controls.Add(statusLabel);
        }

        private async void LoadVersion()
        {
            string ipAddress = tbDeviceIpAddress.Text.Trim();
            currentVersion = await GetVersionFromESP32(ipAddress);
            labelVersion.Text = $"Version: {currentVersion}";
        }

        private void SetProcessingState(bool processing, string statusMessage)
        {
            isProcessing = processing;
            btnLoadFirm.Enabled = !processing;
            btnCheckDevice.Enabled = !processing;
            btnUpdateFirm.Enabled = !processing && isDeviceCompatible && (loadedFirmwareVersion != currentVersion);
            statusLabel.Text = $"Trạng Thái: {statusMessage}";
            Application.DoEvents(); // Cập nhật UI ngay lập tức
        }

        private async void btnLoadFirm_Click(object sender, EventArgs e)
        {
            if (isProcessing) return;

            SetProcessingState(true, "Đang tải Firmware...");

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*",
                Title = "Select Firmware File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedBinFilePath = openFileDialog.FileName;
                LoadFirmwareInfo(selectedBinFilePath);
            }

            await Task.Delay(1000); // Delay để người dùng thấy trạng thái
            SetProcessingState(false, "Sẵn sàng");
        }

        private void LoadFirmwareInfo(string binFilePath)
        {
            try
            {
                byte[] firmwareData = File.ReadAllBytes(binFilePath);
                string firmwareDataBase64 = Convert.ToBase64String(firmwareData);

                string firmwareContent = Encoding.UTF8.GetString(firmwareData);
                string marker = "CHIPID_MARKER:";
                int markerIndex = firmwareContent.IndexOf(marker);
                string chipIdList = "Không tìm thấy";

                if (markerIndex != -1)
                {
                    string remainingContent = firmwareContent.Substring(markerIndex + marker.Length);
                    string pattern = @"\[(?:\d+(?:,\s*\d+)*)\]";
                    Match match = Regex.Match(remainingContent, pattern);
                    if (match.Success)
                    {
                        chipIdList = match.Value;
                    }
                }

                // Trích xuất version (giả sử version nằm sau "VERSION_MARKER:")
                string versionMarker = "VERSION_MARKER:";
                int versionIndex = firmwareContent.IndexOf(versionMarker);
                if (versionIndex != -1)
                {
                    string versionContent = firmwareContent.Substring(versionIndex + versionMarker.Length);
                    string versionPattern = @"[\d]+\.[\d]+\.[\d]+"; // Định dạng version: x.y.z
                    Match versionMatch = Regex.Match(versionContent, versionPattern);
                    loadedFirmwareVersion = versionMatch.Success ? versionMatch.Value : "Unknown";
                }
                else
                {
                    loadedFirmwareVersion = "Unknown";
                }

                tbCompany.Text = "AIoT JSC";
                tbDeviceName.Text = "Led Matrix P5";
                tbSerialNumberList.Text = chipIdList;
                tbFirmwareVersion.Text = loadedFirmwareVersion; // Hiển thị version trong TextBox

                var firmwareInfo = new
                {
                    company = tbCompany.Text,
                    chipIdList = chipIdList,
                    deviceName = tbDeviceName.Text,
                    version = loadedFirmwareVersion, // Thêm version vào JSON
                    firmwareData = firmwareDataBase64
                };

                string jsonString = JsonConvert.SerializeObject(firmwareInfo, Formatting.Indented);
                File.WriteAllText("firmware_info.json", jsonString);

                MessageBox.Show("Thông tin Firmware được tải thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải thông tin Firmware: {ex.Message}");
            }
        }

        private async void btnCheckDevice_Click(object sender, EventArgs e)
        {
            if (isProcessing) return;

            string ipAddress = tbDeviceIpAddress.Text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ IP của thiết bị.");
                return;
            }

            if (string.IsNullOrEmpty(selectedBinFilePath))
            {
                MessageBox.Show("Vui lòng tải Firmware trước.");
                return;
            }

            SetProcessingState(true, "Kiểm tra thiết bị...");

            try
            {
                uint deviceChipId;
                using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
                {
                    string url = $"http://{ipAddress}/chipid";
                    HttpResponseMessage response = await client.GetAsync(url);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Không kết nối được thiết bị. Trạng thái: {response.StatusCode}");
                        SetProcessingState(false, "Sẵn sàng");
                        return;
                    }

                    dynamic chipIdData = JsonConvert.DeserializeObject(responseString);
                    if (chipIdData == null || chipIdData.chip_id == null)
                    {
                        MessageBox.Show("Phản hồi không hợp lệ từ thiết bị.");
                        SetProcessingState(false, "Sẵn sàng");
                        return;
                    }

                    deviceChipId = chipIdData.chip_id;
                    tbDeviceSN.Text = deviceChipId.ToString();
                    currentVersion = await GetVersionFromESP32(ipAddress);
                    labelVersion.Text = $"Version: {currentVersion}";
                }

                byte[] firmwareData = File.ReadAllBytes(selectedBinFilePath);
                string firmwareContent = Encoding.UTF8.GetString(firmwareData);
                string marker = "CHIPID_MARKER:";
                int markerIndex = firmwareContent.IndexOf(marker);

                if (markerIndex == -1)
                {
                    MessageBox.Show("Không tìm thấy danh sách ID chip trong Firmware!");
                    SetProcessingState(false, "Sẵn sàng");
                    return;
                }

                string remainingContent = firmwareContent.Substring(markerIndex + marker.Length);
                string pattern = @"\[(?:\d+(?:,\s*\d+)*)\]";
                Match match = Regex.Match(remainingContent, pattern);
                if (!match.Success)
                {
                    MessageBox.Show("Dữ liệu ID chip không hợp lệ trong Firmware!");
                    SetProcessingState(false, "Sẵn sàng");
                    return;
                }

                string chipIdJson = match.Value;
                List<uint> validChipIds = JsonConvert.DeserializeObject<List<uint>>(chipIdJson);

                if (validChipIds.Contains(deviceChipId))
                {
                    if (loadedFirmwareVersion == currentVersion)
                    {
                        MessageBox.Show($"Thiết bị tương thích nhưng cùng version ({currentVersion}). Không cần cập nhật!");
                        isDeviceCompatible = false; // Không cho phép cập nhật nếu version trùng
                        SetProcessingState(false, "Version trùng khớp");
                    }
                    else
                    {
                        MessageBox.Show($"Thiết bị tương thích với Firmware!\nFirmware Version: {loadedFirmwareVersion}\nDevice Version: {currentVersion}");
                        isDeviceCompatible = true;
                        SetProcessingState(false, "Thiết bị tương thích");
                    }
                }
                else
                {
                    MessageBox.Show($"Thiết bị không tương thích với Firmware!");
                    isDeviceCompatible = false;
                    SetProcessingState(false, "Thiết bị không tương thích");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Thiết bị kiểm tra lỗi: {ex.Message}");
                isDeviceCompatible = false;
                SetProcessingState(false, "Đã xảy ra lỗi");
            }
        }

        private async void btnUpdateFirm_Click(object sender, EventArgs e)
        {
            if (isProcessing) return;

            if (string.IsNullOrEmpty(selectedBinFilePath))
            {
                MessageBox.Show("Vui lòng tải Firmware trước.");
                return;
            }

            string ipAddress = tbDeviceIpAddress.Text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ IP của thiết bị.");
                return;
            }

            if (string.IsNullOrEmpty(tbDeviceSN.Text))
            {
                MessageBox.Show("Vui lòng kiểm tra thiết bị trước để lấy số sê-ri.");
                return;
            }

            SetProcessingState(true, "Cập nhật Firmware...");

            try
            {
                byte[] firmwareData = File.ReadAllBytes(selectedBinFilePath);
                string firmwareContent = Encoding.UTF8.GetString(firmwareData);
                string marker = "CHIPID_MARKER:";
                int markerIndex = firmwareContent.IndexOf(marker);

                if (markerIndex == -1)
                {
                    MessageBox.Show("Không tìm thấy danh sách ID chip trong firmware!");
                    SetProcessingState(false, "Sẵn sàng");
                    return;
                }

                string remainingContent = firmwareContent.Substring(markerIndex + marker.Length);
                string pattern = @"\[(?:\d+(?:,\s*\d+)*)\]";
                Match match = Regex.Match(remainingContent, pattern);
                if (!match.Success)
                {
                    MessageBox.Show("Dữ liệu ID chip không hợp lệ trong firmware!");
                    SetProcessingState(false, "Sẵn sàng");
                    return;
                }

                string chipIdJson = match.Value;
                List<uint> validChipIds = JsonConvert.DeserializeObject<List<uint>>(chipIdJson);
                uint deviceChipId = uint.Parse(tbDeviceSN.Text);

                if (!validChipIds.Contains(deviceChipId))
                {
                    MessageBox.Show($"Firmware không tương thích với thiết bị này!");
                    SetProcessingState(false, "Sẵn sàng");
                    return;
                }

                using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(100) })
                {
                    var multipartContent = new MultipartFormDataContent();
                    var byteArrayContent = new ByteArrayContent(firmwareData);
                    byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                    multipartContent.Add(byteArrayContent, "file", Path.GetFileName(selectedBinFilePath));

                    string url = $"http://{ipAddress}/update";
                    HttpResponseMessage response = await client.PostAsync(url, multipartContent);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Firmware cập nhật thành công! Thiết bị đang khởi động lại...");
                        await Task.Delay(5000);
                        currentVersion = await GetVersionFromESP32(ipAddress);
                        labelVersion.Text = $"Version: {currentVersion}";
                        SetProcessingState(false, "Cập nhật hoàn tất");
                    }
                    else
                    {
                        MessageBox.Show($"Firmware cập nhật không thành công!");
                        SetProcessingState(false, "Cập nhật không thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật firmware: {ex.Message}");
                SetProcessingState(false, "Đã xảy ra lỗi");
            }
        }

        private async Task<string> GetVersionFromESP32(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return "Unknown";
            }

            try
            {
                using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
                {
                    string url = $"http://{ipAddress}/version"; // Giả sử port mặc định là 80
                    HttpResponseMessage response = await client.GetAsync(url);
                    string responseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic versionData = JsonConvert.DeserializeObject(responseString);
                        return versionData.version;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi tìm nạp phiên bản: " + ex.Message);
            }
            return "Unknown";
        }

        private void tbCompany_TextChanged(object sender, EventArgs e) { }
        private void tbDeviceName_TextChanged(object sender, EventArgs e) { }
        private void tbSerialNumberList_TextChanged(object sender, EventArgs e) { }
        private void tbDeviceIpAddress_TextChanged(object sender, EventArgs e) { }
        private void tbDeviceSN_TextChanged(object sender, EventArgs e) { }
        private void gbFirmInfo_Enter(object sender, EventArgs e) { }
    }
}
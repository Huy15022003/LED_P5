using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AIoTFontMaker
{
    public partial class frm : Form
    {
        public frm()
        {
            InitializeComponent();
        }

        // Giả sử các biến f_map, f_dat, boma đã được tạo sau khi nhấn btnCreateFont
        private string f_map = "";
        private string f_dat = "";
        private string boma = "";
        private float fontSize = 0;
        private string fontName = "";
        private Font font_select;
        private int starty = 0;
        private int maxHChar = 0;
        private int maxStartHChar = 0;
        private string font_name_clear = "";
        private Thread coding;
        private Thread coding2;
        private static Random random = new Random();

        // Biến lưu trữ màu sắc
        private Color colorLine1 = Color.Red; // Mặc định đỏ cho Line 1
        private Color colorLine2 = Color.Yellow; // Mặc định vàng cho Line 2

        // Các biến khác như fontName… được lấy từ giao diện
        private Label labelDisplayMode;

        private async void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("UTF-8 (VN)");
            comboBox1.SelectedIndex = 0;       
            comboBoxNetworkMode.SelectedIndex = 0; 
            textBoxIP.Enabled = true;
            comboBoxDisplayMode.SelectedIndex = 1; // Mặc định là "Hiển thị 2 dòng"

            textBox1.Text = " ÀÁẢÃẠĂẰẮẲẴẶÂẦẤẨẪẬĐÈÉẺẼẸÊỀẾỂỄỆÌÍ !\"#$%&‘()*+,–./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴàáảãạăằắẳẵặâầấẩẫậđèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵ你好朋友❤°";
            
            numericUpDown1.Value = 4; // Đặt giá trị mặc định là 4 cho "Độ rộng dấu cách"
            draw_font(textBox1.Text, 0);
        }

        private void btnSelectFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = textBox2.Font;
            if (fontName != "" && fontSize != 0f)
            {
                fontDialog.Font = font_select;
            }
            fontDialog.MinSize = 8;  // Kích thước tối thiểu là 8 cho mọi font
            fontDialog.MaxSize = 11; // Kích thước tối đa là 12 (sẽ điều chỉnh sau nếu font không đặc biệt)
            fontDialog.ShowEffects = false;

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                font_select = fontDialog.Font;
                fontName = fontDialog.Font.Name;
                fontSize = fontDialog.Font.Size;

                // Kiểm tra nếu font nằm trong danh sách đặc biệt (cho phép 8-12)
                if (flexibleSizeFonts.Contains(fontName))
                {
                    // Giới hạn kích thước từ 8 đến 12
                    if (fontSize < 8)
                    {
                        fontSize = 8;
                        font_select = new Font(fontName, 8);
                    }
                    else if (fontSize > 11)
                    {
                        fontSize = 11;
                        font_select = new Font(fontName, 11);
                    }
                    textBox3.Text = fontSize.ToString();
                    textBox3.Enabled = true; // Cho phép thay đổi kích thước
                }
                else
                {
                    // Font không đặc biệt, cố định ở size 8
                    fontSize = 8;
                    font_select = new Font(fontName, 8);
                    textBox3.Text = "8";
                    textBox3.Enabled = false; // Vô hiệu hóa ô nhập kích thước
                    MessageBox.Show($"Font '{fontName}' chỉ hỗ trợ kích thước 8.");
                }

                textBox2.Text = fontName;
                Font font = new Font(fontName, textBox2.Font.Size);
                textBox1.Font = font;
                draw_font(textBox1.Text, 0);
            }
        }

        private Bitmap draw_font(string Text, int kt)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            if (kt == 0)
            {
                if (textBox1.Text == "") return bitmap;
                if (textBox3.Text == "") return bitmap;
            }
            pictureBox1.Size = new Size(pictureBox1.Width, pictureBox1.Height);
            try
            {
                PointF point = new PointF(0f, 0f);
                Graphics graphics = Graphics.FromImage(bitmap);
                SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
                graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                Graphics graphics2 = Graphics.FromImage(bitmap);
                Font font = font_select;
                graphics2.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphics2.DrawString(Text, font, Brushes.Black, point);
            }
            catch
            {
                return bitmap;
            }
            int num = bitmap.Width;
            int num2 = bitmap.Height;
            int num3 = 0;
            int num4 = 0;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        if (i < num) num = i;
                        if (j < num2) num2 = j;
                    }
                }
            }
            for (int k = 0; k < bitmap.Width; k++)
            {
                for (int l = 0; l < bitmap.Height; l++)
                {
                    Color pixel2 = bitmap.GetPixel(k, l);
                    if (pixel2.R == 0 && pixel2.G == 0 && pixel2.B == 0)
                    {
                        if (k > num3) num3 = k;
                        if (l > num4) num4 = l;
                    }
                }
            }
            if (num3 == 0 && num4 == 0) // Khi không tìm thấy ký tự đen
            {
                num = 0;
                num2 = 0;
                num3 = short.Parse(numericUpDown1.Text) - 1; // Sử dụng giá trị từ numericUpDown1 (4 - 1 = 3)
                num4 = 0;
            }
            starty = num2;
            if (kt == 0)
            {
                pictureBox1.Image = CropImage(bitmap, num, num2, num3, num4);
            }
            return CropImage(bitmap, num, num2, num3, num4);
        }

        public static Bitmap CropImage(Image source, int x, int y, int x1, int y1)
        {
            int width = x1 - x + 1;
            int height = y1 - y + 1;

            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Invalid crop dimensions.");
            }

            Rectangle srcRect = new Rectangle(x, y, width, height);
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, bitmap.Width, bitmap.Height), srcRect, GraphicsUnit.Pixel);
            }
            return bitmap;
        }


        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(bmp, 0, 0, width, height);
            }
            return bitmap;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fontSize = short.Parse(textBox3.Text);
                draw_font(textBox1.Text, 0);
            }
            catch
            {
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void creat_font()
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            int num = 1;
            int num2 = 0;
            int num3 = 0;
            int num4 = 1;
            byte b = 0;
            Bitmap bitmap = draw_font("Ẩ", 1);
            string text = "";
            text = " PROGMEM ";
            font_name_clear = textBox2.Text.Replace(" ", "_");
            font_name_clear = font_name_clear.Replace("-", "_");
            font_name_clear += RandomString(10);
            stringBuilder.Append("//(C) 2024 TungNT\r\nconst uint16_t " + text + font_name_clear + "_MAP[]={\r\n");
            stringBuilder2.Append("//(C) 2024 TungNT\r\nconst uint8_t " + text + font_name_clear + "[]={\r\n" + maxHChar + ",\r\n");
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                string text2 = textBox1.Text[i].ToString();
                Bitmap bitmap2 = draw_font(text2, 1);
                stringBuilder2.Append(bitmap2.Width + "," + bitmap2.Height + "," + (int)Math.Ceiling((double)bitmap2.Width / 8.0) + "," + starty + ",\r\n");
                num += 4;
                stringBuilder.Append(num + ",");
                for (int j = 0; j < bitmap2.Height; j++)
                {
                    for (int k = 0; k < bitmap2.Width / 8; k++)
                    {
                        b = 0;
                        for (int l = 0; l < 8; l++)
                        {
                            num4 = 1;
                            num3 = ((bitmap2.GetPixel(l + k * 8, j).R == 0) ? 1 : 0);
                            for (int num5 = 7; num5 > l; num5--)
                            {
                                num4 *= 2;
                            }
                            num3 *= num4;
                            b += (byte)num3;
                        }
                        num++;
                        stringBuilder2.Append("0x" + b.ToString("X2") + ",");
                    }
                    if (bitmap2.Width % 8 == 0)
                    {
                        continue;
                    }
                    int num6 = bitmap2.Width / 8;
                    b = 0;
                    for (int m = 0; m < bitmap2.Width % 8; m++)
                    {
                        num4 = 1;
                        num3 = ((bitmap2.GetPixel(m + num6 * 8, j).R == 0) ? 1 : 0);
                        for (int num7 = 7; num7 > m; num7--)
                        {
                            num4 *= 2;
                        }
                        num3 *= num4;
                        b += (byte)num3;
                    }
                    num++;
                    stringBuilder2.Append("0x" + b.ToString("X2") + ",");
                }
                if (text2 == "\\")
                {
                    text2 = " ";
                }
                stringBuilder2.Append("//" + text2 + "\r\n");
                num2++;
                if (num2 % 16 == 0)
                {
                    stringBuilder.Append("\r\n");
                }
            }
            stringBuilder.Append("\r\n};\r\n");
            stringBuilder2.Append("\r\n};\r\n");
            f_map = stringBuilder.ToString();
            f_dat = stringBuilder2.ToString();
        }

        public static string RandomString(int length)
        {
            return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
                               select s[random.Next(s.Length)]).ToArray());
        }

        private void creat_ma()
        {
            int length = textBox1.Text.Length;
            int byteCount = Encoding.UTF8.GetByteCount(textBox1.Text);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                string text = textBox1.Text[i].ToString();
                uint uTF8_HexValue = getUTF8_HexValue(text);
                if (text == "\\")
                {
                    text = "";
                }
                stringBuilder.Append("0x" + uTF8_HexValue.ToString("X8") + ", //" + text.ToString() + "\r\n");
            }
            boma = stringBuilder.ToString();
        }

        private void getMaxHchar()
        {
            maxHChar = 0;
            maxStartHChar = 0;
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                string text = textBox1.Text[i].ToString();
                Bitmap bitmap = draw_font(text, 1);
                if (bitmap.Height > maxHChar)
                {
                    maxHChar = bitmap.Height;
                    maxStartHChar = starty;
                }
            }
        }

        private void btnCreateFont_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                MessageBox.Show("Vui lòng điền tên bộ font của bạn, nó sẽ được sử dụng trong code Arduino IDE");
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Vui lòng chọn bộ mã hoặc tự gõ bộ mã của riêng bạn");
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Vui lòng chọn font kiểu UTF8 có sẵn trong máy tính của bạn");
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Vui lòng chọn font kiểu UTF8 có sẵn trong máy tính của bạn");
                return;
            }

            statusLabel.Text = "Đang tạo font"; // Cập nhật trạng thái
            button1.Enabled = false; // Vô hiệu hóa nút "Gửi font"
            getMaxHchar();
            draw_font(textBox1.Text, 0);
            btnCreateFont.Enabled = false;

            coding = new Thread((ThreadStart)delegate
            {
                creat_font();
                BeginInvoke((MethodInvoker)delegate
                {
                    // Tạo JSON từ dữ liệu font
                    StringBuilder jsonBuilder = new StringBuilder();
                    jsonBuilder.AppendLine("{");
                    jsonBuilder.Append($"  \"{font_name_clear}\": {{\n");

                    List<int> fontData = ParseArrayValues(f_dat);
                    jsonBuilder.Append("    \"font_name\": [");
                    for (int i = 0; i < fontData.Count; i++)
                    {
                        jsonBuilder.Append(fontData[i]);
                        if (i < fontData.Count - 1) jsonBuilder.Append(", ");
                    }
                    jsonBuilder.AppendLine("],");

                    List<int> fontMap = ParseArrayValues(f_map);
                    jsonBuilder.Append("    \"font_map\": [");
                    for (int i = 0; i < fontMap.Count; i++)
                    {
                        jsonBuilder.Append(fontMap[i]);
                        if (i < fontMap.Count - 1) jsonBuilder.Append(", ");
                    }
                    jsonBuilder.AppendLine("],");

                    List<int> utf8Data = ParseArrayValues(boma);
                    jsonBuilder.Append("    \"utf8_data\": [");
                    for (int i = 0; i < utf8Data.Count; i++)
                    {
                        jsonBuilder.Append(utf8Data[i]);
                        if (i < utf8Data.Count - 1) jsonBuilder.Append(", ");
                    }
                    jsonBuilder.AppendLine("]");

                    jsonBuilder.AppendLine("  }");
                    jsonBuilder.AppendLine("}");

                    string jsonString = jsonBuilder.ToString();
                    File.WriteAllText("FontData.json", jsonString);

                    MessageBox.Show($"Font '{font_name_clear}' đã được tạo và lưu vào FontData.json!");
                    btnCreateFont.Enabled = true; // Kích hoạt lại nút "Tạo font"
                    button1.Enabled = true; // Kích hoạt lại nút "Gửi font"
          
                    statusLabel.Text = "Hoàn tất"; // Cập nhật trạng thái khi hoàn tất
                });
            });
            coding.IsBackground = true;
            coding.Start();

            coding2 = new Thread((ThreadStart)delegate
            {
                creat_ma();
            });
            coding2.IsBackground = true;
            coding2.Start();
        }

        private uint getUTF8_HexValue(string c)
        {
            uint num = 0u;
            byte[] bytes = Encoding.UTF8.GetBytes(c);
            for (int i = 0; i < bytes.Length; i++)
            {
                num <<= 8;
                num |= bytes[i];
            }
            return num;
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBox1.SelectedIndex == 0)
        //    {
        //        textBox1.Text = " ÀÁẢÃẠĂẰẮẲẴẶÂẦẤẨẪẬĐÈÉẺẼẸÊỀẾỂỄỆÌÍ !\"#$%&‘()*+,–./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴàáảãạăằắẳẵặâầấẩẫậđèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵ你好朋友❤°";
        //    }
        //    draw_font(textBox1.Text, 0);
        //}
        //private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        //{
        //    if (short.Parse(numericUpDown1.Text) <= 0)
        //    {
        //        numericUpDown1.Text = "0";
        //    }
        //}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (double num = 1.0; num > 0.0; num -= 0.1)
            {
                base.Opacity = num;
                Thread.Sleep(30);
            }
            Application.Exit();
        }

        private void Form1_MinimumSizeChanged(object sender, EventArgs e)
        {
            int num = 5;
            int num2 = base.Location.X;
            int num3 = base.Location.Y;
            for (double num4 = 1.0; num4 > 0.0; num4 -= 0.1)
            {
                base.Opacity = num4;
                SetDesktopLocation(base.Location.X, Control.MousePosition.Y + num);
                num = (int)((double)(num + 15) / num4);
                Thread.Sleep(30);
            }
            SetDesktopLocation(num2, num3);
            base.WindowState = FormWindowState.Minimized;
            base.Opacity = 1.0;
        }

        //private void ExtractFontFromMyFontMaker()
        //{
        //    string filePath = "MyFontMaker.cpp";
        //    if (!File.Exists(filePath))
        //    {
        //        MessageBox.Show("Không tìm thấy tệp MyFontMaker.cpp!");
        //        return;
        //    }

        //    string fileContent = File.ReadAllText(filePath);

        //    var uint8Matches = Regex.Matches(fileContent, @"const\s+uint8_t\s+PROGMEM\s+(\w+)\[\]\s*=\s*\{([\s\S]*?)\};");
        //    var uint16Matches = Regex.Matches(fileContent, @"const\s+uint16_t\s+PROGMEM\s+(\w+)\[\]\s*=\s*\{([\s\S]*?)\};");

        //    if (uint8Matches.Count == 0 || uint16Matches.Count == 0)
        //    {
        //        MessageBox.Show("Không tìm thấy font trong MyFontMaker.cpp!");
        //        return;
        //    }

        //    Match latestUint8Match = uint8Matches[uint8Matches.Count - 1];
        //    Match latestUint16Match = uint16Matches[uint16Matches.Count - 1];

        //    string uint8Name = latestUint8Match.Groups[1].Value;
        //    string uint8Content = latestUint8Match.Groups[2].Value;
        //    string uint16Name = latestUint16Match.Groups[1].Value;
        //    string uint16Content = latestUint16Match.Groups[2].Value;

        //    List<int> fontData = ParseArrayValues(uint8Content);
        //    List<int> fontMap = ParseArrayValues(uint16Content);

        //    StringBuilder jsonBuilder = new StringBuilder();
        //    jsonBuilder.AppendLine("{");
        //    jsonBuilder.Append($"  \"{uint8Name}\": {{\n");

        //    jsonBuilder.Append("    \"font_name\": [");
        //    for (int i = 0; i < fontData.Count; i++)
        //    {
        //        jsonBuilder.Append(fontData[i]);
        //        if (i < fontData.Count - 1) jsonBuilder.Append(", ");
        //    }
        //    jsonBuilder.AppendLine("],");

        //    jsonBuilder.Append("    \"font_map\": [");
        //    for (int i = 0; i < fontMap.Count; i++)
        //    {
        //        jsonBuilder.Append(fontMap[i]);
        //        if (i < fontMap.Count - 1) jsonBuilder.Append(", ");
        //    }
        //    jsonBuilder.AppendLine("]");

        //    jsonBuilder.AppendLine("  }");
        //    jsonBuilder.AppendLine("}");

        //    string jsonString = jsonBuilder.ToString();
        //    File.WriteAllText("FontData.json", jsonString);

        //    MessageBox.Show($"Font '{uint8Name}' đã được trích xuất và lưu vào FontData.json!");
        //}

        private List<int> ParseArrayValues(string arrayContent)
        {
            var values = new List<int>();
            string[] parts = arrayContent.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                string trimmed = part.Trim();
                if (trimmed.StartsWith("0x"))
                {
                    values.Add(Convert.ToInt32(trimmed, 16));
                }
                else if (int.TryParse(trimmed, out int value))
                {
                    values.Add(value);
                }
            }
            return values;
        }
        //private void btnExtractFont_Click(object sender, EventArgs e)
        //{
        //    ExtractFontFromMyFontMaker();
        //}
        // Sự kiện gửi font xuống ESP32 qua HTTP POST
        private async void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists("FontData.json"))
            {
                MessageBox.Show("Không tìm thấy FontData.json. Vui lòng trích xuất font trước.");
                return;
            }

            string ipAddress = textBoxIP.Text.Trim();
            if (string.IsNullOrEmpty(ipAddress))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ IP của ESP32.");
                return;
            }

            if (!Regex.IsMatch(ipAddress, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                MessageBox.Show("Địa chỉ IP không hợp lệ. Vui lòng nhập theo định dạng: xxx.xxx.xxx.xxx");
                return;
            }

            statusLabel.Text = "Đang gửi font"; // Cập nhật trạng thái
            btnCreateFont.Enabled = false; // Vô hiệu hóa nút "Tạo font"
            button1.Enabled = false;


            try
            {
                string jsonData = File.ReadAllText("FontData.json");
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    MessageBox.Show("File FontData.json rỗng. Vui lòng kiểm tra lại.");
                    return;
                }

                //MessageBox.Show($"JSON gửi đi ({jsonData.Length} ký tự): \n" + jsonData.Substring(0, Math.Min(jsonData.Length, 500)) + "...");

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("plain", jsonData)
            });

                    string url = $"http://{ipAddress}/upload";
                    Console.WriteLine($"Sending POST request to: {url}");
                    Console.WriteLine($"Content-Type: {formData.Headers.ContentType}");
                    Console.WriteLine($"Content-Length: {formData.Headers.ContentLength}");

                    client.Timeout = TimeSpan.FromSeconds(60);
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gửi font thành công");
                    statusLabel.Text = "Hoàn tất"; // Cập nhật trạng thái khi gửi xong
                    await Task.Delay(2000); // Chờ 2s
                    statusLabel.Text = "Sẵn sàng"; // Quay về trạng thái ban đầu
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Lỗi HTTP: " + httpEx.Message);
                statusLabel.Text = "Lỗi gửi font"; // Trạng thái khi có lỗi
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi font: " + ex.Message);
                statusLabel.Text = "Lỗi gửi font"; // Trạng thái khi có lỗi
            }
            finally
            {
                button1.Enabled = true;
                btnCreateFont.Enabled = true; // Vô hiệu hóa nút "Tạo font"
            }
        }

        // Sự kiện gửi text đến ESP32
        private async void buttonSendText_Click(object sender, EventArgs e)
        {
            string ipAddress = textBoxIP.Text.Trim();
            if (!ValidateIPAddress(ipAddress)) return;

            string newTextLine1 = textBoxNewText.Text.Trim();
            bool isSingleLine = comboBoxDisplayMode.SelectedIndex == 0;

            if (string.IsNullOrEmpty(newTextLine1))
            {
                MessageBox.Show("Vui lòng nhập text cho dòng 1.");
                return;
            }

            string newTextLine2 = isSingleLine ? "" : textBoxNewTextLine2.Text.Trim();
            if (!isSingleLine && string.IsNullOrEmpty(newTextLine2))
            {
                MessageBox.Show("Vui lòng nhập text cho dòng 2 khi ở chế độ 2 dòng.");
                return;
            }

            int textX1 = (int)numericUpDownTextX1.Value;
            int textY1 = (int)numericUpDownTextY1.Value;
            int textX2 = isSingleLine ? 0 : (int)numericUpDownTextX2.Value;
            int textY2 = isSingleLine ? 0 : (int)numericUpDownTextY2.Value;

            buttonSendText.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("line1", newTextLine1),
                new KeyValuePair<string, string>("line2", newTextLine2),
                new KeyValuePair<string, string>("textX1", textX1.ToString()),
                new KeyValuePair<string, string>("textY1", textY1.ToString()),
                new KeyValuePair<string, string>("textX2", textX2.ToString()),
                new KeyValuePair<string, string>("textY2", textY2.ToString()),
                new KeyValuePair<string, string>("displayMode", isSingleLine ? "0" : "1") // Gửi displayMode
            });

                    string url = $"http://{ipAddress}/setText";
                    Console.WriteLine($"Sending POST request to: {url}");
                    Console.WriteLine($"Content-Type: {formData.Headers.ContentType}");
                    Console.WriteLine($"Content-Length: {formData.Headers.ContentLength}");

                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gửi văn bản và vị trí thành công");
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Lỗi HTTP: " + httpEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi text: " + ex.Message);
            }
            finally
            {
                buttonSendText.Enabled = true;
            }
        }
        // Hàm kiểm tra IP (được tái sử dụng cho tất cả các nút)
        private bool ValidateIPAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                //MessageBox.Show("Vui lòng nhập địa chỉ IP của ESP32.");
                return false;
            }
            return true;
        }

        // Sự kiện gửi màu sắc
        private async void buttonSendColor_Click(object sender, EventArgs e)
        {
            string ipAddress = textBoxIP.Text.Trim();
            if (!ValidateIPAddress(ipAddress)) return;

            buttonSendColor.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("color1", $"{colorLine1.R},{colorLine1.G},{colorLine1.B}"),
                new KeyValuePair<string, string>("color2", $"{colorLine2.R},{colorLine2.G},{colorLine2.B}")
            });

                    string url = $"http://{ipAddress}:{textBoxPort.Text}/setConfig";
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gửi màu thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi màu sắc: " + ex.Message);
            }
            finally
            {
                buttonSendColor.Enabled = true;
            }
        }

        // Sự kiện gửi độ sáng
        private async void buttonSendBrightness_Click(object sender, EventArgs e)
        {
            string ipAddress = textBoxIP.Text.Trim();
            if (!ValidateIPAddress(ipAddress)) return;

            buttonSendBrightness.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("brightness", numericBrightness.Value.ToString())
            });

                    string url = $"http://{ipAddress}:{textBoxPort.Text}/setConfig";
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Gửi độ sáng thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi độ sáng: " + ex.Message);
            }
            finally
            {
                buttonSendBrightness.Enabled = true;
            }
        }

        // Sự kiện gửi hướng xoay
        private async void buttonSendRotation_Click(object sender, EventArgs e)
        {
            string ipAddress = textBoxIP.Text.Trim();
            if (!ValidateIPAddress(ipAddress)) return;

            buttonSendRotation.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("rotation", comboBoxRotation.SelectedIndex.ToString())
            });

                    string url = $"http://{ipAddress}:{textBoxPort.Text}/setConfig";
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Xoay hướng thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi hướng xoay: " + ex.Message);
            }
            finally
            {
                buttonSendRotation.Enabled = true;
            }
        }

        // Sự kiện gửi căn chỉnh
        private async void buttonSendAlignment_Click(object sender, EventArgs e)
        {
            string ipAddress = textBoxIP.Text.Trim();
            if (!ValidateIPAddress(ipAddress)) return;

            buttonSendAlignment.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var formData = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("alignment", comboBoxAlignment.SelectedIndex.ToString())
            });

                    string url = $"http://{ipAddress}:{textBoxPort.Text}/setConfig";
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("Căn chỉnh thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi căn chỉnh: " + ex.Message);
            }
            finally
            {
                buttonSendAlignment.Enabled = true;
            }
        }


        private void comboBoxNetworkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isDHCP = comboBoxNetworkMode.SelectedIndex == 0; // 0 là "DHCP", 1 là "Static"
            bool isStatic = comboBoxNetworkMode.SelectedIndex == 1;

            // Bật/tắt textBoxIP khi chọn DHCP
            textBoxIP.Enabled = isDHCP;
        }

        private void buttonColorLine2_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorLine2 = colorDialog.Color;
                buttonColorLine2.BackColor = colorLine2; // Hiển thị màu đã chọn trên nút
            }
        }

        private void buttonColorLine1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorLine1 = colorDialog.Color;
                buttonColorLine1.BackColor = colorLine1; // Hiển thị màu đã chọn trên nút
            }
        }

        private readonly List<string> flexibleSizeFonts = new List<string>
        {
            "Arial",
            "Times New Roman",
            "Microsoft Sans Serif", // Ví dụ font Microsoft, thêm các font khác nếu cần
            "Microsoft YaHei",
            "Microsoft JhengHei"
        };

        private void comboBoxDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSingleLine = comboBoxDisplayMode.SelectedIndex == 0; // 0: Hiển thị 1 dòng, 1: Hiển thị 2 dòng

            // Bật/tắt ô chỉnh sửa vị trí cho dòng 1
            labelTextX1.Enabled = true;
            numericUpDownTextX1.Enabled = true;
            labelTextY1.Enabled = true;
            numericUpDownTextY1.Enabled = true;

            // Bật/tắt ô chỉnh sửa vị trí cho dòng 2
            labelTextX2.Enabled = !isSingleLine;
            numericUpDownTextX2.Enabled = !isSingleLine;
            labelTextY2.Enabled = !isSingleLine;
            numericUpDownTextY2.Enabled = !isSingleLine;

            // Bật/tắt ô nhập text cho dòng 2
            labelNewTextLine2.Enabled = !isSingleLine;
            textBoxNewTextLine2.Enabled = !isSingleLine;

            // Nếu chọn 1 dòng, xóa nội dung dòng 2 và đặt mặc định Y1 ở giữa
            if (isSingleLine)
            {
                textBoxNewTextLine2.Text = "";
                numericUpDownTextY1.Value = 16; // Đặt Y1 ở giữa (32/2 = 16)
            }
            else
            {
                numericUpDownTextY1.Value = 0;  // Dòng 1 ở trên cùng
                numericUpDownTextY2.Value = 17; // Dòng 2 ở giữa
            }
        }
    }
}

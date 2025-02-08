using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace RenderIssues
{
    public partial class Form1 : Form
    {
        private const PixelFormat PixFormat = PixelFormat.Format32bppArgb;
        private static readonly int[] DPIs = [96, 144];
        private const int DIP = 96;
        private const int ImageWidthDIP = 480;
        private const int ImageHeightDIP = 480;
        private const int ImageCount = 50;
        private const int CharsCount = 16;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCompareImages_Click(object sender, EventArgs e)
        {
            UpdateControls(false, "RUNNING COMPARISON...", null);
            long totalImageCompareCount = 0;
            long totalImageFailCount = 0;
            long totalPixelCompareCount = 0;
            long totalPixelFailCount = 0;
            Exception? exception = null;
            try
            {
                UpdateControls(false, "RENDERING BITMAPS...", null);
                var fontNames = GetFontNames();
                var bitmaps1 = RenderBitmaps(fontNames);
                var bitmaps2 = RenderBitmaps(fontNames);
                totalImageCompareCount = bitmaps1.Count;
                UpdateControls(false, "COMPARING BITMAPS...", null);
                for (int dpiIdx = 0; dpiIdx < totalImageCompareCount; dpiIdx++)
                {
                    using (var bitmap1 = bitmaps1[dpiIdx])
                    {
                        using (var bitmap2 = bitmaps2[dpiIdx])
                        {
                            long pixelCompareCount = 0;
                            long pixelFailCount = 0;
                            var xPixels = bitmap2.Width;
                            var yPixels = bitmap2.Height;
                            var bitmapData1 = bitmap1.LockBits(
                                new Rectangle(0, 0, xPixels, yPixels), ImageLockMode.ReadOnly, PixFormat);
                            var bitmapData2 = bitmap2.LockBits(
                                new Rectangle(0, 0, xPixels, yPixels), ImageLockMode.ReadOnly, PixFormat);
                            var pixelData1 = new byte[bitmapData1.Stride];
                            var pixelData2 = new byte[bitmapData2.Stride];
                            for (int y = 0; y < yPixels; y++)
                            {
                                System.Runtime.InteropServices.Marshal.Copy(bitmapData1.Scan0 + (y * bitmapData1.Stride), pixelData1, 0, bitmapData1.Stride);
                                System.Runtime.InteropServices.Marshal.Copy(bitmapData2.Scan0 + (y * bitmapData2.Stride), pixelData2, 0, bitmapData2.Stride);
                                for (int x = 0; x < xPixels; x++)
                                {
                                    var pixel1 = GetPixel(pixelData1, x);
                                    var pixel2 = GetPixel(pixelData2, x);
                                    pixelCompareCount++;
                                    if (pixel1 != pixel2)
                                    {
                                        if (pixelFailCount++ == 0)
                                            totalImageFailCount++;
                                    }
                                }
                            }
                            bitmap1.UnlockBits(bitmapData1);
                            bitmap2.UnlockBits(bitmapData2);
                            totalPixelCompareCount += pixelCompareCount;
                            totalPixelFailCount += pixelFailCount;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                exception = exc;
            }
            finally
            {
                if (exception != null)
                    UpdateControls(true, "EXCEPTION", exception.Message);
                else
                {
                    UpdateControls(true, "FINISHED COMPARISON",
                        "Images Compared: " + totalImageCompareCount +
                        "\r\nImages Failed: " + totalImageFailCount +
                        "\r\nPixels Compared: " + totalPixelCompareCount +
                        "\r\nPixels Failed: " + totalPixelFailCount);
                }
            }
        }

        private static int GetPixel(byte[] pixelData, int pixelOffset)
        {
            const int PixelWidth = 4;
            return
                (pixelData[pixelOffset * PixelWidth + 3] << 24) +  // A 
                (pixelData[pixelOffset * PixelWidth + 2] << 16) +  // R 
                (pixelData[pixelOffset * PixelWidth + 1] << 8) +   // G
                pixelData[pixelOffset * PixelWidth];               // B
        }

        private static List<Bitmap> RenderBitmaps(List<string> fontNames)
        {
            var bitmaps = new List<Bitmap>(ImageCount * DPIs.Length);
            var fontIdx = 0;
            var codeIdx = 0;
            for (int dpiIdx = 0; dpiIdx < DPIs.Length; dpiIdx++)
            {
                var dpi = DPIs[dpiIdx];
                for (int index = 0; index < ImageCount; index++)
                    bitmaps.Add(RenderBitmap(dpi, fontNames, ref fontIdx, ref codeIdx));
            }
            return bitmaps;
        }

        private static Bitmap RenderBitmap(int dpi, List<string> fontNames, ref int fontIdx, ref int codeIdx)
        {
            var width = ImageWidthDIP * dpi / DIP;
            var height = ImageHeightDIP * dpi / DIP;
            var xIncrement = width / 2;
            var yIncrement = height / 20;
            var margin = 5 * dpi / DIP;
            var bitmap = new Bitmap(width, height, PixFormat);
            var sb = new StringBuilder();
            bitmap.SetResolution(dpi, dpi);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.FillRectangle(SystemBrushes.Window, 0, 0, width, height);
                for (int x = margin; x < width; x += xIncrement)
                {
                    for (int y = margin; y < height; y += yIncrement)
                    {
                        var fontHeight = 11;
                        var fontName = fontNames[fontIdx++];
                        using (var font = new Font(fontName, fontHeight))
                        {
                            if (fontIdx == fontNames.Count)
                                fontIdx = 0;
                            sb.Length = 0;
                            while (sb.Length < CharsCount)
                            {
                                char c = RenderChars[codeIdx++];
                                if (codeIdx == RenderChars.Length)
                                    codeIdx = 0;
                                sb.Append(c);
                            }
                            var text = sb.ToString();
                            g.DrawString(text, font, SystemBrushes.WindowText, x, y, StringFormat.GenericTypographic);
                        }
                    }
                }
            }
            return bitmap;
        }

        private void UpdateControls(bool enabled, string status, string? message)
        {
            var text = "Status: " + status;
            if (message != null)
                text += "\r\n\r\n" + message;
            if (textBoxStatus.Text != text)
                textBoxStatus.Text = text;
            if (buttonCompareImages.Enabled != enabled)
                buttonCompareImages.Enabled = enabled;
            if (buttonExit.Enabled != enabled)
                buttonExit.Enabled = enabled;
            Application.DoEvents();
        }

        private static readonly char[] RenderChars = [
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            'À',
            'Á',
            'Â',
            'Ã',
            'Ä',
            'Å',
            'Æ',
            'Ç',
            'È',
            'É',
            'Ê',
            'Ë',
            'Ì',
            'Í',
            'Î',
            'Ï',
            'Ð',
            'Ñ',
            'Ò',
            'Ó',
            'Ô',
            'Õ',
            'Ö',
            'Ø',
            'Ù',
            'Ú',
            'Û',
            'Ü',
            'Ý',
            'Þ',
            'Ā',
            'Ă',
            'Ą',
            'Ć',
            'Ĉ',
            'Ċ',
            'Č',
            'Ď',
            'Đ',
            'Ē',
            'Ĕ',
            'Ė',
            'Ę',
            'Ě',
            'Ĝ',
            'Ğ',
            'Ġ',
            'Ģ',
            'Ĥ',
            'Ħ',
            'Ĩ',
            'Ī',
            'Ĭ',
            'Į',
            'İ',
            'Ĳ',
            'Ĵ',
            'Ķ',
            'Ĺ',
            'Ļ',
            'Ľ',
            'Ŀ',
            'Ł',
            'Ń',
            'Ņ',
            'Ň',
            'Ŋ',
            'Ō',
            'Ŏ',
            'Ő',
            'Œ',
            'Ŕ',
            'Ŗ',
            'Ř',
        ];

        private static List<string> GetFontNames()
        {
            var fontNames = new List<string>();
            foreach (var fontFamily in FontFamily.Families)
            {
                fontNames.Add(fontFamily.Name);
            }
            return fontNames;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

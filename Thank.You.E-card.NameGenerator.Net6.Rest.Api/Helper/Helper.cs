using System.Drawing;
using System.Net;
using System.Text;

namespace Thank.You.E_card.NameGenerator.Net6.Rest.Api.Helper
{
    public static class Helper
    {
        public static string FindStringBetween(string str, string from, string to)
        {
            int index = str.IndexOf(from);
            if (index == -1)
            {
                return null;
            }
            int num2 = str.IndexOf(to, (int)(index + from.Length));
            return ((num2 == -1) ? str.Substring(index + from.Length) : str.Substring(index + from.Length, (num2 - from.Length) - index));
        }

        public static string getHtmlCode(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Encoding = UTF8Encoding.UTF8;
                string htmlCode = "";

                try
                {
                    htmlCode = client.DownloadString(url);
                }
                catch (Exception)
                {
                }
                return System.Net.WebUtility.HtmlDecode(htmlCode);
            }
        }

        public static Bitmap Base64StringToBitmap(this string
                                            base64String)
        {
            Bitmap bmpReturn = null;

            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);

            memoryStream.Position = 0;

            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;

            return bmpReturn;
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String.Replace("data:image/png;base64,", ""));
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static Image CreateLinkToQrCodeFromSufeinet(string url)
        {
            string getHtmlCodecCode = getHtmlCode("http://tool.sufeinet.com/Creater/QRCodeCreater.aspx?str=" + url);

            string parseWrCode = FindStringBetween(getHtmlCodecCode, "data:image/png;base64", "id=\"");

            string VResult = "data:image/png;base64" +
                             parseWrCode.Replace("\"", "").
                                 Replace("\"", "").
                                 Replace("-", "").

                                 Trim();

            return Base64ToImage(VResult);
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using Thank.You.E_card.NameGenerator.Net6.Rest.Api.Model;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Thank.You.E_card.NameGenerator.Net6.Rest.Api.Controllers
{
    [Route("EcardNameGenerator")]
    [ApiController]
    public class NameGeneratorController : Controller
    {
        private readonly IHostingEnvironment _HostEnvironment;

        public NameGeneratorController(IHostingEnvironment HostEnvironment)
        {
            _HostEnvironment = HostEnvironment;
        }

        [HttpPost]
        public Response<Result> GetEcardNameGenerator([FromBody] CardGenerator nameGenerator)
        {
            Response<Result> retVal = new Response<Result>();

            System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile("thanks_ecard_preview.png"); // set image
            Graphics graphicsImage = Graphics.FromImage(bitmap);

            string Str_TextOnImage = "Sevgili " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.NameSurname);

            if (!String.IsNullOrEmpty(nameGenerator.QrCardVerificationUrl))
            {
                Image ImgQrCode = Helper.Helper.CreateLinkToQrCodeFromSufeinet(nameGenerator.QrCardVerificationUrl);
                graphicsImage.DrawImage(ImgQrCode, 60, 550, 100, 100);
            }

            int font_x = 460;
            int font_y = 275;

            float font_size;

            //MessageBox.Show(Str_TextOnImage.Length.ToString());

            if (Str_TextOnImage.Length <= 29)
            {
                font_size = 25f;
            }
            else if (Str_TextOnImage.Length >= 38)
            {
                font_size = 17f;
            }
            else
            {
                font_size = 20f;
            }

            StringFormat stringformat = new StringFormat();
            stringformat.Alignment = StringAlignment.Center;
            stringformat.LineAlignment = StringAlignment.Center;

            string webRootPath = _HostEnvironment.WebRootPath;
            string contentRootPath = _HostEnvironment.ContentRootPath;

            var foo = new PrivateFontCollection();
            foo.AddFontFile(contentRootPath + "MuseoSlab-900Italic.ttf");

            Font myCustomFont = new Font((FontFamily)foo.Families[0], font_size, GraphicsUnit.Pixel);
            Color StringColor = System.Drawing.ColorTranslator.FromHtml("#00622a");

            graphicsImage.DrawString(Str_TextOnImage, myCustomFont, new SolidBrush(StringColor), new Point(font_x, font_y),
                stringformat);

            bitmap.Save(contentRootPath + "Gift_Card_Cagdas.jpg", ImageFormat.Jpeg);

            retVal.Result = true;
            retVal.ResultCode = 200;
            retVal.Message = "İşlem Başarılı";
            retVal.Comment = " ";
            retVal.Data = new Result
            {
                ImageUrl = contentRootPath + "Gift_Card_Cagdas.jpg",
                Base64Image = "data:image/png;base64," + Convert.ToBase64String(Helper.Helper.ImageToByteArray(bitmap))
            };
            retVal.UpdateTime = DateTime.Now.ToString();

            return retVal;
        }
    }
}
using KeyholeCaptcha.Core;
using KeyholeCaptcha.Core.PhraseGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace KeyholeCaptcha.Web
{
    internal class CaptchaResponse
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class KeyholeCaptchaHandler : IHttpHandler
    {
        private static string badGuess = "Bad guess. Try again or reload image.";
        private static string badGuid = "Too many bad guesses or Captcha Id expired. Try reloading page.";
        private static string badRequest = "Bad request.";

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            NameValueCollection query = context.Request.QueryString;
            string operation = query["operation"];
            string id = query["id"];
            string guess = query["guess"];
            string version = query["version"];

            if (operation == "script")
            {
                // return the script, allowing caching.
                context.Response.ContentType = "application/javascript";
                // stream js file from resource
                byte[] response = Encoding.UTF8.GetBytes(KeyholeCaptcha.resources.Script);
                context.Response.OutputStream.Write(response, 0, response.Length);
                return;
            }

            // disable caching for all others
            HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.MinValue);

            CaptchaResponse responsePayload = new CaptchaResponse();

            switch (operation)
            {
                // register a new, unique Id and return it to the client
                case "register":
                    context.Response.ContentType = "application/json";
                    responsePayload.Id = Validator.Register();
                    responsePayload.Success = true;
                    break;

                // ensure the guid is one we generated.
                // generate a phrase, associate it with the guid, and return a captcha image
                case "reload":
                    // check inputs
                    if (!Arg(id))
                    {
                        context.Response.ContentType = "application/json";
                        responsePayload.Success = false;
                        responsePayload.Message = badRequest;
                    }
                    else if (Validator.IsActiveGuid(id))
                    {
                        context.Response.ContentType = "image/gif";
                        string phrase = string.Empty;
                        int phraseRandomizer = RandomnessProvider.RandFromRange(0, 99);

                        PhraseGenerator phraseGenerator = null;
                        if (phraseRandomizer >= 50)
                        {
                            phraseGenerator = new WordListPhraseGenerator();
                        }
                        else
                        {
                            phraseGenerator = new RandomTextPhraseGenerator();
                        }
                        phrase = phraseGenerator.RandomPhrase();

                        CaptchaMaker.CaptchaType type = (CaptchaMaker.CaptchaType)RandomnessProvider.RandFromRange(0,3);
                        CaptchaMaker maker = new CaptchaMaker();
                        Validator.Refresh(id, phrase);
                        maker.MakeCaptcha(context.Response.OutputStream,phrase,type);
                        return;
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        responsePayload.Success = false;
                        responsePayload.Message = badGuid;
                    }
                    break;

                // check the user's guess against the diaplayed phrase
                case "validate":
                    context.Response.ContentType = "application/json";
                    // check inputs
                    if (!Arg(id) || !Arg(guess))
                    {
                        responsePayload.Success = false;
                        responsePayload.Message = badRequest;
                    }
                    else if (Validator.ValidateUserInput(id, guess))
                    {
                        responsePayload.Success = true;
                    }
                    else
                    {
                        responsePayload.Success = false;
                        if (Validator.IsActiveGuid(id))
                        {
                            responsePayload.Message = badGuess;
                        }
                        else
                        {
                            responsePayload.Message = badGuid;
                        }

                    }
                    break;

                default:
                    context.Response.ContentType = "application/json";
                    responsePayload.Success = false;
                    responsePayload.Message = badRequest;
                    break;
            }

            // stream json result
            context.Response.ContentType = "application/json";
            byte[] responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responsePayload));
            context.Response.OutputStream.Write(responseBytes,0,responseBytes.Length);
        }

        private bool Arg(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }
            return true;
        }
    }
}

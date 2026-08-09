using System;
using System.IO;
using AntiCaptcha.ApiResponse;
using AntiCaptcha.Helper;
using Newtonsoft.Json.Linq;

namespace AntiCaptcha.Api
{
    /// <summary>
    /// Image captcha with text on it.
    /// https://anti-captcha.com/apidoc/task-types/ImageToTextTask
    /// </summary>
    public class ImageToText : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public enum NumericOption
        {
            NoRequirements = 0,
            NumbersOnly = 1,
            AnyLettersExceptNumbers = 2
        }

        /// <summary>
        /// Captcha image encoded in base64.
        /// </summary>
        public string BodyBase64 { get; set; } = "";

        /// <summary>
        /// Path to the captcha image file. Setting it fills <see cref="BodyBase64" />.
        /// </summary>
        public string FilePath
        {
            set
            {
                if (!File.Exists(value))
                {
                    DebugHelper.Out("File " + value + " not found", DebugHelper.Type.Error);

                    return;
                }

                BodyBase64 = StringHelper.FileToBase64String(value);
            }
        }

        /// <summary>Set to true if the image contains 2 or more words.</summary>
        public bool Phrase { get; set; }

        /// <summary>Set to true if the answer is case sensitive.</summary>
        public bool Case { get; set; }

        /// <summary>Character set restriction.</summary>
        public NumericOption Numeric { get; set; } = NumericOption.NoRequirements;

        /// <summary>Set to 1 if the answer is the result of a math operation, like 50+5.</summary>
        public int Math { get; set; }

        /// <summary>Minimum answer length, 0 for no limit.</summary>
        public int MinLength { get; set; }

        /// <summary>Maximum answer length, 0 for no limit.</summary>
        public int MaxLength { get; set; }

        /// <summary>Worker language pool: "en" for English, "rn" for Russian and others.</summary>
        public string LanguagePool { get; set; } = "en";

        /// <summary>Optional hint for the worker, for example "type in green characters only".</summary>
        public string Comment { get; set; }

        /// <summary>Optional, lets you group the statistics in the dashboard by website.</summary>
        public Uri WebsiteUrl { get; set; }

        public override JObject GetPostData()
        {
            if (string.IsNullOrEmpty(BodyBase64))
            {
                DebugHelper.Out("Captcha image is not set, use BodyBase64 or FilePath", DebugHelper.Type.Error);

                return null;
            }

            var postData = new JObject
            {
                ["type"] = "ImageToTextTask",
                ["body"] = BodyBase64.Replace("\r", "").Replace("\n", ""),
                ["phrase"] = Phrase,
                ["case"] = Case,
                ["numeric"] = (int)Numeric,
                ["math"] = Math,
                ["minLength"] = MinLength,
                ["maxLength"] = MaxLength,
                ["languagePool"] = LanguagePool
            };

            SetIfNotEmpty(postData, "comment", Comment);
            SetIfNotEmpty(postData, "websiteURL", WebsiteUrl?.ToString());

            return postData;
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo?.Solution;
        }
    }
}

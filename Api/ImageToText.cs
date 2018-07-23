using Anticaptcha_example.ApiResponse;
using Anticaptcha_example.Helper;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Anticaptcha_example.Api
{
    public class ImageToText : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public enum NumericOption
        {
            NoRequirements,
            NumbersOnly,
            AnyLettersExceptNumbers
        }

        public ImageToText()
        {
            BodyBase64 = "";
            Phrase = false;
            Case = false;
            Numeric = NumericOption.NoRequirements;
            Math = 0;
            MinLength = 0;
            MaxLength = 0;
        }

        public string BodyBase64 { private get; set; }

        public string FilePath
        {
            set
            {
                if (!File.Exists(value))
                {
                    DebugHelper.Out("File " + value + " not found", DebugHelper.Type.Error);
                }
                else
                {
                    BodyBase64 = StringHelper.ImageFileToBase64String(value);

                    if (BodyBase64 == null)
                    {
                        DebugHelper.Out(
                            "Could not convert the file " + value + " to base64. Is this an image file?",
                            DebugHelper.Type.Error
                            );
                    }
                }
            }
        }

        public bool Phrase { private get; set; }
        public bool Case { private get; set; }
        public NumericOption Numeric { private get; set; }
        public int Math { private get; set; }
        public int MinLength { private get; set; }
        public int MaxLength { private get; set; }

        public override JObject GetPostData()
        {
            if (string.IsNullOrEmpty(BodyBase64))
            {
                return null;
            }

            return new JObject
            {
                {"type", "ImageToTextTask"},
                {"body", BodyBase64.Replace("\r", "").Replace("\n", "")},
                {"phrase", Phrase},
                {"case", Case},
                {"numeric", Numeric.Equals(NumericOption.NoRequirements) ? 0 : Numeric.Equals(NumericOption.NumbersOnly) ? 1 : 2},
                {"math", Math},
                {"minLength", MinLength},
                {"maxLength", MaxLength}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}
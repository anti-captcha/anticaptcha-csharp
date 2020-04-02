using System.IO;
using Anticaptcha_example.ApiResponse;
using Anticaptcha_example.Helper;
using Newtonsoft.Json.Linq;

namespace Anticaptcha_example.Api
{
    public class SquareCaptcha : AnticaptchaBase, IAnticaptchaTaskProtocol
    {
        public string BodyBase64 { private get; set; }
        public string ObjectName { private get; set; }
        public int RowsCount { private get; set; }
        public int ColumnsCount { private get; set; }

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

        public SquareCaptcha()
        {
            RowsCount = 3;
            ColumnsCount = 3;
        }

        public override JObject GetPostData()
        {
            return new JObject
            {
                {"type", "SquareNetTask"},
                {"body", BodyBase64.Replace("\r", "").Replace("\n", "")},
                {"objectName", ObjectName},
                {"rowsCount", RowsCount},
                {"columnsCount", ColumnsCount}
            };
        }

        public TaskResultResponse.SolutionData GetTaskSolution()
        {
            return TaskInfo.Solution;
        }
    }
}
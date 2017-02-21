using Newtonsoft.Json.Linq;

namespace Anticaptcha_example
{
    public interface IAnticaptchaTaskProtocol
    {
        JObject GetPostData();
        string GetTaskSolution();
    }
}
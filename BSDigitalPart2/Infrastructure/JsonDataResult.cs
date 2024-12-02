using Microsoft.AspNetCore.Mvc;

namespace BSDigitalPart2.Infrastructure
{
    public class JsonDataResult<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public List<string> errors { get; set; }

        public static JsonResult MapResponse(bool success, T data, List<string> errors, int statusCode)
        {
            JsonDataResult<T> jsonDataResult = new JsonDataResult<T>();
            jsonDataResult.success = success;
            jsonDataResult.data = data;
            jsonDataResult.errors = errors;

            JsonResult result = new JsonResult(jsonDataResult);
            result.StatusCode = statusCode;

            return result;
        }
    }
}

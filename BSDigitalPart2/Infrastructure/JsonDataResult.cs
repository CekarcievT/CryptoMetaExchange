using Microsoft.AspNetCore.Mvc;

namespace BSDigitalPart2.Infrastructure
{
    public class JsonDataResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public static JsonResult MapResponse(bool success, T data, List<string> errors, int statusCode)
        {
            JsonDataResult<T> jsonDataResult = new JsonDataResult<T>();
            jsonDataResult.Success = success;
            jsonDataResult.Data = data;
            jsonDataResult.Errors = errors;

            JsonResult result = new JsonResult(jsonDataResult);
            result.StatusCode = statusCode;

            return result;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MiniBook
{
    public static class ApiResultExtensions
    {
        public static IActionResult ErrorResult(this Controller controller, int errorCode, string errorMessage, HttpStatusCode statusCode)
        {
            return JsonResult(new ApiResponse<object>(errorCode, errorMessage), statusCode);
        }

        public static IActionResult ErrorResult(this Controller controller, int errorCode, string errorMessage)
        {
            return JsonResult(new ApiResponse<object>(errorCode, errorMessage), HttpStatusCode.BadRequest);
        }

        public static IActionResult OkResult<T>(this Controller controller, T result)
        {
            return JsonResult(new ApiResponse<T>(result));
        }

        public static IActionResult OkResult(this Controller controller)
        {
            return JsonResult(new ApiResponse<object>(true));
        }

        public static IActionResult OkResult(this Controller controller, object result)
        {
            return JsonResult(new ApiResponse<object>(result));
        }

        private static IActionResult JsonResult(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiJsonResult(result, statusCode);
        }
    }
}

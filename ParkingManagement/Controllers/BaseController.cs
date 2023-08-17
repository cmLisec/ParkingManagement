using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.Dtos;

namespace ParkingManagement.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private bool Is201SuccessStatusCode(int statusCode, bool successCode)
        {
            if (statusCode != 201)
            {
                if (successCode)
                {
                    return HttpMethods.IsPost(base.Request?.Method);
                }

                return false;
            }

            return true;
        }

        private ObjectResult GetProblemDetailsWithExtensions(BaseResponse response)
        {
            ObjectResult obj = Problem(statusCode: response.StatusCode, detail: response.Message);
            ProblemDetails problemDetails = (ProblemDetails)obj.Value;
            if (response.MultitermId.HasValue)
            {
                problemDetails.Extensions.Add("multitermId", response.MultitermId);
            }

            return obj;
        }

        private ActionResult HandleStatusCodeAndProblemDetailsResponse(BaseResponse response)
        {
            if (response.StatusCode >= 400 && response.StatusCode <= 600)
            {
                return GetProblemDetailsWithExtensions(response);
            }

            return StatusCode(response.StatusCode);
        }

        //
        // Summary:
        //     This function process BaseResponse and returns appropriate ActionResult with
        //     Resource.
        //
        // Parameters:
        //   response:
        //     Specify the BaseResponse.
        //
        // Type parameters:
        //   T:
        //     Specify the type of Resource.
        //
        // Returns:
        //     The ActionResult.
        protected ActionResult<T> ReplyBaseResponse<T>(BaseResponse<T> response)
        {
            if (response.StatusCode == 204)
            {
                return (ActionResult<T>)NoContent();
            }

            bool flag = response.IsSuccessStatusCode();
            if (Is201SuccessStatusCode(response.StatusCode, flag))
            {
                return (ActionResult<T>)StatusCode(201, response.Resource);
            }

            if (flag)
            {
                if (response.Resource != null)
                {
                    return (ActionResult<T>)Ok(response.Resource);
                }

                return (ActionResult<T>)Ok();
            }

            if (response.Resource != null)
            {
                return (ActionResult<T>)StatusCode(response.StatusCode, response.Resource);
            }

            return (ActionResult<T>)HandleStatusCodeAndProblemDetailsResponse(response);
        }

        //
        // Summary:
        //     This function process BaseResponse and returns appropriate IActionResult.
        //
        // Parameters:
        //   response:
        //     Specify the BaseResponse.
        //
        // Returns:
        //     The IActionResult.
        protected IActionResult ReplyBaseResponse(BaseResponse response)
        {
            if (response.StatusCode == 204)
            {
                return NoContent();
            }

            bool flag = response.IsSuccessStatusCode();
            if (Is201SuccessStatusCode(response.StatusCode, flag))
            {
                return StatusCode(201);
            }

            if (flag)
            {
                return Ok();
            }

            return HandleStatusCodeAndProblemDetailsResponse(response);
        }
    }
}

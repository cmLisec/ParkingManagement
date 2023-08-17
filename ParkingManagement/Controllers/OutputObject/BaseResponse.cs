using ParkingManagement.Domain.Dtos;

namespace ParkingManagement.Controllers.OutputObject
{
    public class BaseResponse<T> : BaseResponse
    {
        //
        // Summary:
        //     The Resource property specifies the response resource.
        public T Resource { get; set; }

        //
        // Summary:
        //     Constructor for the BaseResponse.
        public BaseResponse()
        {
            Resource = default(T);
        }

        //
        // Summary:
        //     Constructor for the BaseResponse.
        //
        // Parameters:
        //   resource:
        //     Specifies the resource to be set to the response.
        public BaseResponse(T resource)
        {
            Resource = resource;
        }

        //
        // Summary:
        //     Constructor for the BaseResponse.
        //
        // Parameters:
        //   statusCode:
        //     Specifies the status code to be set to the response.
        //
        //   resource:
        //     Specifies the resource to be set to the response.
        public BaseResponse(int statusCode, T resource)
        {
            base.StatusCode = statusCode;
            Resource = resource;
        }

        //
        // Summary:
        //     Constructor for the BaseResponse.
        //
        // Parameters:
        //   message:
        //     Specifies the message to be set to the response.
        //
        //   statusCode:
        //     Specifies the status code to be set to the response.
        //
        //   multitermId:
        //     Specifies the multiterm id to be set to the response.
        public BaseResponse(string message, int statusCode, int? multitermId = null)
            : base(message, statusCode, multitermId)
        {
            Resource = default(T);
        }
    }
}

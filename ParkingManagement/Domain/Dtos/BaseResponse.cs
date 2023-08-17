namespace ParkingManagement.Domain.Dtos
{
    public class BaseResponse
    {
        //
        // Summary:
        //     The StatusCode property specifies the response status code.
        public int StatusCode { get; set; }

        //
        // Summary:
        //     The Message property specifies the response message.
        public string Message { get; set; }

        //
        // Summary:
        //     The MultitermId specifies the multiterm id.
        public int? MultitermId { get; set; }

        //
        // Summary:
        //     This function indicates if the response is successful.
        //
        // Returns:
        //     True if the StatusCode is Status200OK or Status201Created else False.
        public bool IsSuccessStatusCode()
        {
            if (StatusCode != 200)
            {
                return StatusCode == 201;
            }

            return true;
        }

        //
        // Summary:
        //     Constructor for the BaseResponse.
        public BaseResponse()
        {
            StatusCode = 200;
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
        {
            StatusCode = statusCode;
            Message = message;
            MultitermId = multitermId;
        }
    }
}

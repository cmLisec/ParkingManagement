using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Services.v1;

namespace ParkingManagement.Controllers
{
    [Route("v1/api/[Controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly PaymentService _service;
        public PaymentController(PaymentService service)
        {
            _service = service;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PaymentDTO>> AddPaymentAsync([FromBody] PaymentDTO payment)
        {
            BaseResponse<PaymentDTO> response = await _service.AddPaymentAsync(payment).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }
    }
}

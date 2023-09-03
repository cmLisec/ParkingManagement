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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ExpenseHistoryDTO>>> GetAllExpenseAsync()
        {
            BaseResponse<List<ExpenseHistoryDTO>> response = await _service.GetAllExpenseAsync().ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        [HttpPost("settleUp")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SettleUpHistoryDTO>> AddSettleUpAsync([FromBody] SettleUpHistoryDTO settleUp)
        {
            BaseResponse<SettleUpHistoryDTO> response = await _service.AddSettleUpAsync(settleUp).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        [HttpGet("settleUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SettleUpDTO>>> GetSettleUpDetailsAsync(int userId)
        {
            BaseResponse<List<SettleUpDTO>> response = await _service.GetSettleUpDetailsAsync(userId).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        [HttpGet("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionDetailsAsync(int userId)
        {
            BaseResponse<List<TransactionDTO>> response = await _service.GetTransactionDetailsAsync(userId).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }
    }
}

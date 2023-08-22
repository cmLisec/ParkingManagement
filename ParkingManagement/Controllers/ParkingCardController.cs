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
    public class ParkingCardController : BaseController
    {
        private readonly ParkingCardService _service;
        public ParkingCardController(ParkingCardService service) { _service = service; }

        /// <summary>
        /// This function returns list of all parking card available in database
        /// </summary>
        /// <param name="query">Specify Query parameter</param>
        /// <returns>BaseResponse with list of parking card</returns>
        /// <response code="200">Successfully get all parking card</response>
        /// <response code="204">Content Not Available</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ParkingCardDTO>>> GetAllUserAsync()
        {
            BaseResponse<List<ParkingCardDTO>> response = await _service.GetAllParkingCardAsync().ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function returns parking card by its Id
        /// </summary>
        /// <param name="id">Specify Id of parking card</param>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">Successfully get the parking card with Id</response>
        /// <response code="404">parking card with the given Id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParkingCardDTO>> GetUserByIdAsync([FromRoute] int id)
        {
            BaseResponse<ParkingCardDTO> response = await _service.GetParkingCardByIdAsync(id).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function add new parking card to database
        /// </summary>
        /// <param name="parkingCard">Specify parking card object</param>
        /// <returns>BaseResponse with parking card</returns>
        /// <response code="201">Successfully created the parking card</response>
        /// <response code="400">BadRequest</response>
        /// <response code="409">Parking card already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParkingCardDTO>> AddUserAsync([FromBody] ParkingCardDTO parkingCard)
        {
            BaseResponse<ParkingCardDTO> response = await _service.AddParkingCardAsync(parkingCard).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function update existing parking card
        /// </summary>
        /// <param name="id">Specify Id of parking card</param>
        /// <param name="parkingCard">Specify parking card object</param>
        /// <returns>BaseResponse with parking card</returns>
        /// <response code="200">Successfully updated the parking card</response>
        /// <response code="400">BadRequest</response>
        /// <response code="404">Parking card not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParkingCardDTO>> UpdateUserAsync([FromRoute] int id, [FromBody] ParkingCardDTO parkingCard)
        {
            BaseResponse<ParkingCardDTO> response = await _service.UpdateParkingCardAsync(id, parkingCard).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function delete parking card by given id
        /// </summary>
        /// <param name="id">Specify Id of parking card</param>
        /// <returns>BaseResponse with parking card</returns>        
        /// <response code="200">Successfully added the parking card</response>      
        /// <response code="404">Parking card with the given id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParkingCardDTO>> DeleteUserByIdAsync([FromRoute] int id, [FromRoute] int userId)
        {
            BaseResponse<ParkingCardDTO> response = await _service.DeleteParkingCardByIdAsync(id, userId).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }
    }
}

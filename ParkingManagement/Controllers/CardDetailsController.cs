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
    public class CardDetailsController : BaseController
    {
        private readonly CardDetailsService _service;
        public CardDetailsController(CardDetailsService service) { _service = service; }

        /// <summary>
        /// This function returns list of all cards available in database
        /// </summary>
        /// <returns>BaseResponse</returns>
        /// <response code="200">Successfully get all cards</response>
        /// <response code="204">Content Not Available</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CardDetailsDTO>>> GetAllCardsAsync()
        {
            BaseResponse<List<CardDetailsDTO>> response = await _service.GetAllCardsAsync().ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function returns User by its Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">Successfully get the User with Id</response>
        /// <response code="404">User with the given Id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CardDetailsDTO>> GetUserByIdAsync([FromRoute] int id)
        {
            BaseResponse<CardDetailsDTO> response = await _service.GetCardByIdAsync(id).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function add new card to database
        /// </summary>
        /// <param name="card">Specify card object</param>
        /// <returns>BaseResponse</returns>
        /// <response code="201">Successfully created the card</response>
        /// <response code="400">BadRequest</response>
        /// <response code="409">Card already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CardDetailsDTO>> AddCardAsync([FromBody] CardDetailsDTO card)
        {
            BaseResponse<CardDetailsDTO> response = await _service.AddCardAsync(card).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function update existing card
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <param name="card">Specify card object</param>
        /// <returns>BaseResponse</returns>
        /// <response code="200">Successfully updated the card</response>
        /// <response code="400">BadRequest</response>
        /// <response code="404">Card not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CardDetailsDTO>> UpdateCardAsync([FromRoute] int id, [FromBody] CardDetailsDTO card)
        {
            BaseResponse<CardDetailsDTO> response = await _service.UpdateCardAsync(id, card).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function delete card by given id
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <returns>BaseResponse</returns>        
        /// <response code="200">Successfully added the card</response>      
        /// <response code="404">Card with the given id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CardDetailsDTO>> DeleteCardByIdAsync([FromRoute] int id)
        {
            BaseResponse<CardDetailsDTO> response = await _service.DeleteCardByIdAsync(id).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }
    }
}

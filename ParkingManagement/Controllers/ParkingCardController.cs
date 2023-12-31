﻿using Microsoft.AspNetCore.Authorization;
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
        /// <param name="startDate">Specify start date</param>
        /// <param name="endDate">Specify start date</param>
        /// <returns>BaseResponse with list of parking card</returns>
        /// <response code="200">Successfully get all parking card</response>
        /// <response code="204">Content Not Available</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("ParkingCardsAvailable/{startDate}/{endDate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AvailableParkingCardDTO>> GetAvailableParkingCardAsync(DateTime startDate, DateTime endDate)
        {
            BaseResponse<AvailableParkingCardDTO> response = await _service.GetAvailableParkingCardAsync(startDate, endDate).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function returns parking card by its Id
        /// </summary>
        /// <param name="id">Specify Id</param>
        /// <param name="userId">Specify user id</param>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">Successfully get the parking card with Id</response>
        /// <response code="404">parking card with the given Id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParkingCardDTO>> GetParkingCardByIdAsync([FromRoute] int id, [FromRoute] int userId)
        {
            BaseResponse<ParkingCardDTO> response = await _service.GetParkingCardByIdAsync(id, userId).ConfigureAwait(false);
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
        public async Task<ActionResult<List<ParkingCardDTO>>> AddParkingCardAsync(List<ParkingCardDTO> parkingCard)
        {
            BaseResponse<List<ParkingCardDTO>> response = await _service.AddParkingCardAsync(parkingCard).ConfigureAwait(false);
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
        public async Task<ActionResult<ParkingCardDTO>> UpdateParkingCardAsync([FromRoute] int id, [FromBody] ParkingCardDTO parkingCard)
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
        public async Task<ActionResult<ParkingCardDTO>> DeleteParkingCardByIdAsync([FromRoute] int id, [FromRoute] int userId)
        {
            BaseResponse<ParkingCardDTO> response = await _service.DeleteParkingCardByIdAsync(id, userId).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function returns booked parking card history
        /// </summary>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">Successfully get the parking card with Id</response>
        /// <response code="404">parking card with the given Id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("BookedParkingCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ParkingCardDTO>>> GetBookedParkingCardHistory()
        {
            BaseResponse<List<ParkingCardDTO>> response = await _service.GetBookedParkingCardHistory().ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }

        /// <summary>
        /// This function returns booked parking card history for user
        /// </summary>
        /// <returns>BaseResponse with UserDto</returns>
        /// <response code="200">Successfully get the parking card with Id</response>
        /// <response code="404">parking card with the given Id not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("BookedParkingCardForUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ParkingCardDTO>>> GetBookedParkingCardHistoryForUser(int userId)
        {
            BaseResponse<List<ParkingCardDTO>> response = await _service.GetBookedParkingCardHistoryForUser(userId).ConfigureAwait(false);
            return ReplyBaseResponse(response);
        }
    }
}

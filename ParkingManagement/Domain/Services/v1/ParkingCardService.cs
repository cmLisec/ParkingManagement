using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Models;
using ParkingManagement.Domain.Repositories.v1;

namespace ParkingManagement.Domain.Services.v1
{
    public class ParkingCardService
    {
        private readonly ParkingCardRepository _repo;

        private readonly IMapper _mapper;
        public ParkingCardService(ParkingCardRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        /// <summary>
        /// This fuction returns list of all parking card available in database
        /// </summary>
        /// <param name="query">Specify Query parameter</param>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<List<ParkingCardDTO>>> GetAllParkingCardAsync()
        {
            BaseResponse<List<ParkingCard>> response = await _repo.GetAllParkingCardAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<ParkingCardDTO> mappedResponse = _mapper.Map<List<ParkingCard>, List<ParkingCardDTO>>(response.Resource);
                return new BaseResponse<List<ParkingCardDTO>>(mappedResponse);
            }
            return new BaseResponse<List<ParkingCardDTO>>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function returns parking card based on Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>BaseResponse with parking card</returns>
        public async Task<BaseResponse<ParkingCardDTO>> GetParkingCardByIdAsync(int id)
        {
            BaseResponse<ParkingCard> response = await _repo.GetParkingCardByUserIdAsync(id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                ParkingCardDTO mappedResponse = _mapper.Map<ParkingCard, ParkingCardDTO>(response.Resource);
                return new BaseResponse<ParkingCardDTO>(mappedResponse);
            }
            return new BaseResponse<ParkingCardDTO>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function add new parking card to database
        /// </summary>
        /// <param name="parkingCard">Specify parking card</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCardDTO>> AddParkingCardAsync(ParkingCardDTO parkingCard)
        {
            if (parkingCard == null)
                return new BaseResponse<ParkingCardDTO>("", StatusCodes.Status400BadRequest);


            var parkingCardToAdd = _mapper.Map<ParkingCardDTO, ParkingCard>(parkingCard);
            BaseResponse<ParkingCard> response = await _repo.AddParkingCardAsync(parkingCardToAdd).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                ParkingCardDTO mappedResponse = _mapper.Map<ParkingCard, ParkingCardDTO>(response.Resource);
                return new BaseResponse<ParkingCardDTO>(mappedResponse);
            }
            return new BaseResponse<ParkingCardDTO>(response.Message, response.StatusCode);

        }

        /// <summary>
        /// This function update existing parking card
        /// </summary>
        /// <param name="id">Specify Id of parking card</param>
        /// <param name="parkingCard">Specify parking card object</param>
        /// <returns>BaseResponse with parking card</returns>
        public async Task<BaseResponse<ParkingCardDTO>> UpdateParkingCardAsync(int id, ParkingCardDTO parkingCard)
        {
            if (parkingCard == null)
                return new BaseResponse<ParkingCardDTO>("Bad request", StatusCodes.Status400BadRequest);

            var parkingCardToUpdate = _mapper.Map<ParkingCardDTO, ParkingCard>(parkingCard);
            if (parkingCardToUpdate != null)
            {
                parkingCardToUpdate.Id = id;
            }

            BaseResponse<ParkingCard> response = await _repo.UpdateParkingCardAsync(parkingCardToUpdate).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                ParkingCardDTO mappedResponse = _mapper.Map<ParkingCard, ParkingCardDTO>(response.Resource);
                return new BaseResponse<ParkingCardDTO>(mappedResponse);
            }
            return new BaseResponse<ParkingCardDTO>(response.Message, response.StatusCode);

        }

        /// <summary>
        /// This function deletes parking card from Database
        /// </summary>
        /// <param name="id">Specify Id of parking card</param>
        /// <param name="userId">Specify Id of User</param>
        /// <returns>BaseResponse with parking card</returns>
        public async Task<BaseResponse<ParkingCardDTO>> DeleteParkingCardByIdAsync(int id, int userId)
        {
            BaseResponse<ParkingCard> response = await _repo.DeleteParkingCardByIdAsync(id, userId);
            if (response.IsSuccessStatusCode())
            {
                ParkingCardDTO mappedResponse = _mapper.Map<ParkingCard, ParkingCardDTO>(response.Resource);
                return new BaseResponse<ParkingCardDTO>(mappedResponse);
            }
            return new BaseResponse<ParkingCardDTO>(response.Message, response.StatusCode);
        }
    }
}

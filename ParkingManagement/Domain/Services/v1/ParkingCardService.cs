using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Models;
using ParkingManagement.Domain.Repositories.Utilities;
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
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<AvailableParkingCardDTO>> GetAvailableParkingCardAsync(DateTime startDate, DateTime endDate)
        {
            BaseResponse<List<ParkingCard>> response = await _repo.GetAvailableParkingCardAsync(startDate, endDate).ConfigureAwait(false);
            BaseResponse<int> cardCount = await _repo.GetAvailableCardDetailsAsync();
            if (response.IsSuccessStatusCode() && cardCount.IsSuccessStatusCode())
            {
                AvailableParkingCardDTO availableParkingCard = new()
                {
                    AvailableParkingCards = new Dictionary<int, List<DateSlotDTO>>()
                };
                var date = ParkingCardUtility.GetParkingSchedule();

                if (response.Resource.Count == 0)
                {
                    for (int i = 1; i <= cardCount.Resource; i++)
                    {
                        List<DateSlotDTO> children = new();
                        DateSlotDTO child = new()
                        {
                            AvailableSlot = new Dictionary<DateTime, List<TimeSlotDTO>>
                        {
                            { date.start, new List<TimeSlotDTO> { new TimeSlotDTO() { StartDate = date.start, EndDate = date.end } } }
                        }
                        };
                        children.Add(child);
                        availableParkingCard.AvailableParkingCards.Add(i, children);
                    }
                }
                else
                {
                    Dictionary<int, AvailableDateSlotsModel> availableParkingCards = new();

                    ParkingCardUtility.CalculationForAvailableParkingSlots(response, date, availableParkingCards);

                    ParkingCardUtility.AddAvailableParkingSlots(availableParkingCard, availableParkingCards);
                }

                return new BaseResponse<AvailableParkingCardDTO>(availableParkingCard);
            }
            return new BaseResponse<AvailableParkingCardDTO>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function returns parking card based on Id
        /// </summary>
        /// <param name="id">Specify Id</param>
        /// <param name="userId">Specify user id</param>
        /// <returns>BaseResponse with parking card</returns>
        public async Task<BaseResponse<ParkingCardDTO>> GetParkingCardByIdAsync(int id, int userId)
        {
            BaseResponse<ParkingCard> response = await _repo.GetParkingCardByUserIdAsync(id, userId).ConfigureAwait(false);
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
        public async Task<BaseResponse<List<ParkingCardDTO>>> AddParkingCardAsync(List<ParkingCardDTO> parkingCard)
        {
            if (parkingCard.Count == 0)
                return new BaseResponse<List<ParkingCardDTO>>("Bad request", StatusCodes.Status400BadRequest);

            var parkingCardToAdd = _mapper.Map<List<ParkingCardDTO>, List<ParkingCard>>(parkingCard);
            BaseResponse<List<ParkingCard>> response = await _repo.AddParkingCardAsync(parkingCardToAdd).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<ParkingCardDTO> mappedResponse = _mapper.Map<List<ParkingCard>, List<ParkingCardDTO>>(response.Resource);
                return new BaseResponse<List<ParkingCardDTO>>(mappedResponse);
            }
            return new BaseResponse<List<ParkingCardDTO>>(response.Message, response.StatusCode);

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

        /// <summary>
        /// This functions gets booked parking card history
        /// </summary>
        /// <returns>BaseResponse/returns>
        public async Task<BaseResponse<List<ParkingCardDTO>>> GetBookedParkingCardHistory()
        {
            BaseResponse<List<ParkingCard>> response = await _repo.GetBookedParkingCardHistory().ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<ParkingCardDTO> mappedResponse = _mapper.Map<List<ParkingCard>, List<ParkingCardDTO>>(response.Resource);
                return new BaseResponse<List<ParkingCardDTO>>(mappedResponse);
            }
            return new BaseResponse<List<ParkingCardDTO>>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function gets booked parking card history for user
        /// </summary>
        /// <param name="userId">Specify userid</param>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<List<ParkingCardDTO>>> GetBookedParkingCardHistoryForUser(int userId)
        {
            BaseResponse<List<ParkingCard>> response = await _repo.GetBookedParkingCardHistoryForUser(userId).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<ParkingCardDTO> mappedResponse = _mapper.Map<List<ParkingCard>, List<ParkingCardDTO>>(response.Resource);
                return new BaseResponse<List<ParkingCardDTO>>(mappedResponse);
            }
            return new BaseResponse<List<ParkingCardDTO>>(response.Message, response.StatusCode);
        }
    }
}

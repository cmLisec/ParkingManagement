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
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<AvailableParkingCardDTO>> GetAvailableParkingCardAsync(DateTime startDate)
        {
            BaseResponse<List<ParkingCard>> response = await _repo.GetAvailableParkingCardAsync(startDate).ConfigureAwait(false);
            BaseResponse<int> cardCount = await _repo.GetAvailableCardDetailsAsync();
            if (response.IsSuccessStatusCode() && cardCount.IsSuccessStatusCode())
            {
                AvailableParkingCardDTO parent = new()
                {
                    AvailableParkingCards = new Dictionary<int, List<TimeSlotDTO>>()
                };
                var date = GetPArkingSchedule();

                if (response.Resource.Count == 0)
                {
                    for (int i = 1; i <= cardCount.Resource; i++)
                    {
                        List<TimeSlotDTO> children = new List<TimeSlotDTO>();
                        TimeSlotDTO child = new TimeSlotDTO
                        {
                            StartDate = date.start,
                            EndDate = date.end
                        };
                        children.Add(child);
                        parent.AvailableParkingCards.Add(i, children);
                    }
                }
                else
                {
                    Dictionary<int, Dictionary<DateTime, DateTime>> availableParkingCards = new();

                    foreach (var item in response.Resource)
                    {
                        availableParkingCards.TryGetValue(item.CardId, out var values);

                        if (values == null)
                        {
                            availableParkingCards.Add(item.CardId, new Dictionary<DateTime, DateTime>());
                            availableParkingCards[item.CardId].Add(date.start, date.end);
                        }
                        availableParkingCards.TryGetValue(item.CardId, out var value);
                        foreach (var item2 in value.ToList())
                        {
                            if (item.StartDate == item2.Key && item.EndDate >= item2.Value || item.StartDate < item2.Key && item.EndDate > item2.Value || item.StartDate < item2.Key && item.EndDate == item2.Value)
                            {
                                value.Remove(item2.Key);
                            }
                            else if (item.StartDate >= item2.Key && item.StartDate <= item2.Value)
                            {
                                // Exception is in-range of caltimes
                                if (item.StartDate == item2.Key && item.EndDate > item2.Key && item.EndDate < item2.Value)
                                {
                                    value.Remove(item2.Key);
                                    value.Add(item.EndDate, item2.Value);
                                }
                                else if (item.StartDate > item2.Key && item.EndDate == item2.Value)
                                {
                                    value[item2.Key] = item.EndDate;
                                }
                                else if (item.StartDate > item2.Key && item.EndDate >= item2.Value)
                                {
                                    value[item2.Key] = item.EndDate;
                                }
                                else
                                {
                                    value[item2.Key] = item.StartDate;
                                    value.Add(item.EndDate, item2.Value);
                                }
                            }
                            else if (item.StartDate < item2.Key && item.EndDate > item2.Key && item.EndDate < item2.Value)
                            {
                                // Exception starting before caltime and ending inside it
                                value.Remove(item2.Key);
                                value.Add(item.EndDate, item2.Value);
                            }
                            else if (item.StartDate > item2.Key && item.StartDate < item2.Value && item.EndDate > item2.Value)
                            {
                                // Exception starting inside caltime and ending outside it
                                value[item2.Key] = item.StartDate;
                            }
                        }
                    }

                    if (availableParkingCards.Count > 0)
                    {
                        foreach (var item in availableParkingCards)
                        {
                            parent.AvailableParkingCards.TryGetValue(item.Key, out var children);
                            if (children != null)
                            {
                                parent.AvailableParkingCards[item.Key] = children;
                            }
                            else
                            {
                                List<TimeSlotDTO> childrens = new();
                                foreach (var item1 in item.Value)
                                {
                                    TimeSlotDTO child = new TimeSlotDTO();
                                    child.StartDate = item1.Key;
                                    child.EndDate = item1.Value;
                                    childrens.Add(child);
                                }
                                parent.AvailableParkingCards.Add(item.Key, childrens);
                            }
                        }
                    }
                }

                return new BaseResponse<AvailableParkingCardDTO>(parent);
            }
            return new BaseResponse<AvailableParkingCardDTO>(response.Message, response.StatusCode);

        }

        private static (DateTime start, DateTime end) GetPArkingSchedule()
        {
            DateTime s = DateTime.Now;
            TimeSpan ts = new TimeSpan(09, 00, 00);
            s = s.Date + ts;

            DateTime e = DateTime.Now;
            TimeSpan tes = new TimeSpan(18, 00, 00);
            e = e.Date + tes;
            return (s, e);
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
                return new BaseResponse<ParkingCardDTO>("Bad request", StatusCodes.Status400BadRequest);


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
    }
}

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

        public class AvailableDateSlots
        {
            public Dictionary<DateTime, Dictionary<DateTime, DateTime>> AvailableSlot { get; set; }
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
                AvailableParkingCardDTO parent = new()
                {
                    AvailableParkingCards = new Dictionary<int, List<DateSlotDTO>>()
                };
                var date = GetPArkingSchedule();

                if (response.Resource.Count == 0)
                {
                    for (int i = 1; i <= cardCount.Resource; i++)
                    {
                        List<DateSlotDTO> children = new List<DateSlotDTO>();
                        DateSlotDTO child = new DateSlotDTO();
                        child.AvailableSlot = new Dictionary<DateTime, List<TimeSlotDTO>>();
                        child.AvailableSlot.Add(date.start, new List<TimeSlotDTO> { new TimeSlotDTO() { StartDate = date.start, EndDate = date.end } });
                        children.Add(child);
                        parent.AvailableParkingCards.Add(i, children);
                    }
                }
                else
                {
                    Dictionary<int, AvailableDateSlots> availableParkingCards = new();

                    foreach (var item in response.Resource)
                    {
                        date.start = new DateTime(item.StartDate.Date.Year, item.StartDate.Date.Month, item.StartDate.Date.Day, date.start.Hour, date.start.Minute, date.start.Second);
                        date.end = new DateTime(item.EndDate.Date.Year, item.EndDate.Date.Month, item.EndDate.Date.Day, date.end.Hour, date.end.Minute, date.end.Second);
                        availableParkingCards.TryGetValue(item.CardId, out var values);

                        if (values == null)
                        {
                            availableParkingCards.Add(item.CardId, new AvailableDateSlots());
                            availableParkingCards[item.CardId].AvailableSlot = new Dictionary<DateTime, Dictionary<DateTime, DateTime>>();
                            availableParkingCards[item.CardId].AvailableSlot.Add(date.start, new Dictionary<DateTime, DateTime>());//.KeyDate = date.start;
                            availableParkingCards[item.CardId].AvailableSlot[date.start].Add(date.start, date.end);
                        }

                        availableParkingCards.TryGetValue(item.CardId, out var values1);
                        if (values1 != null)
                        {
                            bool key = values1.AvailableSlot.ContainsKey(date.start);
                            if (!key)
                            {
                                availableParkingCards[item.CardId].AvailableSlot.Add(date.start, new Dictionary<DateTime, DateTime>());//.KeyDate = date.start;

                                availableParkingCards[item.CardId].AvailableSlot[date.start].Add(date.start, date.end);
                            }
                        }
                        values1.AvailableSlot.TryGetValue(date.start, out var values2);

                        foreach (var item2 in values2.ToList())
                        {
                            if (item.StartDate == item2.Key && item.EndDate >= item2.Value || item.StartDate < item2.Key && item.EndDate > item2.Value || item.StartDate < item2.Key && item.EndDate == item2.Value)
                            {
                                values2.Remove(item2.Key);
                            }
                            else if (item.StartDate >= item2.Key && item.StartDate <= item2.Value)
                            {
                                // Exception is in-range of caltimes
                                if (item.StartDate == item2.Key && item.EndDate > item2.Key && item.EndDate < item2.Value)
                                {
                                    values2.Remove(item2.Key);
                                    values2.Add(item.EndDate, item2.Value);
                                }
                                else if (item.StartDate > item2.Key && item.EndDate == item2.Value)
                                {
                                    values2[item2.Key] = item.EndDate;
                                }
                                else if (item.StartDate > item2.Key && item.EndDate >= item2.Value)
                                {
                                    values2[item2.Key] = item.EndDate;
                                }
                                else
                                {
                                    values2[item2.Key] = item.StartDate;
                                    values2.Add(item.EndDate, item2.Value);
                                }
                            }
                            else if (item.StartDate < item2.Key && item.EndDate > item2.Key && item.EndDate < item2.Value)
                            {
                                // Exception starting before caltime and ending inside it
                                values2.Remove(item2.Key);
                                values2.Add(item.EndDate, item2.Value);
                            }
                            else if (item.StartDate > item2.Key && item.StartDate < item2.Value && item.EndDate > item2.Value)
                            {
                                // Exception starting inside caltime and ending outside it
                                values2[item2.Key] = item.StartDate;
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
                                List<DateSlotDTO> childrens = new();

                                foreach (var item1 in item.Value.AvailableSlot)
                                {
                                    DateSlotDTO child = new DateSlotDTO();
                                    child.AvailableSlot = new Dictionary<DateTime, List<TimeSlotDTO>>();
                                    child.AvailableSlot.TryGetValue(item1.Key, out var data);
                                    if (data != null)
                                    {
                                        //child.AvailableSlot[item1.Key].Add(new TimeSlotDTO { StartDate = item1. })
                                    }
                                    else
                                    {
                                        foreach (var item2 in item1.Value)
                                        {
                                            child.AvailableSlot.TryGetValue(item1.Key, out var gh);
                                            if (gh == null)
                                            {
                                                child.AvailableSlot.Add(item1.Key, new List<TimeSlotDTO>());
                                            }
                                            child.AvailableSlot[item1.Key].Add(new TimeSlotDTO { StartDate = item2.Key, EndDate = item2.Value });
                                        }


                                        childrens.Add(child);
                                        parent.AvailableParkingCards.TryGetValue(item.Key, out var ty);
                                        if (ty == null)
                                        {
                                            parent.AvailableParkingCards.Add(item.Key, childrens);

                                        }
                                        else
                                        {
                                            parent.AvailableParkingCards[item.Key] = ty;
                                        }
                                    }
                                }
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

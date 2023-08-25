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
        //public class CalendarExceptions
        //{
        //    public DateTime? TimestampFrom { get; set; }
        //    public DateTime? TimestampTo { get; set; }
        //}

        /// <summary>
        /// This fuction returns list of all parking card available in database
        /// </summary>
        /// <param name="query">Specify Query parameter</param>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<AvailableParkingCardDTO>> GetAvailableParkingCardAsync(DateTime startDate)
        {

            //Dictionary<DateTime, DateTime> calTimeDictionary = new Dictionary<DateTime, DateTime>();
            //calTimeDictionary.Add(new DateTime(2023, 08, 22, 09, 00, 00), new DateTime(2023, 08, 22, 18, 00, 00));
            //List<CalendarExceptions> ex = new List<CalendarExceptions>();
            //ex.Add(new CalendarExceptions { TimestampFrom = new DateTime(2023, 08, 22, 10, 00, 00), TimestampTo = new DateTime(2023, 08, 22, 12, 00, 00) });
            //ex.Add(new CalendarExceptions { TimestampFrom = new DateTime(2023, 08, 22, 14, 00, 00), TimestampTo = new DateTime(2023, 08, 22, 16, 00, 00) });

            //foreach (var exe in ex)
            //{
            //    foreach (var keyValuePair in calTimeDictionary.ToList())
            //    {
            //        // Exception is exactly the same as cal times or caltimes is inside the exception
            //        if (exe.TimestampFrom == keyValuePair.Key && exe.TimestampTo >= keyValuePair.Value || exe.TimestampFrom < keyValuePair.Key && exe.TimestampTo > keyValuePair.Value
            //            || exe.TimestampFrom < keyValuePair.Key && exe.TimestampTo == keyValuePair.Value)
            //        {
            //            calTimeDictionary.Remove(keyValuePair.Key);
            //        }
            //        else if (exe.TimestampFrom >= keyValuePair.Key && exe.TimestampFrom <= keyValuePair.Value)
            //        {
            //            // Exception is in-range of caltimes
            //            if (exe.TimestampFrom == keyValuePair.Key && exe.TimestampTo > keyValuePair.Key && exe.TimestampTo < keyValuePair.Value)
            //            {
            //                calTimeDictionary.Remove(keyValuePair.Key);
            //                calTimeDictionary.Add(exe.TimestampTo.Value, keyValuePair.Value);
            //            }
            //            else if (exe.TimestampFrom > keyValuePair.Key && exe.TimestampTo == keyValuePair.Value)
            //            {
            //                calTimeDictionary[keyValuePair.Key] = exe.TimestampFrom.Value;
            //            }
            //            else if (exe.TimestampFrom > keyValuePair.Key && exe.TimestampTo >= keyValuePair.Value)
            //            {
            //                calTimeDictionary[keyValuePair.Key] = exe.TimestampFrom.Value;
            //            }
            //            else
            //            {
            //                calTimeDictionary[keyValuePair.Key] = exe.TimestampFrom.Value;
            //                calTimeDictionary.Add(exe.TimestampTo.Value, keyValuePair.Value);
            //            }
            //        }
            //        else if (exe.TimestampFrom < keyValuePair.Key && exe.TimestampTo > keyValuePair.Key && exe.TimestampTo < keyValuePair.Value)
            //        {
            //            // Exception starting before caltime and ending inside it
            //            calTimeDictionary.Remove(keyValuePair.Key);
            //            calTimeDictionary.Add(exe.TimestampTo.Value, keyValuePair.Value);
            //        }
            //        else if (exe.TimestampFrom > keyValuePair.Key && exe.TimestampFrom < keyValuePair.Value && exe.TimestampTo > keyValuePair.Value)
            //        {
            //            // Exception starting inside caltime and ending outside it
            //            calTimeDictionary[keyValuePair.Key] = exe.TimestampFrom.Value;
            //        }
            //    }
            //}






            BaseResponse<List<ParkingCard>> response = await _repo.GetAvailableParkingCardAsync(startDate).ConfigureAwait(false);
            BaseResponse<int> cardCount = await _repo.GetAvailableCardDetailsAsync();
            if (response.IsSuccessStatusCode() && cardCount.IsSuccessStatusCode())
            {
                List<AvailableParkingCardDTO> availableParkings = new();
                AvailableParkingCardDTO availableParkingCardDTO = new();
                if (response.Resource.Count == 0)
                {
                    //var card = Environment.GetEnvironmentVariable("ParkingCard");

                    //int f = int.Parse(card);
                    for (int i = 0; i <= cardCount.Resource; i++)
                    {
                        DateTime s = DateTime.Now;
                        TimeSpan ts = new TimeSpan(09, 00, 00);
                        s = s.Date + ts;

                        DateTime e = DateTime.Now;
                        TimeSpan tes = new TimeSpan(06, 00, 00);
                        e = e.Date + tes;
                        availableParkings.Add(new AvailableParkingCardDTO { StartDate = s, EndDate = e });
                        availableParkingCardDTO.AvailableParkingCards.Add(i, new Dictionary<DateTime, DateTime>());
                        availableParkingCardDTO.AvailableParkingCards.TryGetValue(i, out var keyValuePairs);
                        keyValuePairs.Add(s, e);
                    }


                }
                else
                {
                    DateTime s = new DateTime(2023, 08, 23, 09, 00, 00);
                    DateTime e = new DateTime(2023, 08, 23, 18, 00, 00);
                    //TimeSpan ts = new TimeSpan(06, 00, 00);
                    //s = s.Date + ts;
                    if (response.Resource.Count <= 5)
                    {
                        for (int i = 0; i < 5 - response.Resource.Count; i++)
                        {
                            //DateTime s = DateTime.Now;
                            //TimeSpan ts = new TimeSpan(09, 00, 00);
                            //s = s.Date + ts;

                            //DateTime e = DateTime.Now;
                            //TimeSpan tes = new TimeSpan(06, 00, 00);
                            //e = e.Date + tes;
                            availableParkings.Add(new AvailableParkingCardDTO { StartDate = s, EndDate = e });
                        }
                    }
                    //Dictionary<int, Dictionary<DateTime, DateTime>> gh = new Dictionary<int, Dictionary<DateTime, DateTime>>();
                    response = new BaseResponse<List<ParkingCard>>();
                    response.Resource = new List<ParkingCard>();
                    response.Resource.Add(new ParkingCard { CardId = 1, StartDate = new DateTime(2023, 08, 23, 10, 00, 00), EndDate = new DateTime(2023, 08, 23, 12, 00, 00, 00) });
                    response.Resource.Add(new ParkingCard { CardId = 1, StartDate = new DateTime(2023, 08, 23, 14, 00, 00), EndDate = new DateTime(2023, 08, 23, 16, 00, 00, 00) });
                    foreach (var item in response.Resource)
                    {
                        availableParkingCardDTO.AvailableParkingCards.TryGetValue(item.CardId, out var values);
                        //if (value != null)
                        //{
                        //    gh[item.CardId] = value;
                        //}
                        if (values == null)
                        {
                            availableParkingCardDTO.AvailableParkingCards.Add(item.CardId, new Dictionary<DateTime, DateTime>());
                            availableParkingCardDTO.AvailableParkingCards[item.CardId].Add(s, e);
                            //value.Add(s, e);
                        }
                        availableParkingCardDTO.AvailableParkingCards.TryGetValue(item.CardId, out var value);
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
                                //else if (item.StartDate > s && item.EndDate < e)
                                //{
                                //    value.Add(item.EndDate, e);
                                //}
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



                        //if (item.EndDate < s && item.StartDate => 9)
                        //{
                        //    availableParkings.Add(new AvailableParkingCardDTO { StartDate = item.EndDate, EndDate = s });
                        //}
                        //else if (item.StartDate > 9 && item.EndDate == s)
                        //{
                        //    availableParkings.Add(new AvailableParkingCardDTO { StartDate = 9, EndDate = item.StartDate })
                        //}
                        //else if (item.StartDate > 9 && item.EndDate < s)
                        //{
                        //    availableParkings.Add(new AvailableParkingCardDTO { StartDate = 9, EndDate = item.StartDate })
                        //    availableParkings.Add(new AvailableParkingCardDTO { StartDate = item.EndDate, EndDate = 6 })

                        //}
                    }
                }



                //List<ParkingCardDTO> mappedResponse = _mapper.Map<List<ParkingCard>, List<ParkingCardDTO>>(response.Resource);
                //List<AvailableParkingCardDTO> availableParkings = new List<AvailableParkingCardDTO>();
                return new BaseResponse<AvailableParkingCardDTO>(availableParkingCardDTO);

                //return new BaseResponse<List<AvailableParkingCardDTO>>(response.Message, response.StatusCode);
            }
            return new BaseResponse<AvailableParkingCardDTO>(response.Message, response.StatusCode);

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

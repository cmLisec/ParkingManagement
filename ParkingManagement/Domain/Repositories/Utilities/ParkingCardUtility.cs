﻿using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Models;

namespace ParkingManagement.Domain.Repositories.Utilities
{
    public class ParkingCardUtility
    {
        public static void AddAvailableParkingSlots(AvailableParkingCardDTO availableParkingCardDTO, Dictionary<int, AvailableDateSlotsModel> availableParkingCards)
        {
            if (availableParkingCards.Count > 0)
            {
                foreach (var availableParkingCard in availableParkingCards)
                {
                    availableParkingCardDTO.AvailableParkingCards.TryGetValue(availableParkingCard.Key, out var dateSlotValue);
                    if (dateSlotValue != null)
                    {
                        availableParkingCardDTO.AvailableParkingCards[availableParkingCard.Key] = dateSlotValue;
                    }
                    else
                    {
                        List<DateSlotDTO> dateSlotList = new();

                        foreach (var availableSlot in availableParkingCard.Value.AvailableSlot)
                        {
                            DateSlotDTO dateSlot = new()
                            {
                                AvailableSlot = new Dictionary<DateTime, List<TimeSlotDTO>>()
                            };
                            dateSlot.AvailableSlot.TryGetValue(availableSlot.Key, out var data);

                            foreach (var item2 in availableSlot.Value)
                            {
                                dateSlot.AvailableSlot.TryGetValue(availableSlot.Key, out var gh);
                                if (gh == null)
                                {
                                    dateSlot.AvailableSlot.Add(availableSlot.Key, new List<TimeSlotDTO>());
                                }
                                dateSlot.AvailableSlot[availableSlot.Key].Add(new TimeSlotDTO { StartDate = item2.Key, EndDate = item2.Value });
                            }

                            dateSlotList.Add(dateSlot);
                            availableParkingCardDTO.AvailableParkingCards.TryGetValue(availableParkingCard.Key, out var ty);
                            if (ty == null)
                            {
                                availableParkingCardDTO.AvailableParkingCards.Add(availableParkingCard.Key, dateSlotList);
                            }
                            else
                            {
                                availableParkingCardDTO.AvailableParkingCards[availableParkingCard.Key] = ty;
                            }
                        }
                    }
                }
            }
        }

        public static void CalculationForAvailableParkingSlots(BaseResponse<List<ParkingCard>> response, (DateTime start, DateTime end) date, Dictionary<int, AvailableDateSlotsModel> availableParkingCards)
        {

            foreach (var parkingCard in response.Resource)
            {
                date.start = new DateTime(parkingCard.StartDate.Date.Year, parkingCard.StartDate.Date.Month, parkingCard.StartDate.Date.Day, date.start.Hour, date.start.Minute, date.start.Second);
                date.end = new DateTime(parkingCard.EndDate.Date.Year, parkingCard.EndDate.Date.Month, parkingCard.EndDate.Date.Day, date.end.Hour, date.end.Minute, date.end.Second);
                availableParkingCards.TryGetValue(parkingCard.CardId, out var values);

                if (values == null)
                {
                    availableParkingCards.Add(parkingCard.CardId, new AvailableDateSlotsModel());
                    availableParkingCards[parkingCard.CardId].AvailableSlot = new Dictionary<DateTime, Dictionary<DateTime, DateTime>>
                    {
                        { date.start, new Dictionary<DateTime, DateTime>() }
                    };
                    availableParkingCards[parkingCard.CardId].AvailableSlot[date.start].Add(date.start, date.end);
                }

                availableParkingCards.TryGetValue(parkingCard.CardId, out var values1);
                if (values1 != null)
                {
                    bool key = values1.AvailableSlot.ContainsKey(date.start);
                    if (!key)
                    {
                        availableParkingCards[parkingCard.CardId].AvailableSlot.Add(date.start, new Dictionary<DateTime, DateTime>());

                        availableParkingCards[parkingCard.CardId].AvailableSlot[date.start].Add(date.start, date.end);
                    }
                }
                values1.AvailableSlot.TryGetValue(date.start, out var values2);

                foreach (var availableSlot in values2.ToList())
                {
                    if (parkingCard.StartDate == availableSlot.Key && parkingCard.EndDate >= availableSlot.Value || parkingCard.StartDate < availableSlot.Key && parkingCard.EndDate > availableSlot.Value || parkingCard.StartDate < availableSlot.Key && parkingCard.EndDate == availableSlot.Value)
                    {
                        values2.Remove(availableSlot.Key);
                    }
                    else if (parkingCard.StartDate >= availableSlot.Key && parkingCard.StartDate <= availableSlot.Value)
                    {
                        if (parkingCard.StartDate == availableSlot.Key && parkingCard.EndDate > availableSlot.Key && parkingCard.EndDate < availableSlot.Value)
                        {
                            values2.Remove(availableSlot.Key);
                            values2.Add(parkingCard.EndDate, availableSlot.Value);
                        }
                        else if (parkingCard.StartDate > availableSlot.Key && parkingCard.EndDate == availableSlot.Value)
                        {
                            values2[availableSlot.Key] = parkingCard.EndDate;
                        }
                        else if (parkingCard.StartDate > availableSlot.Key && parkingCard.EndDate >= availableSlot.Value)
                        {
                            values2[availableSlot.Key] = parkingCard.EndDate;
                        }
                        else
                        {
                            values2[availableSlot.Key] = parkingCard.StartDate;
                            values2.Add(parkingCard.EndDate, availableSlot.Value);
                        }
                    }
                    else if (parkingCard.StartDate < availableSlot.Key && parkingCard.EndDate > availableSlot.Key && parkingCard.EndDate < availableSlot.Value)
                    {
                        values2.Remove(availableSlot.Key);
                        values2.Add(parkingCard.EndDate, availableSlot.Value);
                    }
                    else if (parkingCard.StartDate > availableSlot.Key && parkingCard.StartDate < availableSlot.Value && parkingCard.EndDate > availableSlot.Value)
                    {
                        values2[availableSlot.Key] = parkingCard.StartDate;
                    }
                }
            }
        }

        public static void DefaultParkingSlot(BaseResponse<int> cardCount, AvailableParkingCardDTO availableParkingCard, (DateTime start, DateTime end) date)
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
                availableParkingCard.AvailableParkingCards.TryGetValue(i, out var parkingCard);
                if (parkingCard != null)
                {
                    availableParkingCard.AvailableParkingCards[i].Add(child);
                }
                else
                {
                    availableParkingCard.AvailableParkingCards.Add(i, children);
                }
            }
        }

        public static (DateTime start, DateTime end) GetParkingSchedule()
        {
            DateTime startDate = DateTime.Now;
            TimeSpan startTime = new(09, 00, 00);
            startDate = startDate.Date + startTime;

            DateTime endDate = DateTime.Now;
            TimeSpan endTime = new(18, 00, 00);
            endDate = endDate.Date + endTime;
            return (startDate, endDate);
        }
    }
}

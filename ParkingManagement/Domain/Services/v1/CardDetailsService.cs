using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Models;
using ParkingManagement.Domain.Repositories.v1;

namespace ParkingManagement.Domain.Services.v1
{
    public class CardDetailsService
    {
        private readonly CardDetailsRepository _repo;
        private readonly IMapper _mapper;
        public CardDetailsService(CardDetailsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        /// <summary>
        /// This fuction returns list of all card available in database
        /// </summary>
        /// <returns>Baseresponse with list of cards</returns>
        public async Task<BaseResponse<List<CardDetailsDTO>>> GetAllCardsAsync()
        {
            BaseResponse<List<CardDetails>> response = await _repo.GetAllCardsAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<CardDetailsDTO> mappedResponse = _mapper.Map<List<CardDetails>, List<CardDetailsDTO>>(response.Resource);
                return new BaseResponse<List<CardDetailsDTO>>(mappedResponse);
            }
            return new BaseResponse<List<CardDetailsDTO>>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function returns card based on Id
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<CardDetailsDTO>> GetCardByIdAsync(int id)
        {
            BaseResponse<CardDetails> response = await _repo.GetCardByIdAsync(id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                CardDetailsDTO mappedResponse = _mapper.Map<CardDetails, CardDetailsDTO>(response.Resource);
                return new BaseResponse<CardDetailsDTO>(mappedResponse);
            }
            return new BaseResponse<CardDetailsDTO>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function add new card to database
        /// </summary>
        /// <param name="card">Specify card</param>
        /// <returns>Baseresponse</returns>
        public async Task<BaseResponse<CardDetailsDTO>> AddCardAsync(CardDetailsDTO card)
        {
            if (card == null)
                return new BaseResponse<CardDetailsDTO>("", StatusCodes.Status400BadRequest);


            var cardEntity = _mapper.Map<CardDetailsDTO, CardDetails>(card);
            BaseResponse<CardDetails> response = await _repo.AddCardAsync(cardEntity).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                CardDetailsDTO mappedResponse = _mapper.Map<CardDetails, CardDetailsDTO>(response.Resource);
                return new BaseResponse<CardDetailsDTO>(mappedResponse);
            }
            return new BaseResponse<CardDetailsDTO>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function update existing card
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <param name="card">Specify card object</param>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<CardDetailsDTO>> UpdateCardAsync(int id, CardDetailsDTO card)
        {
            if (card == null)
                return new BaseResponse<CardDetailsDTO>("", StatusCodes.Status400BadRequest);


            var cardEntity = _mapper.Map<CardDetailsDTO, CardDetails>(card);
            if (cardEntity != null)
                cardEntity.Id = id;
            BaseResponse<CardDetails> response = await _repo.UpdateCardAsync(cardEntity).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                CardDetailsDTO mappedResponse = _mapper.Map<CardDetails, CardDetailsDTO>(response.Resource);
                return new BaseResponse<CardDetailsDTO>(mappedResponse);
            }
            return new BaseResponse<CardDetailsDTO>(response.Message, response.StatusCode);

        }

        /// <summary>
        /// This function deletes card from Database
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<CardDetailsDTO>> DeleteCardByIdAsync(int id)
        {
            BaseResponse<CardDetails> response = await _repo.DeleteCardIdAsync(id);
            if (response.IsSuccessStatusCode())
            {
                CardDetailsDTO mappedResponse = _mapper.Map<CardDetails, CardDetailsDTO>(response.Resource);
                return new BaseResponse<CardDetailsDTO>(mappedResponse);
            }
            return new BaseResponse<CardDetailsDTO>(response.Message, response.StatusCode);
        }
    }
}

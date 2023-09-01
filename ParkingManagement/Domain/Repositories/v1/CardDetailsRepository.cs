using Microsoft.EntityFrameworkCore;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.Models;

namespace ParkingManagement.Domain.Repositories.v1
{
    public class CardDetailsRepository : BaseRepository<CardDetails>
    {
        public CardDetailsRepository(ParkingManagementDBContext context) : base(context)
        {
        }

        /// <summary>
        /// This function fetch all cards available from database
        /// </summary>
        /// <returns>Baseresponse with list of cards</returns>
        public async Task<BaseResponse<List<CardDetails>>> GetAllCardsAsync()
        {
            List<CardDetails> cardList = await GetContext().CardDetails.ToListAsync().ConfigureAwait(false);
            if (cardList.Count <= 0)
                return new BaseResponse<List<CardDetails>>("", StatusCodes.Status204NoContent);
            return new BaseResponse<List<CardDetails>>(cardList);
        }

        /// <summary>
        /// This function fetch card from database based on Id
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <returns>Baseresponse with card</returns>
        public async Task<BaseResponse<CardDetails>> GetCardByIdAsync(int id)
        {
            CardDetails card = await GetContext().CardDetails.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (card == null)
                return new BaseResponse<CardDetails>("", StatusCodes.Status404NotFound);
            return new BaseResponse<CardDetails>(card);
        }

        /// <summary>
        /// This function add new card to database
        /// </summary>
        /// <param name="cardToAdd">Specify card</param>
        /// <returns>Baseresponse with card</returns>
        public async Task<BaseResponse<CardDetails>> AddCardAsync(CardDetails cardToAdd)
        {
            if (cardToAdd == null) return new BaseResponse<CardDetails>("", StatusCodes.Status400BadRequest);
            CardDetails cardEntity = await GetContext().CardDetails.FirstOrDefaultAsync(i => i.Id == cardToAdd.Id).ConfigureAwait(false);
            if (cardEntity != null)
                return new BaseResponse<CardDetails>("", StatusCodes.Status409Conflict);

            GetContext().CardDetails.Add(cardToAdd);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<CardDetails>(cardToAdd);
        }

        /// <summary>
        /// This function update card in database
        /// </summary>
        /// <param name="cardToUpdate">Specif card</param>
        /// <returns>Baseresponse with card</returns>
        public async Task<BaseResponse<CardDetails>> UpdateCardAsync(CardDetails cardToUpdate)
        {
            if (cardToUpdate == null) return new BaseResponse<CardDetails>("", StatusCodes.Status400BadRequest);

            CardDetails cardEntity = await GetContext().CardDetails.AsNoTracking().SingleOrDefaultAsync(i => i.Id == cardToUpdate.Id).ConfigureAwait(false);
            if (cardEntity == null)
                return new BaseResponse<CardDetails>("", StatusCodes.Status404NotFound);

            GetContext().CardDetails.Update(cardToUpdate);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<CardDetails>(cardToUpdate);

        }
        /// <summary>
        /// This function delete card
        /// </summary>
        /// <param name="id">Specify Id of card</param>
        /// <returns>Baseresponse with card</returns>
        public async Task<BaseResponse<CardDetails>> DeleteCardIdAsync(int id)
        {
            CardDetails cardEntity = await GetContext().CardDetails.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (cardEntity == null)
                return new BaseResponse<CardDetails>("", StatusCodes.Status404NotFound);

            GetContext().CardDetails.Remove(cardEntity);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<CardDetails>(cardEntity);
        }
    }
}

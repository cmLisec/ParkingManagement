using Microsoft.EntityFrameworkCore;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.Models;

namespace ParkingManagement.Domain.Repositories.v1
{
    public class ParkingCardRepository : BaseRepository<ParkingCard>
    {
        public ParkingCardRepository(ParkingManagementDBContext context) : base(context)
        {
        }

        /// <summary>
        /// This function fetch all parking card available from database.
        /// </summary>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<List<ParkingCard>>> GetAvailableParkingCardAsync(DateTime startDate, DateTime endDate)
        {
            List<ParkingCard> parkingCard = await GetContext().ParkingCard.Where(x => x.StartDate >= startDate.Date && x.StartDate.Date <= endDate.Date).OrderBy(x => x.CardId).ToListAsync().ConfigureAwait(false);
            foreach (var data in parkingCard)
            {
                data.StartDate = data.StartDate.ToUniversalTime();
                data.EndDate = data.EndDate.ToUniversalTime();
            }
            return new BaseResponse<List<ParkingCard>>(parkingCard);
        }

        /// <summary>
        /// This function fetch booked parking card from database.
        /// </summary>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<List<ParkingCard>>> GetBookedParkingCardHistory()
        {
            List<ParkingCard> parkingCard = await GetContext().ParkingCard.Include(x => x.User).Where(x => x.StartDate >= DateTime.Now.Date).ToListAsync().ConfigureAwait(false);
            return new BaseResponse<List<ParkingCard>>(parkingCard);
        }

        /// <summary>
        /// This function fetch all parking card for user from database.
        /// </summary>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<List<ParkingCard>>> GetBookedParkingCardHistoryForUser(int userId)
        {
            List<ParkingCard> parkingCard = await GetContext().ParkingCard.Include(x => x.User).Where(x => x.UserId == userId).ToListAsync().ConfigureAwait(false);
            return new BaseResponse<List<ParkingCard>>(parkingCard);
        }

        /// <summary>
        /// This function fetch all parking card available from database.
        /// </summary>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<int>> GetAvailableCardDetailsAsync()
        {
            int parkingCard = await GetContext().CardDetails.CountAsync();
            return new BaseResponse<int>(parkingCard);
        }

        /// <summary>
        /// This function fetch parking card from database based on Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCard>> GetParkingCardByUserIdAsync(int id, int userId)
        {
            ParkingCard parkingCard = await GetContext().ParkingCard.SingleOrDefaultAsync(i => i.Id == id && i.UserId == userId).ConfigureAwait(false);
            if (parkingCard == null)
                return new BaseResponse<ParkingCard>("Not found", StatusCodes.Status404NotFound);
            return new BaseResponse<ParkingCard>(parkingCard);
        }

        /// <summary>
        /// This function add new parking card to database
        /// </summary>
        /// <param name="parkingCard">Specify parking card</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<List<ParkingCard>>> AddParkingCardAsync(List<ParkingCard> parkingCard)
        {
            parkingCard.ForEach(x => x.CreatedAt = DateTime.Now);
            GetContext().ParkingCard.AddRange(parkingCard);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<List<ParkingCard>>(parkingCard);
        }

        /// <summary>
        /// This function update parking card in database
        /// </summary>
        /// <param name="parkingCardUpdate">Specif parking card</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCard>> UpdateParkingCardAsync(ParkingCard parkingCardUpdate)
        {
            ParkingCard parkingCard = await GetContext().ParkingCard.AsNoTracking().SingleOrDefaultAsync(i => i.Id == parkingCardUpdate.Id && i.UserId == parkingCardUpdate.UserId).ConfigureAwait(false);
            if (parkingCard == null)
                return new BaseResponse<ParkingCard>("Parking card not found", StatusCodes.Status404NotFound);

            GetContext().ParkingCard.Update(parkingCardUpdate);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<ParkingCard>(parkingCardUpdate);

        }
        /// <summary>
        /// This function delete parking card
        /// </summary>
        /// <param name="id">Specify Id of parking card</param>
        /// <param name="userId">Specify Id of User</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCard>> DeleteParkingCardByIdAsync(int id, int userId)
        {
            ParkingCard parkingCard = await GetContext().ParkingCard.SingleOrDefaultAsync(i => i.Id == id && i.UserId == userId).ConfigureAwait(false);
            if (parkingCard == null)
                return new BaseResponse<ParkingCard>("Not found", StatusCodes.Status404NotFound);

            GetContext().ParkingCard.Remove(parkingCard);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<ParkingCard>(parkingCard);
        }
    }
}

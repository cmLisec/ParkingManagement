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
        /// <param name="query">Specify Query Parameter</param>
        /// <returns>Baseresponse with list of parking card</returns>
        public async Task<BaseResponse<List<ParkingCard>>> GetAllParkingCardAsync()
        {
            List<ParkingCard> UserList = await GetContext().ParkingCard.Where(x => x.StartDate >= DateTime.Now.Date).ToListAsync().ConfigureAwait(false);
            if (UserList.Count <= 0)
                return new BaseResponse<List<ParkingCard>>("No content available", StatusCodes.Status204NoContent);
            return new BaseResponse<List<ParkingCard>>(UserList);
        }

        /// <summary>
        /// This function fetch parking card from database based on Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCard>> GetParkingCardByUserIdAsync(int id)
        {
            ParkingCard UserEntity = await GetContext().ParkingCard.SingleOrDefaultAsync(i => i.UserId == id).ConfigureAwait(false);
            if (UserEntity == null)
                return new BaseResponse<ParkingCard>("Not found", StatusCodes.Status404NotFound);
            return new BaseResponse<ParkingCard>(UserEntity);
        }

        /// <summary>
        /// This function add new parking card to database
        /// </summary>
        /// <param name="parkingCard">Specify parking card</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCard>> AddParkingCardAsync(ParkingCard parkingCard)
        {
            if (parkingCard == null) return new BaseResponse<ParkingCard>("Bad request", StatusCodes.Status400BadRequest);
            GetContext().ParkingCard.Add(parkingCard);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<ParkingCard>(parkingCard);
        }

        /// <summary>
        /// This function update parking card in database
        /// </summary>
        /// <param name="parkingCardUpdate">Specif parking card</param>
        /// <returns>Baseresponse with parking card</returns>
        public async Task<BaseResponse<ParkingCard>> UpdateParkingCardAsync(ParkingCard parkingCardUpdate)
        {
            if (parkingCardUpdate == null) return new BaseResponse<ParkingCard>("Bad request", StatusCodes.Status400BadRequest);

            ParkingCard UserEntity = await GetContext().ParkingCard.AsNoTracking().SingleOrDefaultAsync(i => i.Id == parkingCardUpdate.Id && i.UserId == parkingCardUpdate.UserId).ConfigureAwait(false);
            if (UserEntity == null)
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
            ParkingCard UserEntity = await GetContext().ParkingCard.SingleOrDefaultAsync(i => i.Id == id && i.UserId == userId).ConfigureAwait(false);
            if (UserEntity == null)
                return new BaseResponse<ParkingCard>("Not found", StatusCodes.Status404NotFound);

            GetContext().ParkingCard.Remove(UserEntity);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<ParkingCard>(UserEntity);
        }
    }
}

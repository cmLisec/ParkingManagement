using Microsoft.EntityFrameworkCore;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.Models;

namespace ParkingManagement.Domain.Repositories.v1
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(ParkingManagementDBContext context) : base(context)
        {
        }

        /// <summary>
        /// This function fetch all Countries available from database.
        /// </summary>
        /// <param name="query">Specify Query Parameter</param>
        /// <returns>Baseresponse with list of User</returns>
        public async Task<BaseResponse<List<User>>> GetAllUserAsync()
        {
            List<User> UserList = await GetContext().User.ToListAsync().ConfigureAwait(false);
            if (UserList.Count <= 0)
                return new BaseResponse<List<User>>("", StatusCodes.Status204NoContent);
            return new BaseResponse<List<User>>(UserList);
        }

        /// <summary>
        /// This function fetch User from database based on Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> GetUserByIdAsync(int id)
        {
            User UserEntity = await GetContext().User.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (UserEntity == null)
                return new BaseResponse<User>("", StatusCodes.Status404NotFound);
            return new BaseResponse<User>(UserEntity);
        }

        /// <summary>
        /// This function add new User to database
        /// </summary>
        /// <param name="UserToAdded">Specify User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> AddUserAsync(User UserToAdded)
        {
            if (UserToAdded == null) return new BaseResponse<User>("", StatusCodes.Status400BadRequest);
            User UserEntity = await GetContext().User.FirstOrDefaultAsync(i => i.Id == UserToAdded.Id).ConfigureAwait(false);
            if (UserEntity != null)
                return new BaseResponse<User>("", StatusCodes.Status409Conflict);

            UserToAdded.CreatedDate = DateTime.Now;
            UserToAdded.UpdatedDate = DateTime.Now;
            GetContext().User.Add(UserToAdded);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<User>(UserToAdded);
        }

        /// <summary>
        /// This function update User in database
        /// </summary>
        /// <param name="UserToUpdate">Specif User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> UpdateUserAsync(User UserToUpdate)
        {
            if (UserToUpdate == null) return new BaseResponse<User>("", StatusCodes.Status400BadRequest);

            User UserEntity = await GetContext().User.AsNoTracking().SingleOrDefaultAsync(i => i.Id == UserToUpdate.Id).ConfigureAwait(false);
            if (UserEntity == null)
                return new BaseResponse<User>("", StatusCodes.Status404NotFound);

            GetContext().User.Update(UserToUpdate);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<User>(UserToUpdate);

        }
        /// <summary>
        /// This function delete User
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> DeleteUserByIdAsync(int id)
        {
            User UserEntity = await GetContext().User.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (UserEntity == null)
                return new BaseResponse<User>("", StatusCodes.Status404NotFound);

            GetContext().User.Remove(UserEntity);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<User>(UserEntity);
        }

        public User IsValidUser(string username, string password)
        {
            User UserEntity = GetContext().User.FirstOrDefault(i => i.Email == username && i.Password == password);
            if (UserEntity != null)
                return UserEntity;
            return null; 
        }
    }
}

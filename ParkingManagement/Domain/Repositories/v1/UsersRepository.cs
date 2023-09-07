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
        /// <returns>Baseresponse with list of User</returns>
        public async Task<BaseResponse<List<User>>> GetAllUserAsync()
        {
            List<User> userList = await GetContext().User.ToListAsync().ConfigureAwait(false);
            if (userList.Count <= 0)
                return new BaseResponse<List<User>>("", StatusCodes.Status204NoContent);
            return new BaseResponse<List<User>>(userList);
        }

        /// <summary>
        /// This function fetch User from database based on Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> GetUserByIdAsync(int id)
        {
            User userEntity = await GetContext().User.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (userEntity == null)
                return new BaseResponse<User>("", StatusCodes.Status404NotFound);
            return new BaseResponse<User>(userEntity);
        }

        /// <summary>
        /// This function add new User to database
        /// </summary>
        /// <param name="userToAdded">Specify User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> AddUserAsync(User userToAdded)
        {
            if (userToAdded == null) return new BaseResponse<User>("", StatusCodes.Status400BadRequest);
            User userEntity = await GetContext().User.FirstOrDefaultAsync(i => i.Id == userToAdded.Id).ConfigureAwait(false);
            if (userEntity != null)
                return new BaseResponse<User>("", StatusCodes.Status409Conflict);

            userToAdded.CreatedDate = DateTime.Now;
            userToAdded.UpdatedDate = DateTime.Now;
            GetContext().User.Add(userToAdded);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<User>(userToAdded);
        }

        /// <summary>
        /// This function update User in database
        /// </summary>
        /// <param name="userToUpdate">Specif User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> UpdateUserAsync(User userToUpdate)
        {
            if (userToUpdate == null) return new BaseResponse<User>("", StatusCodes.Status400BadRequest);

            User UserEntity = await GetContext().User.AsNoTracking().SingleOrDefaultAsync(i => i.Id == userToUpdate.Id).ConfigureAwait(false);
            if (UserEntity == null)
                return new BaseResponse<User>("", StatusCodes.Status404NotFound);

            GetContext().User.Update(userToUpdate);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<User>(userToUpdate);

        }
        /// <summary>
        /// This function delete User
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>Baseresponse with User</returns>
        public async Task<BaseResponse<User>> DeleteUserByIdAsync(int id)
        {
            User userEntity = await GetContext().User.SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (userEntity == null)
                return new BaseResponse<User>("", StatusCodes.Status404NotFound);

            GetContext().User.Remove(userEntity);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<User>(userEntity);
        }

        /// <summary>
        /// Checks for valid user
        /// </summary>
        /// <param name="username">Specify username</param>
        /// <param name="password">Speciy password</param>
        /// <returns>User</returns>
        public User IsValidUser(string username, string password)
        {
            User userEntity = GetContext().User.FirstOrDefault(i => i.Email == username && i.Password == password);
            if (userEntity != null)
                return userEntity;
            return null;
        }
    }
}

using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.Dtos;
using ParkingManagement.Domain.Models;
using ParkingManagement.Domain.Repositories.v1;

namespace ParkingManagement.Domain.Services.v1
{
    public class UsersService
    {
        private readonly UsersRepository _repo;
        private readonly IMapper _mapper;
        public UsersService(UsersRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        /// <summary>
        /// This fuction returns list of all User available in database
        /// </summary>
        /// <param name="query">Specify Query parameter</param>
        /// <returns>Baseresponse with list of UserDto</returns>
        public async Task<BaseResponse<List<UserDto>>> GetAllUserAsync()
        {
            BaseResponse<List<User>> response = await _repo.GetAllUserAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<UserDto> mappedResponse = _mapper.Map<List<User>, List<UserDto>>(response.Resource);
                return new BaseResponse<List<UserDto>>(mappedResponse);
            }
            return new BaseResponse<List<UserDto>>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function returns User based on Id
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>BaseResponse with UserDto</returns>
        public async Task<BaseResponse<UserDto>> GetUserByIdAsync(int id)
        {
            BaseResponse<User> response = await _repo.GetUserByIdAsync(id).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                UserDto mappedResponse = _mapper.Map<User, UserDto>(response.Resource);
                return new BaseResponse<UserDto>(mappedResponse);
            }
            return new BaseResponse<UserDto>(response.Message, response.StatusCode);
        }

        /// <summary>
        /// This function add new  User to database
        /// </summary>
        /// <param name="user">Specify User</param>
        /// <returns>Baseresponse with UserDto</returns>
        public async Task<BaseResponse<UserDto>> AddUserAsync(UserDto user)
        {
            if (user == null)
                return new BaseResponse<UserDto>("Bad Request", StatusCodes.Status400BadRequest);


            var userToAdded = _mapper.Map<UserDto, User>(user);
            BaseResponse<User> response = await _repo.AddUserAsync(userToAdded).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                UserDto mappedResponse = _mapper.Map<User, UserDto>(response.Resource);
                return new BaseResponse<UserDto>(mappedResponse);
            }
            return new BaseResponse<UserDto>(response.Message, response.StatusCode);

        }

        /// <summary>
        /// This function update existing User
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <param name="user">Specify User object</param>
        /// <returns>BaseResponse with UserDto</returns>
        public async Task<BaseResponse<UserDto>> UpdateUserAsync(int id, UserDto user)
        {
            if (user == null)
                return new BaseResponse<UserDto>("Bad Request", StatusCodes.Status400BadRequest);


            var userToUpdate = _mapper.Map<UserDto, User>(user);
            if (userToUpdate != null)
                userToUpdate.Id = id;
            BaseResponse<User> response = await _repo.UpdateUserAsync(userToUpdate).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                UserDto mappedResponse = _mapper.Map<User, UserDto>(response.Resource);
                return new BaseResponse<UserDto>(mappedResponse);
            }
            return new BaseResponse<UserDto>(response.Message, response.StatusCode);

        }

        /// <summary>
        /// This function deletes User from Database
        /// </summary>
        /// <param name="id">Specify Id of User</param>
        /// <returns>BaseResponse with UserDto</returns>
        public async Task<BaseResponse<UserDto>> DeleteUserByIdAsync(int id)
        {
            BaseResponse<User> response = await _repo.DeleteUserByIdAsync(id);
            if (response.IsSuccessStatusCode())
            {
                UserDto mappedResponse = _mapper.Map<User, UserDto>(response.Resource);
                return new BaseResponse<UserDto>(mappedResponse);
            }
            return new BaseResponse<UserDto>(response.Message, response.StatusCode);
        }
    }
}

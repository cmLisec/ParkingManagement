using AutoMapper;
using ParkingManagement.Domain.Dtos;
using ParkingManagement.Domain.Models;
using ParkingManagement.Domain.Repositories.v1;

namespace ParkingManagement.Domain.Services.v1
{
    public class LoginService
    {
        private readonly UsersRepository _repo;
        private readonly IMapper _mapper;
        public LoginService(UsersRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Checks for valid user
        /// </summary>
        /// <param name="username">Specify username</param>
        /// <param name="password">Speciy password</param>
        /// <returns>UserDto</returns>
        public UserDto IsValidUser(string username, string password)
        {
            User response = _repo.IsValidUser(username, password);
            if (response != null)
            {
                UserDto mappedResponse = _mapper.Map<User, UserDto>(response);
                return mappedResponse;
            }
            return null;
        }
    }
}

using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
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

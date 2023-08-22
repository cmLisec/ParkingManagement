using AutoMapper;
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

        public bool IsValidUser(string username, string password)
        {
            return _repo.IsValidUser(username,password);
        }
    }
}

using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Models;
using ParkingManagement.Domain.Repositories.v1;

namespace ParkingManagement.Domain.Services.v1
{
    public class PaymentService
    {
        private readonly PaymentRepository _repo;
        private readonly IMapper _mapper;
        public PaymentService(PaymentRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PaymentDTO>> AddPaymentAsync(PaymentDTO payment)
        {
            if (payment == null)
                return new BaseResponse<PaymentDTO>("", StatusCodes.Status400BadRequest);


            var UserToAdded = _mapper.Map<PaymentDTO, Payments>(payment);
            BaseResponse<Payments> response = await _repo.AddPaymentAsync(UserToAdded).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                PaymentDTO mappedResponse = _mapper.Map<Payments, PaymentDTO>(response.Resource);
                return new BaseResponse<PaymentDTO>(mappedResponse);
            }
            return new BaseResponse<PaymentDTO>(response.Message, response.StatusCode);

        }
    }
}

using AutoMapper;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Dtos;
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
        public async Task<BaseResponse<List<ExpenseHistoryDTO>>> GetAllExpenseAsync()
        {
            BaseResponse<List<ExpenseHistory>> response = await _repo.GetAllExpenseAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<ExpenseHistoryDTO> mappedResponse = _mapper.Map<List<ExpenseHistory>, List<ExpenseHistoryDTO>>(response.Resource);
                return new BaseResponse<List<ExpenseHistoryDTO>>(mappedResponse);
            }
            return new BaseResponse<List<ExpenseHistoryDTO>>(response.Message, response.StatusCode);
        }

        public async Task<BaseResponse<SettleUpHistoryDTO>> AddSettleUpAsync(SettleUpHistoryDTO settleUp)
        {
            if (settleUp == null)
                return new BaseResponse<SettleUpHistoryDTO>("", StatusCodes.Status400BadRequest);


            var settleUpToAdd = _mapper.Map<SettleUpHistoryDTO, SettleUpHistory>(settleUp);
            BaseResponse<SettleUpHistory> response = await _repo.AddSettleUpAsync(settleUpToAdd).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                SettleUpHistoryDTO mappedResponse = _mapper.Map<SettleUpHistory, SettleUpHistoryDTO>(response.Resource);
                return new BaseResponse<SettleUpHistoryDTO>(mappedResponse);
            }
            return new BaseResponse<SettleUpHistoryDTO>(response.Message, response.StatusCode);

        }

        public async Task<BaseResponse<List<SettleUpDTO>>> GetSettleUpDetailsAsync(int userId)
        {
            BaseResponse<List<SettleUp>> response = await _repo.GetSettleUpDetailsAsync(userId).ConfigureAwait(false);
            if (response.IsSuccessStatusCode())
            {
                List<SettleUpDTO> mappedResponse = _mapper.Map<List<SettleUp>, List<SettleUpDTO>>(response.Resource);
                return new BaseResponse<List<SettleUpDTO>>(mappedResponse);
            }
            return new BaseResponse<List<SettleUpDTO>>(response.Message, response.StatusCode);
        }

        public async Task<BaseResponse<List<TransactionDTO>>> GetTransactionDetailsAsync(int userId)
        {
          return await _repo.GetTransactionDetailsAsync(userId).ConfigureAwait(false);
        }
    }
}

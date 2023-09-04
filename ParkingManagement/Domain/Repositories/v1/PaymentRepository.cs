﻿using Microsoft.EntityFrameworkCore;
using ParkingManagement.Controllers.OutputObject;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Models;

namespace ParkingManagement.Domain.Repositories.v1
{
    public class PaymentRepository : BaseRepository<Payments>
    {
        public PaymentRepository(ParkingManagementDBContext context) : base(context)
        {
        }
        public async Task<BaseResponse<Payments>> AddPaymentAsync(Payments PaymentToAdded)
        {
            if (PaymentToAdded == null) return new BaseResponse<Payments>("", StatusCodes.Status400BadRequest);
            Payments PaymentEntity = await GetContext().Payments.FirstOrDefaultAsync(i => i.Id == PaymentToAdded.Id).ConfigureAwait(false);
            if (PaymentEntity != null)
                return new BaseResponse<Payments>("", StatusCodes.Status409Conflict);
            PaymentToAdded.CreatedDate = DateTime.UtcNow;
            PaymentToAdded.ModifiedDate = DateTime.UtcNow;
            var expenseHistoryEntity = new ExpenseHistory()
            {
                PayerUserId = PaymentToAdded.PayerUserId,
                Amount = PaymentToAdded.Amount,
                Date = PaymentToAdded.Date,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
            };
            //var userPayment = new UserPayment()
            //{
            //    PaymentId = PaymentToAdded.Id,
            //    UserId = PaymentToAdded.PayerUserId,
            //    CreatedDate = DateTime.UtcNow,
            //    ModifiedDate = DateTime.UtcNow        
            //};
            GetContext().Payments.Add(PaymentToAdded);
            GetContext().ExpenseHistories.Add(expenseHistoryEntity);
            //GetContext().UserPayments.Add(userPayment);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<Payments>(PaymentToAdded);
        }
        public async Task<BaseResponse<List<ExpenseHistory>>> GetAllExpenseAsync()
        {
            List<ExpenseHistory> ExpenseList = await GetContext().ExpenseHistories.ToListAsync().ConfigureAwait(false);
            if (ExpenseList.Count <= 0)
                return new BaseResponse<List<ExpenseHistory>>("", StatusCodes.Status204NoContent);
            return new BaseResponse<List<ExpenseHistory>>(ExpenseList);
        }
        public async Task<BaseResponse<SettleUpHistory>> AddSettleUpAsync(SettleUpHistory settleUp)
        {
            if (settleUp == null) return new BaseResponse<SettleUpHistory>("", StatusCodes.Status400BadRequest);
            SettleUpHistory settleUpEntity = await GetContext().SettleUpHistories.FirstOrDefaultAsync(i => i.Id == settleUp.Id).ConfigureAwait(false);
            if (settleUpEntity != null)
                return new BaseResponse<SettleUpHistory>("", StatusCodes.Status409Conflict);
            settleUp.CreatedDate = DateTime.UtcNow;
            settleUp.ModifiedDate = DateTime.UtcNow;

            GetContext().SettleUpHistories.Add(settleUp);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<SettleUpHistory>(settleUp);
        }

        public async Task<BaseResponse<List<SettleUp>>> GetSettleUpDetailsAsync(int userId)
        {
            var currentUser = GetContext().User.Include(u => u.Payments).FirstOrDefault(u => u.Id == userId);
            var users = GetContext().User.Include(u => u.Payments).ToList();
            var transactions = GetContext().SettleUpHistories.ToList();
            var settlements = new List<SettleUp>();
            // Calculate the total amount paid by the current user
            decimal totalPaidByCurrentUser = currentUser.Payments.Sum(p => p.Amount);

            // Calculate the total amount received by the current user from others
            decimal totalReceivedByCurrentUser = GetContext().SettleUpHistories
                .Where(t => t.ReceiverUserId == userId)
                .Sum(t => t.Amount);

            // Calculate the net amount that the current user owes or is owed
            decimal netBalance = totalReceivedByCurrentUser - totalPaidByCurrentUser;

            // Calculate the amount to be divided equally among other users
            decimal amountToDivide = totalPaidByCurrentUser / (users.Count - 1);

            foreach (var user in users)
            {
                if (user.Id != userId)
                {
                    decimal totalReceivedByUser = transactions.Where(t => t.ReceiverUserId == user.Id)
                                                              .Sum(t => t.Amount);
                    decimal netBalanceForUser = totalReceivedByUser - user.Payments.Sum(p => p.Amount);

                    // Adjust the balance based on the net balance
                    balance += netBalance / (users.Count - 1);

                    settlements.Add(new SettleUp { User = user, AmountToSettle = balance });
                }
            }
            return new BaseResponse<List<SettleUp>>(settlements);
        }
        public async Task<BaseResponse<List<TransactionDTO>>> GetTransactionDetailsAsync(int userId)
        {
            var currentUser = GetContext().User.Include(u => u.Payments).FirstOrDefault(u => u.Id == userId);
            var users = GetContext().User.ToList();
            var transactions = new List<TransactionDTO>();

            if (currentUser != null)
            {
                var paidByCurrentUser = currentUser.Payments.ToList();
                if (paidByCurrentUser != null)
                {
                    foreach (var payment in paidByCurrentUser)
                    {
                        transactions.Add(new TransactionDTO()
                        {
                            Amount = payment.Amount,
                            Date = payment.Date,
                            IsPayment = true,
                            PayedBy = null
                        });
                    }
                }

            }

            var receivedByCurrentUser = GetContext().PaymentTransaction
                .Where(t => t.PayeeId == userId).ToList();
            if (receivedByCurrentUser != null)
            {
                foreach (var payment in receivedByCurrentUser)
                {
                    var payedUser = users.FirstOrDefault(u => u.Id.Equals(payment.PayerId));
                    if (payedUser != null)
                    {
                        transactions.Add(new TransactionDTO()
                        {
                            Amount = payment.Amount,
                            Date = payment.CreatedDate,
                            IsPayment = false,
                            PayedBy = payedUser.Name
                        });
                    }
                }
            }
            if (!transactions.Any())
                return new BaseResponse<List<TransactionDTO>>("no transaction history", StatusCodes.Status204NoContent);
            return new BaseResponse<List<TransactionDTO>>(transactions);
        }
    }
}

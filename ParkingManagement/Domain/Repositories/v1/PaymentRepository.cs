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

        /// <summary>
        /// This function adds paymnet
        /// </summary>
        /// <param name="paymentToAdded">Specify payment</param>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<Payments>> AddPaymentAsync(Payments paymentToAdded)
        {
            if (paymentToAdded == null) return new BaseResponse<Payments>("", StatusCodes.Status400BadRequest);
            Payments paymentEntity = await GetContext().Payments.FirstOrDefaultAsync(i => i.Id == paymentToAdded.Id).ConfigureAwait(false);
            if (paymentEntity != null)
                return new BaseResponse<Payments>("", StatusCodes.Status409Conflict);
            paymentToAdded.CreatedDate = DateTime.UtcNow;
            paymentToAdded.ModifiedDate = DateTime.UtcNow;
            var expenseHistoryEntity = new ExpenseHistory()
            {
                PayerUserId = paymentToAdded.PayerUserId,
                Amount = paymentToAdded.Amount,
                Date = paymentToAdded.Date,
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
            GetContext().Payments.Add(paymentToAdded);
            GetContext().ExpenseHistories.Add(expenseHistoryEntity);
            //GetContext().UserPayments.Add(userPayment);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<Payments>(paymentToAdded);
        }

        /// <summary>
        /// This function get all expenses
        /// </summary>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<List<ExpenseHistory>>> GetAllExpenseAsync()
        {
            List<ExpenseHistory> expenseList = await GetContext().ExpenseHistories.ToListAsync().ConfigureAwait(false);
            if (expenseList.Count <= 0)
                return new BaseResponse<List<ExpenseHistory>>("", StatusCodes.Status204NoContent);
            return new BaseResponse<List<ExpenseHistory>>(expenseList);
        }

        /// <summary>
        /// This function adds settle up
        /// </summary>
        /// <param name="settleUp">Specify settleup</param>
        /// <returns>BaseResponse</returns>
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

        /// <summary>
        /// This function gets settle up details
        /// </summary>
        /// <param name="userId">Sepecify user id</param>
        /// <returns>BaseResponse</returns>
        public async Task<BaseResponse<List<SettleUp>>> GetSettleUpDetailsAsync(int userId)
        {
            var currentUser = GetContext().User.Include(u => u.Payments).FirstOrDefault(u => u.Id == userId);
            var users = GetContext().User.Include(u => u.Payments).ToList();
            var transactions = GetContext().SettleUpHistories.ToList();
            var settlements = new List<SettleUp>();
            // Calculate the total amount paid by the current user
            decimal totalPaidByCurrentUser = currentUser.Payments.Sum(p => p.Amount);
            // Calculate the amount to be divided equally among other users
            decimal amountToDivide = totalPaidByCurrentUser / (users.Count);

            foreach (var user in users)
            {
                if (user.Id != userId)
                {
                    decimal payedByUser = user.Payments.Sum(i => i.Amount);
                    decimal amountUserShouldGet = payedByUser / (users.Count);
                    decimal currentUserPaid = transactions.Where(t => t.PayerUserId == currentUser.Id && t.ReceiverUserId == user.Id)
                                                              .Sum(t => t.Amount);
                    decimal balanceCurrentUserShouldPay = amountToDivide - (amountUserShouldGet - currentUserPaid);



                    //decimal balance = 0;
                    //decimal totalReceivedByUser = transactions.Where(t => t.PayerUserId == user.Id && t.ReceiverUserId == currentUser.Id)
                    //                                          .Sum(t => t.Amount);

                    //decimal paidAmount = amountToDivide - currentUserPaid;
                    //if (paidAmount > 0)
                    //{
                    //    if (balanceCurrentUserShouldPay > 0)
                    //    {
                    //        balance = balanceCurrentUserShouldPay - (amountToDivide - currentUserPaid);
                    //    }
                    //    else
                    //    {
                    //        balance = amountToDivide - currentUserPaid;
                    //    }
                    //}
                    settlements.Add(new SettleUp { User = user, AmountToSettle = balanceCurrentUserShouldPay });
                }
            }
            return new BaseResponse<List<SettleUp>>(settlements);
        }

        /// <summary>
        /// This function gets transaction details
        /// </summary>
        /// <param name="userId">Sepecify user id</param>
        /// <returns>BaseResponse</returns>
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
                            PayedBy = null,
                            PayedTo = null
                        });
                    }
                }

            }

            var receivedByCurrentUser = GetContext().SettleUpHistories
                .Where(t => t.PayerUserId == userId).ToList();
            if (receivedByCurrentUser != null)
            {
                foreach (var payment in receivedByCurrentUser)
                {
                    var payedUser = users.FirstOrDefault(u => u.Id.Equals(payment.PayerUserId));
                    var recievedUser = users.FirstOrDefault(u => u.Id.Equals(payment.ReceiverUserId));
                    if (payedUser != null)
                    {
                        transactions.Add(new TransactionDTO()
                        {
                            Amount = payment.Amount,
                            Date = payment.CreatedDate,
                            IsPayment = false,
                            PayedBy = payedUser.Name,
                            PayedTo = recievedUser.Name
                        });
                    }
                }
            }
            if (!transactions.Any())
                return new BaseResponse<List<TransactionDTO>>("No transaction history", StatusCodes.Status204NoContent);
            return new BaseResponse<List<TransactionDTO>>(transactions);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ParkingManagement.Controllers.OutputObject;
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

            GetContext().Payments.Add(PaymentToAdded);
            await CompleteAsync().ConfigureAwait(false);
            return new BaseResponse<Payments>(PaymentToAdded);
        }
    }
}

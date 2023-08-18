using Microsoft.EntityFrameworkCore;

namespace ParkingManagement.Domain.Repositories.v1
{
    public abstract class BaseRepository<T> 
    {
        protected readonly DbContext _context;
        /// <summary>
        /// Constructor for BaseRepository class.
        /// </summary>
        /// <param name="context">Specifies the  db context.</param>
        protected BaseRepository(ParkingManagementDBContext context)  
        {
            _context = context;
        }

        /// <summary>
        /// This is the getter for context.
        /// </summary>
        public ParkingManagementDBContext GetContext()
        {
            return (ParkingManagementDBContext)_context;
        }

        /// <summary>
        /// This function save context data to the database and update the value of object from database generated value.
        /// </summary>
        /// <returns></returns>
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

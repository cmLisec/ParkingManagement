using AutoMapper;

namespace ParkingManagement
{
    /// <summary>
    /// Database mapping with Business objects /DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Create a mapper using this MappingProfile
        /// </summary>
        /// <returns></returns>
        public static Mapper CreateMapper()
        {
            MappingProfile mp = new MappingProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(mp));
            return new Mapper(config);
        }

        /// <summary>
        /// Constructor for mapping profile class.
        /// </summary>
        public MappingProfile()
        {
            
        }
    }
}

using EventAPI.Models.DTOs;
using Mapster;

namespace EventAPI.Models.Mappings
{
    public class UserMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Entity to UserDto mappings
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Email, src => src.Email);
        }
    }
}

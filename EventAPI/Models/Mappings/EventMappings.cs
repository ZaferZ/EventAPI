using EventAPI.Models.DTOs;
using Mapster;

namespace EventAPI.Models.Mappings
{
    public class EventMappings:IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //Entity to DTO mappings
            config.NewConfig<Event, EventDto>()
                .Map(dest => dest.Participants, src => src.Participants.Select(p => p.Id).ToList())
                .Map(dest => dest.OwnerId, src => src.OwnerId)
                .Map(dest => dest.HobbyId, src => src.HobbyId);

            //Create DTO to Entity mappings
            config.NewConfig<CreateEventDto, Event>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.ModifiedAt)
                .Ignore(dest => dest.CreatedBy)
                .Ignore(dest => dest.ModifiedBy)
                .Ignore(dest => dest.Status);

            //Update DTO to Entity mappings
            config.NewConfig<UpdateEventDto, Event>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.CreatedBy)
                .Ignore(dest => dest.Status);
        }
    }
}

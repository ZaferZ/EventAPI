using EventAPI.Helpers;
using EventAPI.Models.DTOs;
using Mapster;

namespace EventAPI.Models.Mappings
{
    public class EventMappings : IRegister
    {
        JwtContext _jwtContext;
        public void Register(TypeAdapterConfig config)
        {

            // Comment → CommentDto all property names match.
            TypeAdapterConfig<Comment, CommentDto>.NewConfig();


            // User → ParticipantDto
            // We don’t want to expose the entire User entity.
            // Only map the Id and the UserName (basic info about participant).
            TypeAdapterConfig<User, ParticipantDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)                 // User.Id → ParticipantDto.Id
                .Map(dest => dest.Username, src => src.Username);    // User.UserName → ParticipantDto.UserName


            // TaskEntity → TaskDto
            TypeAdapterConfig<TaskEntity, TaskDto>.NewConfig();

            // Event → EventDto
            TypeAdapterConfig<Event, EventDto>.NewConfig()
                // Convert EventStatus enum to string for JSON readability.
                .Map(dest => dest.Status, src => src.Status.ToString())

                // Map all related comments.
                .Map(dest => dest.Comments, src => src.Comments)

                // Map all participants, but thanks to the User → ParticipantDto
                // config above, only Id and UserName will be included.
                .Map(dest => dest.Participants, src => src.Participants)

                // Map related tasks into TaskDto.
                .Map(dest => dest.Tasks, src => src.Tasks)

                   // Map OwnerId and HobbyId directly.
                .Map(dest => dest.OwnerId, src => src.OwnerId)
                .Map(dest => dest.HobbyId, src => src.HobbyId); 

            //Entity to EventDTO mappings
            //config.NewConfig<Event, EventDto>()
            //    .Map(dest => dest.Participants, src => src.Participants.Select(p => p.Adapt<UserDto>()).ToList())
            //    .Map(dest => dest.OwnerId, src => src.OwnerId)
            //    .Map(dest => dest.HobbyId, src => src.HobbyId);

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
                .IgnoreNullValues(true);



            //Entity to EventDTO mappings
            config.NewConfig<Event, UpdateEventDto>()
                .Map(dest => dest.OwnerId, src => src.OwnerId)
                .Map(dest => dest.HobbyId, src => src.HobbyId);

        }
    }
}

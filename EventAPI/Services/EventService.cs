using EventAPI.Models;
using EventAPI.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EventAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<Event> Create(EventCreateDTO newEvent)
        {
            TypeAdapterConfig<EventCreateDTO, Event>.NewConfig()
                .Map(d => d.Id, s => 0)
                .Map(d => d.CreatedAt, s => DateTime.UtcNow)
                .Map(d => d.CreatedBy, s => new Guid()); // add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();


            return await _eventRepository.Create(eventEntity);
        }

        public async Task Delete(Event newEvent)
        {
            await _eventRepository.Delete(newEvent);
        }

        public async Task<IEnumerable<EventGetDTO>> GetAll()
        {
            var events = await _eventRepository.GetAll();
            var response = events.Adapt<List<EventGetDTO>>();
            return response;
        }

        public async Task<Event> GetById(int id)
        {
            var events = await _eventRepository.GetAll();
            var response = events.Adapt<List<EventGetDTO>>();
            return await _eventRepository.GetById(id);
        }

        public async Task<Event> Update(EventUpdateDTO newEvent)
        {
            TypeAdapterConfig<EventUpdateDTO, Event>.NewConfig()
               .Map(d => d.ModifiedAt, s => DateTime.UtcNow)
               .Map(d => d.ModifiedBy, s => new Guid());// add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();

            return await _eventRepository.Update(eventEntity);
        }


    }
}

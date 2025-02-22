using AutoMapper;
using SAW.DTO.Event;
using SAW.Models;

namespace SAW.Mappers
{
    public class UpdateEventMapper
    {
        private readonly IMapper _mapper;

        
        public UpdateEventMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Mapowanie
        public Event ToEntity(Event eventEntity, UpdateEventRequest request)
        {
            return
                _mapper.Map(request, eventEntity);
        }
    }
}
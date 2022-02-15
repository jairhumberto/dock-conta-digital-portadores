using AutoMapper;
using PortadoresService.Dtos;
using PortadoresService.Models;

namespace PortadoresService.Profiles
{
    public class PortadoresProfile : Profile
    {
        public PortadoresProfile()
        {
            CreateMap<Portador, PortadorReadDto>();
            CreateMap<PortadorCreateDto, Portador>();
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PortadoresService.Data;
using PortadoresService.Dtos;
using PortadoresService.Models;

namespace PortadoresService.Controllers
{
    [Route("api/ps/[controller]")]
    [ApiController]
    public class PortadoresController : ControllerBase
    {
        private readonly IPortadoresRepository _repository;
        private readonly IMapper _mapper;

        public PortadoresController(IPortadoresRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<PortadorReadDto> CreatePortador(PortadorCreateDto portadorCreateDto)
        {
            var portador = _mapper.Map<Portador>(portadorCreateDto);

            if (_repository.GetPortadorByCpf(portador.Cpf) != null)
            {
                return Conflict("Já existe um portador com o Cpf informado");
            }

            _repository.CreatePortador(portador);
            _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetPortadorByCpf), new { Cpf = portador.Cpf }, _mapper.Map<PortadorReadDto>(portador));
        }

        [HttpDelete("{cpf}")]
        public ActionResult DeletePortadorByCpf(string cpf)
        {
            var portador = _repository.GetPortadorByCpf(cpf);

            if (portador == null)
            {
                return NotFound();
            }

            _repository.DeletePortador(portador);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpGet("{cpf}", Name="GetPortadorByCpf")]
        public ActionResult<PortadorReadDto> GetPortadorByCpf(string cpf)
        {
            var portador = _repository.GetPortadorByCpf(cpf);

            if (portador == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PortadorReadDto>(portador));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PortadorReadDto>> GetPortadores()
        {
            var portadores = _repository.GetPortadores();
            return Ok(_mapper.Map<IEnumerable<PortadorReadDto>>(portadores));
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PortadoresService.Data;
using PortadoresService.Dtos;
using PortadoresService.Models;
using PortadoresService.SyncDataServices.Http;

namespace PortadoresService.Controllers
{
    [Route("api/ps/[controller]")]
    [ApiController]
    public class PortadoresController : ControllerBase
    {
        private readonly IPortadoresRepository _portadoresRepository;
        private readonly IMapper _mapper;
        private readonly IContasServiceClient _contasServiceClient;

        public PortadoresController(IPortadoresRepository portadoresRepository, IMapper mapper,
                IContasServiceClient contasServiceClient)
        {
            _portadoresRepository = portadoresRepository;
            _mapper = mapper;
            _contasServiceClient = contasServiceClient;
        }

        [HttpPost]
        public async Task<ActionResult<PortadorReadDto>> CreatePortador(PortadorCreateDto portadorCreateDto)
        {
            var portadorModel = _mapper.Map<Portador>(portadorCreateDto);

            if (_portadoresRepository.GetPortadorByCpf(portadorModel.Cpf) != null)
            {
                return Conflict("Cpf ja cadastrado");
            }

            var portadorReadDto = _mapper.Map<PortadorReadDto>(portadorModel);
            await _contasServiceClient.CreatePortador(portadorReadDto);

            _portadoresRepository.CreatePortador(portadorModel);
            _portadoresRepository.SaveChanges();

            return CreatedAtRoute(nameof(GetPortadorByCpf), new { Cpf = portadorModel.Cpf }, _mapper.Map<PortadorReadDto>(portadorModel));
        }

        [HttpDelete("{cpf}")]
        public async Task<ActionResult> DeletePortadorByCpf(string cpf)
        {
            var portadorModel = _portadoresRepository.GetPortadorByCpf(cpf);

            if (portadorModel == null)
            {
                return NotFound("Cpf nao cadastrado");
            }

            var portadorReadDto = _mapper.Map<PortadorReadDto>(portadorModel);
            await _contasServiceClient.DeletePortadorByCpf(portadorModel.Cpf);

            _portadoresRepository.DeletePortador(portadorModel);
            _portadoresRepository.SaveChanges();

            return NoContent();
        }

        [HttpGet("{cpf}", Name="GetPortadorByCpf")]
        public ActionResult<PortadorReadDto> GetPortadorByCpf(string cpf)
        {
            var portadorModel = _portadoresRepository.GetPortadorByCpf(cpf);

            if (portadorModel == null)
            {
                return NotFound("Cpf nao cadastrado");
            }

            return Ok(_mapper.Map<PortadorReadDto>(portadorModel));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PortadorReadDto>> GetPortadores()
        {
            var portadoresModels = _portadoresRepository.GetPortadores();
            return Ok(_mapper.Map<IEnumerable<PortadorReadDto>>(portadoresModels));
        }
    }
}
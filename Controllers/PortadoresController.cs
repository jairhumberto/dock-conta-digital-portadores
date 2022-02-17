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
        private readonly IPortadoresRepository _repository;
        private readonly IMapper _mapper;
        private readonly IContaDataClient _contaDataClient;

        public PortadoresController(IPortadoresRepository repository, IMapper mapper,
                IContaDataClient contaDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _contaDataClient = contaDataClient;
        }

        [HttpPost]
        public async Task<ActionResult<PortadorReadDto>> CreatePortador(PortadorCreateDto portadorCreateDto)
        {
            var portadorModel = _mapper.Map<Portador>(portadorCreateDto);

            if (_repository.GetPortadorByCpf(portadorModel.Cpf) != null)
            {
                return Conflict("Ja existe portador com o cpf informado");
            }

            _repository.CreatePortador(portadorModel);
            _repository.SaveChanges();

            var portadorReadDto = _mapper.Map<PortadorReadDto>(portadorModel);
            await _contaDataClient.SendPortadorToConta(portadorReadDto);

            return CreatedAtRoute(nameof(GetPortadorByCpf), new { Cpf = portadorModel.Cpf }, _mapper.Map<PortadorReadDto>(portadorModel));
        }

        [HttpDelete("{cpf}")]
        public async Task<ActionResult> DeletePortadorByCpf(string cpf)
        {
            var portadorModel = _repository.GetPortadorByCpf(cpf);

            if (portadorModel == null)
            {
                return NotFound();
            }

            var portadorReadDto = _mapper.Map<PortadorReadDto>(portadorModel);
            await _contaDataClient.DeletePortadorFromConta(portadorReadDto);

            _repository.DeletePortador(portadorModel);
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
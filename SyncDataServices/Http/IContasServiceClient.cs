using PortadoresService.Dtos;

namespace PortadoresService.SyncDataServices.Http
{
    public interface IContasServiceClient
    {
        Task CreatePortador(PortadorReadDto portadorReadDto); 
        Task DeletePortadorByCpf(string cpf); 
    }
}
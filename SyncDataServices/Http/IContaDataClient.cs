using PortadoresService.Dtos;

namespace PortadoresService.SyncDataServices.Http
{
    public interface IContaDataClient
    {
        Task SendPortadorToConta(PortadorReadDto portadorReadDto); 
        Task DeletePortadorFromConta(PortadorReadDto portadorReadDto); 
    }
}
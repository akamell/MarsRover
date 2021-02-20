using System.Threading.Tasks;
using DomainMarsRover = MarsRoverTracking.MarsRover.Domain.MarsRover;

public interface IMarsGetService
{
    Task<DomainMarsRover> Get(string roverId);
    Task<DomainMarsRover> GetFake();
}


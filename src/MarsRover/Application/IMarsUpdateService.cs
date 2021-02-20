using System.Threading.Tasks;
using MarsRoverTracking.MarsRover.Domain;
using DomainMarsRover = MarsRoverTracking.MarsRover.Domain.MarsRover;

public interface IMarsUpdateService
{
    Task<DomainMarsRover> UpdateMovement(MovementRequest newMovement);
}
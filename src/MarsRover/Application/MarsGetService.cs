using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

using DomainMarsRover = MarsRoverTracking.MarsRover.Domain.MarsRover;

namespace MarsRoverTracking.MarsRover.Application
{
    public class MarsGetService : IMarsGetService
    {
        public readonly IMemoryCache _cache;
        public MarsGetService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<DomainMarsRover> Get(string roverId)
        {
            return Task.Run(() =>
            {
                var obj = new Domain.MarsRover();
                if (_cache.TryGetValue<Domain.MarsRover>(roverId, out obj))
                {
                    obj.Message = $"Rover {roverId} was found";
                }
                return obj;
            });
        }

        public Task<DomainMarsRover> GetFake()
        {
            return Task.Run(() =>
            {
                return new DomainMarsRover()
                {
                    RoverId = "1",
                    LastIndexDirection = 0,
                    CurrentPositionX = 1,
                    CurrentPositionY = 2,
                    Message = "Message"
                };
            });
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

using DomainMarsRover = MarsRoverTracking.MarsRover.Domain.MarsRover;
using MarsRoverTracking.MarsRover.Domain;

namespace MarsRoverTracking.MarsRover.Application
{
    public class MarsUpdateService : IMarsUpdateService
    {
        public readonly IMemoryCache _cache;
        private readonly IMarsGetService _marsGetService;
        public MarsUpdateService(IMemoryCache cache, IMarsGetService marsGetService)
        {
            _cache = cache;
            _marsGetService = marsGetService;
        }

        public Task<DomainMarsRover> UpdateMovement(MovementRequest newMovement)
        {
            return Task.Run(() =>
            {
                var marsRover = this._marsGetService.Get(newMovement.RoverId).Result;
                if (string.IsNullOrEmpty(marsRover?.RoverId))
                {
                    marsRover = new DomainMarsRover()
                    {
                        RoverId = newMovement.RoverId,
                        CurrentPositionX = 0,
                        CurrentPositionY = 0,
                        Message = "New Mars Rover"
                    };
                }

                char[] instructions = newMovement.MovementInstruction.ToCharArray();
                char[] cardinalPoints = { 'N', 'E', 'S', 'O' };

                foreach (var instruction in instructions)
                {
                    if (instruction.Equals('L') || instruction.Equals('R'))
                    {
                        marsRover.LastIndexDirection = this.getNewDirection(instruction, marsRover.LastIndexDirection);
                    }
                    else
                    {
                        // move mars rover!
                        char currentCardinal = cardinalPoints[marsRover.LastIndexDirection];
                        if (currentCardinal == 'N')
                        {
                            // Y positivo
                            marsRover.CurrentPositionY = marsRover.CurrentPositionY + 1;
                        }
                        else if (currentCardinal == 'S')
                        {
                            // Y negativo
                            marsRover.CurrentPositionY = marsRover.CurrentPositionY + (-1);
                        }
                        else if (currentCardinal == 'E')
                        {
                            // X positivo
                            marsRover.CurrentPositionX = marsRover.CurrentPositionX + 1;
                        }
                        else if (currentCardinal == 'O')
                        {
                            // X negativo
                            marsRover.CurrentPositionX = marsRover.CurrentPositionX + (-1);
                        }
                    }
                }
                _cache.Set<Domain.MarsRover>(newMovement.RoverId, marsRover);
                return marsRover;
            });
        }

        private int getNewDirection(char instruction, int lastIndexPosition)
        {
            if (instruction == 'L')
            {
                if (lastIndexPosition == 0) lastIndexPosition = 3;
                else lastIndexPosition--;
            }
            else if (instruction == 'R')
            {
                if (lastIndexPosition == 3) lastIndexPosition = 0;
                else lastIndexPosition++;
            }
            return lastIndexPosition;
        }
    }
}
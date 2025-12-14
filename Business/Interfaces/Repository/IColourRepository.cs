using Domain.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces.Repository
{
    public interface IColourRepository
    {
        public Task<int> AddColour(Colour colour);
        public Task<IEnumerable<Colour>> GetAllColours();
    }
}

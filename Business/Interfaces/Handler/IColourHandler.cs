using Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces.Handler
{
    public interface IColourHandler
    {
        public Task<int> AddColour(ColourRequest colour);
        public Task<IEnumerable<Domain.EntityModel.Colour>> GetAllColours();
    }
}

using Business.Interfaces.Repository;
using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
   
    /// Provides data access operations for colour entities.
    
    public class ColourRepository : IColourRepository
    {
        private readonly ApplicationDbContext _context;

       
        /// Initializes a new instance of the <see cref="ColourRepository"/> class.
        
        /// <param name="context">The database context instance.</param>
        public ColourRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        /// Adds a new colour to the database.
        
        /// <param name="colour">The colour entity to add.</param>
        /// <returns>The identifier of the newly created colour.</returns>
        public async Task<int> AddColour(Colour colour)
        {
            _context.colours.Add(colour);
            await _context.SaveChangesAsync();
            return colour.ColourId;
        }

       
        /// Retrieves all colours from the database.
        
        /// <returns>A collection of all colour entities.</returns>
        public async Task<IEnumerable<Colour>> GetAllColours()
        {
            return await _context.colours.ToListAsync();
        }
    }
}
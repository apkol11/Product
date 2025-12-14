    using Business.Interfaces.Handler;
    using Business.Interfaces.Repository;
    using Domain.Request;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace Business.Handlers
    {
        /// <summary>
        /// Handles business logic operations related to colours.
        /// </summary>
        public class ColourHandler : IColourHandler
        {
            private readonly IColourRepository _colourRepository;

            /// <summary>
            /// Initializes a new instance of the <see cref="ColourHandler"/> class.
            /// </summary>
            /// <param name="repository">The colour repository instance.</param>
            public ColourHandler(IColourRepository repository)
            {
                _colourRepository = repository;
            }

            /// <summary>
            /// Adds a new colour to the system.
            /// </summary>
            /// <param name="colour">The colour creation request.</param>
            /// <returns>The identifier of the newly created colour.</returns>
            /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
            /// <exception cref="ArgumentException">Thrown when the colour name is missing or invalid.</exception>
            public async Task<int> AddColour(ColourRequest colour)
            {
                // Validate request object
                if (colour is null)
                    throw new ArgumentNullException(nameof(colour));

                // Validate colour name
                if (string.IsNullOrWhiteSpace(colour.ColourName))
                    throw new ArgumentException("Colour name is required.", nameof(colour.ColourName));

                // Map request to entity
                var entity = new Domain.EntityModel.Colour
                {
                    ColourName = colour.ColourName
                };

                // Persist colour and return generated identifier
                return await _colourRepository.AddColour(entity);
            }

            /// <summary>
            /// Retrieves all colours from the system.
            /// </summary>
            /// <returns>A collection of colour entities.</returns>
            public Task<IEnumerable<Domain.EntityModel.Colour>> GetAllColours()
            {
                return _colourRepository.GetAllColours();
            }
        }
    }

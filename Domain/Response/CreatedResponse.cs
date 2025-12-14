namespace Domain.Response
{
    /// <summary>
    /// Represents a standardized response for resource creation operations.
    /// </summary>
    public class CreatedResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the created resource.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a success message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the URI location of the created resource.
        /// </summary>
        public string Location { get; set; }
    }
}
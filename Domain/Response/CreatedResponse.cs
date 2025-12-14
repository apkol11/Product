namespace Domain.Response
{
   
    /// Represents a standardized response for resource creation operations.
    
    public class CreatedResponse
    {
       
        /// Gets or sets the unique identifier of the created resource.
        
        public int Id { get; set; }

       
        /// Gets or sets a success message.
        
        public string Message { get; set; }

       
        /// Gets or sets the URI location of the created resource.
        
        public string Location { get; set; }
    }
}
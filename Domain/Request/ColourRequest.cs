using System.ComponentModel.DataAnnotations;

namespace Domain.Request
{
    /// <summary>
    /// Request model for creating a new colour.
    /// </summary>
    public class ColourRequest
    {
        /// <summary>
        /// Gets or sets the name of the colour.
        /// </summary>
        [Required(ErrorMessage = "Colour name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Colour name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Colour name can only contain letters and spaces.")]
        public string ColourName { get; set; }
    }
}

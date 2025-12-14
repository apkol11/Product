using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request
{
    /// <summary>
    /// Request model for creating a new product type.
    /// </summary>
    public class ProductTypeRequest
    {
        /// <summary>
        /// Gets or sets the name of the product type.
        /// </summary>
        [Required(ErrorMessage = "Product type name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product type name must be between 2 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Product type name can only contain letters, numbers, and spaces.")]
        public string ProductTypeName { get; set; }
    }
}
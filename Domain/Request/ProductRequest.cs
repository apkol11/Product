using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Request
{
    /// <summary>
    /// Request model for creating a new product.
    /// </summary>
    public class ProductRequest
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 200 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Product name can only contain letters, numbers, and spaces.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product type identifier.
        /// </summary>
        [Required(ErrorMessage = "Product type is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product type.")]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Gets or sets the list of colour identifiers associated with the product.
        /// </summary>
        [Required(ErrorMessage = "At least one colour is required.")]
        [MinLength(1, ErrorMessage = "At least one colour must be selected.")]
        public List<int> ColourIds { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the username of the user creating the product.
        /// This property is optional and will be set automatically if not provided.
        /// </summary>
        [StringLength(100, ErrorMessage = "Created by name cannot exceed 100 characters.")]
        public string? CreatedBy { get; set; }
    }
}

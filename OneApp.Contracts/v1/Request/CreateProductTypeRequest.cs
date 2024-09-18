
using System.ComponentModel.DataAnnotations;

namespace OneApp.Contracts.v1.Request;

public class CreateProductTypeRequest
{
    [Required(ErrorMessage = "Name cannot be empty.")]
    public string Name { get; set; } = default!;
}

using System.ComponentModel.DataAnnotations;

namespace User.Web.Controllers.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "Name é obrigatorio")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name é obrigatorio")]
    public string Email { get; set; } = string.Empty;
}
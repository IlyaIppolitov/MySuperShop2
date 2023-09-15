using System.ComponentModel.DataAnnotations;

namespace MySuperShop.HttpModels.Requests;

public class UpdateAccountRequest
{
    [Required] 
    public Guid Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 20 символов")]
    public string Name { get; set; }

    [Required, EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "Пароль минимум 8 символов", MinimumLength = 8)]
    public string Password { get; set; }

    [Required]
    public string Roles { get; set; }

}
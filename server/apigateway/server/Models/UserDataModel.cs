using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models;

public class UserDataModel
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
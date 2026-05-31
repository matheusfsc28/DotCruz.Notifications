using System.ComponentModel.DataAnnotations;

namespace DotCruz.Notifications.CrossCutting.Settings;

public class EmailSettings
{
    [Required]
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string FromEmail { get; set; } = string.Empty;
    [Required]
    public string FromName { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
}

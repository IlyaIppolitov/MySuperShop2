using System.ComponentModel.DataAnnotations;

namespace MySuperShop.EmailSender.NotiSend;

public class NotiSendConfig
{
    [Required, RegularExpression(@"[^@\s]+\.[^@\s]+\.[^@\s]+$")] public string Host { get; set; }
    [Required] public string UserName { get; set; }
    [Required] public string Token { get; set; }
    [EmailAddress, Required] public string SendFrom { get; set; }
}
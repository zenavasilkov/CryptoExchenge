namespace CryptoExchange.Models
{
    public class AccountFormViewModel
    {
        public LoginViewModel Login { get; set; } = new();
        public RegisterViewModel Register { get; set; } = new();
        public string ActiveTab { get; set; } = "login";
    }
}

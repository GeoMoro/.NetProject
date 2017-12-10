namespace Presentation.Models.ManageViewModels
{
    public class TwoFactorAuthenticationViewModel
    {
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        // ReSharper disable once InconsistentNaming
        public bool Is2faEnabled { get; set; }
    }
}

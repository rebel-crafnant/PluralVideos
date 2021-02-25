using CommandLine;

namespace PluralVideos.Options
{
    [Verb("auth", HelpText = "Authenticates the app to pluralsight")]
    public class AuthenticatorOptions
    {
        [Option("login", SetName = "login", HelpText = "Login the app to Pluralsight")]
        public bool Login { get; set; }

        [Option("local-login", SetName = "local_login", HelpText = "Use Offline Pluralsight credentials")]
        public bool LocalLogin { get; set; }

        [Option('u', "username", SetName = "login", HelpText = "Pluralsight username")]
        public string Username { get; set; }

        [Option('p', "password", SetName = "login", HelpText = "Pluralsight password")]
        public string Password { get; set; } 

        [Option("logout", SetName = "logout", HelpText = "Logout the App from Pluralsight")]
        public bool Logout { get; set; }
    }
}

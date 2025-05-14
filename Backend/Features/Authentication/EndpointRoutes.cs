namespace Backend.Api;

public class EndpointRoutes
{
    public const string Root = "api";
    public const string Base = Root;

    public static class Auth
    {
        public const string AuthBase = Base + "/auth";
        public const string Login = AuthBase + "/login";
        public const string Register = AuthBase + "/register";
        public const string Refresh = AuthBase + "/refresh";
        public const string Logout = AuthBase + "/logout";
        public const string Session = AuthBase + "/session";

        public const string LoginGoogle = Login + "/google";
        public const string LoginGoogleCallback = Login + "/google/callback";

    }
}

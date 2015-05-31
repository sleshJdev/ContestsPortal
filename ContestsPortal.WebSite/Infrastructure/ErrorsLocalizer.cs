using System.Collections.Generic;

namespace ContestsPortal.WebSite.Infrastructure
{
    public static class ErrorsLocalizer
    {
        static ErrorsLocalizer()
        {
            MapToRussianError = new Dictionary<string, string>();
            MapToRussianError.Add(AspIdentityNativeErrorMessages.MustHaveAtLeastOneUpperCase,RussianErrorMessages.MustHaveAtLeastOneUpperCase);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.PasswordRequireDigit, RussianErrorMessages.PasswordRequireDigit);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.PasswordRequireNonLetterOrDigit, RussianErrorMessages.PasswordRequireNonLetterOrDigit);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.UserAlreadyHasPassword, RussianErrorMessages.UserAlreadyHasPassword);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.MissingInputSecret, RussianErrorMessages.MissingInputSecret);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.InvalidInputSecret, RussianErrorMessages.InvalidInputSecret);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.MissingInputResponse, RussianErrorMessages.MissingInputResponse);
            MapToRussianError.Add(AspIdentityNativeErrorMessages.InvalidInputResponse, RussianErrorMessages.InvalidInputResponse);
        }

        public static Dictionary<string, string> MapToRussianError { get; set; }
        
    }

    public static class AspIdentityNativeErrorMessages
    {
        #region PasswordRelated

        public const string MustHaveAtLeastOneUpperCase = "Passwords must have at least one uppercase ('A'-'Z').";
        public const string PasswordTooShort = "Passwords must be at least {0} characters.";
        public const string PasswordRequireDigit = "Passwords must have at least one digit ('0'-'9').";
        public const string PasswordRequireNonLetterOrDigit = "Passwords must have at least one non letter or digit character.";
        public const string UserAlreadyHasPassword = "User already has a password set.";

        #endregion


        #region RecaptchaRelated

        public const string MissingInputSecret = "missing-input-secret";
        public const string InvalidInputSecret = "invalid-input-secret";
        public const string MissingInputResponse = "missing-input-response";
        public const string InvalidInputResponse = "invalid-input-response";

        #endregion


    }

    public static class RussianErrorMessages
    {
        #region PasswordRelated

        public const string MustHaveAtLeastOneUpperCase = "Пароль должен содержать как минимум один символ в верхнем регистре.";
        public const string PasswordTooShort = "Пароль должен состоять минимум из {0} символов.";
        public const string PasswordRequireDigit = "Пароль должен содержать как минимум одну цифру ('0'-'9').";
        public const string PasswordRequireNonLetterOrDigit = "Пароль должен содержать как минимум один специальный символ (не цифра и не буква).";
        public const string UserAlreadyHasPassword = "Пользователь уже установил свой пароль.";
        
        #endregion


        #region Recaptcha
        
        public const string MissingInputSecret = "Отсутствует секретный ключ, необходимый для верификации Recapthca.";
        public const string InvalidInputSecret = "Секретный ключ некорректный, либо содержит ошибки.";
        public const string MissingInputResponse = "Необходимо пройти Recapthca-верификацию.";
        public const string InvalidInputResponse = "Поле g-recapthcha-response некорректно, либо содержит ошибки.";

        #endregion
    }


    public static class SocialLoginProviders
    {
        public static string Google = "Google";
        public static string MicrosoftAccount = "Microsoft Account";
        public static string Facebook = "Facebook";
        public static string Twitter = "Twitter";
        public static string VKontakte = "VKontakte";
    }
}
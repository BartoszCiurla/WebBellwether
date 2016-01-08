using WebBellwether.Services.Services.AuthService;
using WebBellwether.Services.Services.FileService;
using WebBellwether.Services.Services.IntegrationGameService;
using WebBellwether.Services.Services.JokeService;
using WebBellwether.Services.Services.LanguageService;
using WebBellwether.Services.Services.TranslateService;
using WebBellwether.Services.Services.VersionService;

namespace WebBellwether.API.Utility
{
    public static class ServiceFactory
    {
        private static ILanguageFileService _languageFileService;
        public static ILanguageFileService LanguageFileService
            => _languageFileService ?? (_languageFileService = new JsonLanguageFileService());

        private static IManagementIntegrationGamesService _managementIntegrationGamesService;
        public static IManagementIntegrationGamesService ManagementIntegrationGamesService
            =>
                _managementIntegrationGamesService ??
                (_managementIntegrationGamesService = new ManagementIntegrationGamesService());

        private static IManagementFeaturesService _managementFeaturesService;
        public static IManagementFeaturesService ManagementFeaturesService
            => _managementFeaturesService ?? (_managementFeaturesService = new ManagementFeaturesService());

        private static IIntegrationGameService _integrationGameService;
        public static IIntegrationGameService IntegrationGameService
            => _integrationGameService ?? (_integrationGameService = new IntegrationGameService());

        private static IManagementLanguageService _managementLanguageService;
        public static IManagementLanguageService ManagementLanguageService
            =>
                _managementLanguageService ??
                (_managementLanguageService = new ManagementLanguageService(LanguageFileService));

        private static ITranslateService _translateService;
        public static ITranslateService TranslateService
            => _translateService ?? (_translateService = new YandexTranslateService());

        private static IAuthService _authService;
        public static IAuthService AuthService => _authService ?? (_authService = new AuthService());

        private static IVersionService _versionService;
        public static IVersionService VersionService
            => _versionService ?? (_versionService = new VersionService(LanguageFileService));

        private static IJokeTranslationService _jokeTranslationService;
        private static IJokeTranslationService JokeTranslationService
            => _jokeTranslationService ?? (_jokeTranslationService = new JokeTranslationService());

        private static IJokeCategoryTranslationService _jokeCategoryTranslationService;
        private static IJokeCategoryTranslationService JokeCategoryTranslationService
            =>
                _jokeCategoryTranslationService ??
                (_jokeCategoryTranslationService = new JokeCategoryTranslationService());

        private static IJokeManagementService _jokeManagementService;
        public static IJokeManagementService JokeManagementService
            => _jokeManagementService ?? (_jokeManagementService = new JokeManagementService(JokeTranslationService));

        private static IJokeCategoryManagementService _jokeCategoryManagementService;
        public static IJokeCategoryManagementService JokeCategoryManagementService
            => _jokeCategoryManagementService ?? (_jokeCategoryManagementService = new JokeCategoryManagementService(JokeCategoryTranslationService));

        private static IJokeService _jokeService;
        public static IJokeService JokeService => _jokeService ?? (_jokeService = new JokeService(JokeTranslationService,JokeCategoryTranslationService));

    }
}
using WebBellwether.Services.Services;
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

        private static IManagementJokeService _managementJokeService;
        public static IManagementJokeService ManagementJokeService
            => _managementJokeService ?? (_managementJokeService = new ManagementJokeService());

        private static IManagementJokeCategoryService _managementJokeCategoryService;
        public static IManagementJokeCategoryService ManagementJokeCategoryService
            => _managementJokeCategoryService ?? (_managementJokeCategoryService = new ManagementJokeCategoryService());

        private static IJokeService _jokeService;
        public static IJokeService JokeService => _jokeService ?? (_jokeService = new JokeService());

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

    

    }
}
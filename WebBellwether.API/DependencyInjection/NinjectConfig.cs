using Ninject;
using WebBellwether.Services.Services.AuthService;
using WebBellwether.Services.Services.FileService;
using WebBellwether.Services.Services.IntegrationGameService;
using WebBellwether.Services.Services.JokeService;
using WebBellwether.Services.Services.LanguageService;
using WebBellwether.Services.Services.TranslateService;
using WebBellwether.Services.Services.VersionService;

namespace WebBellwether.API.DependencyInjection
{
    public class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IGameFeatureManagementService>().To<GameFeatureManagementService>();
            kernel.Bind<IIntegrationGameManagementService>().To<IntegrationGameManagementService>();
            kernel.Bind<IIntegrationGameService>().To<IntegrationGameService>();
            kernel.Bind<IJokeCategoryManagementService>().To<JokeCategoryManagementService>();
            kernel.Bind<IJokeManagementService>().To<JokeManagementService>();
            kernel.Bind<IJokeService>().To<JokeService>();
            kernel.Bind<ILanguageManagementService>().To<LanguageManagementService>();
            kernel.Bind<IAuthService>().To<AuthService>();
            kernel.Bind<ITranslateService>().To<YandexTranslateService>();
            kernel.Bind<IVersionService>().To<VersionService>();
            kernel.Bind<ILanguageFileService>().To<JsonLanguageFileService>();
            return kernel;
        }
    }
}

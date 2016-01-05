using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;

namespace WebBellwether.Repositories.Repositories.Abstract
{
    public interface IAggregateRepositories
    {
        void Save();
        IGenericRepository<IntegrationGameDao> IntegrationGameRepository { get; }
        IGenericRepository<IntegrationGameDetailDao> IntegrationGameDetailRepository { get; }
        IGenericRepository<IntegrationGameFeatureDao> IntegrationGameFeatureRepository { get; }
        IGenericRepository<GameFeatureDetailLanguageDao> GameFeatureDetailLanguageRepository { get; }
        IGenericRepository<GameFeatureLanguageDao> GameFeatureLanguageRepository { get; }
        IGenericRepository<GameFeatureDetailDao> GameFeatureDetailRepository { get; }
        IGenericRepository<GameFeatureDao> GameFeatureRepository { get; }
        IGenericRepository<LanguageDao> LanguageRepository { get; }
        IGenericRepository<JokeDao> JokeRepository { get; }
        IGenericRepository<JokeDetailDao> JokeDetailRepository { get; }
        IGenericRepository<JokeCategoryDao> JokeCategoryRepository { get; }
        IGenericRepository<JokeCategoryDetailDao> JokeCategoryDetailRepository { get; }
        IGenericRepository<JokeVersionDao> JokeVersionRepository { get; }
        IGenericRepository<JokeCategoryVersionDao> JokeCategoryVersionRepository { get; }
        IGenericRepository<LanguageVersionDao> LanguageVersionRepository { get; }
        IGenericRepository<IntegrationGameVersionDao> IntegrationGameVersionRepository { get; }
    }
}


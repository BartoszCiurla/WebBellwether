using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Entities.Version;

namespace WebBellwether.API.Repositories.Abstract
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


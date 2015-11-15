using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Repositories.Abstract
{
    public interface IAggregateRepositories
    {
        void Save();
        IGenericRepository<IntegrationGame> IntegrationGameRepository { get; }
        IGenericRepository<IntegrationGameDetail> IntegrationGameDetailRepository { get; }
        IGenericRepository<IntegrationGameFeature> IntegrationGameFeatureRepository { get; }
        IGenericRepository<GameFeatureDetailLanguage> GameFeatureDetailLanguageRepository { get; }
        IGenericRepository<GameFeatureLanguage> GameFeatureLanguageRepository { get; }
        IGenericRepository<GameFeatureDetail> GameFeatureDetailRepository { get; }
        IGenericRepository<GameFeature> GameFeatureRepository { get; }
        IGenericRepository<Language> LanguageRepository { get; }
        IGenericRepository<Joke> JokeRepository { get; }
        IGenericRepository<JokeDetail> JokeDetailRepository { get; }
        IGenericRepository<JokeCategory> JokeCategoryRepository { get; }
        IGenericRepository<JokeCategoryDetail> JokeCategoryDetailRepository { get; }
    }
}


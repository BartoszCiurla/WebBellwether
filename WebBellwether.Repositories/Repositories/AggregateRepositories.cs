using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using WebBellwether.Repositories.Context;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Entities.Version;
using IAggregateRepositories = WebBellwether.Repositories.Repositories.Abstract.IAggregateRepositories;


namespace WebBellwether.Repositories.Repositories
{
    public class AggregateRepositories: IAggregateRepositories,IDisposable
    {
        private readonly WebBellwetherDbContext _context;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameDao> _integrationGameRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameDetailDao> _integrationGameDetailRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameFeatureDao> _integrationGameFeatureRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureDetailLanguageDao> _gameFeatureDetailLanguageRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureLanguageDao> _gameFeatureLanguageRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureDetailDao> _gameFeatureDetailRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureDao> _gameFeatureRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<LanguageDao> _languageRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeDao> _jokeRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeDetailDao> _jokeDetailRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeCategoryDao> _jokeCategoryRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeCategoryDetailDao> _jokeCategoryDetailRepository;

        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<LanguageVersionDao> _languageVersionRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameVersionDao> _integrationGameVersionRepository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeCategoryVersionDao> _jokeCategoryVersionRespository;
        private WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeVersionDao> _jokeVersionRepository;

        public AggregateRepositories()
        {
            _context = new WebBellwetherDbContext();
        }

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeVersionDao> JokeVersionRepository
            => _jokeVersionRepository ?? (_jokeVersionRepository = new GenericRepository<JokeVersionDao>(_context)); 

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeCategoryVersionDao> JokeCategoryVersionRepository
            =>
                _jokeCategoryVersionRespository ??
                (_jokeCategoryVersionRespository = new GenericRepository<JokeCategoryVersionDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<LanguageVersionDao> LanguageVersionRepository
            =>
                _languageVersionRepository ??
                (_languageVersionRepository = new GenericRepository<LanguageVersionDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameVersionDao> IntegrationGameVersionRepository
            =>
                _integrationGameVersionRepository ??
                (_integrationGameVersionRepository = new GenericRepository<IntegrationGameVersionDao>(_context)); 

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeDao> JokeRepository => _jokeRepository ?? (_jokeRepository = new GenericRepository<JokeDao>(_context));
        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeDetailDao> JokeDetailRepository => _jokeDetailRepository ?? (_jokeDetailRepository = new GenericRepository<JokeDetailDao>(_context));
        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeCategoryDao> JokeCategoryRepository => _jokeCategoryRepository ?? (_jokeCategoryRepository = new GenericRepository<JokeCategoryDao>(_context));
        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<JokeCategoryDetailDao> JokeCategoryDetailRepository => _jokeCategoryDetailRepository ?? (_jokeCategoryDetailRepository = new GenericRepository<JokeCategoryDetailDao>(_context));
        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<LanguageDao> LanguageRepository => _languageRepository ?? (_languageRepository = new GenericRepository<LanguageDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameDao> IntegrationGameRepository
         => _integrationGameRepository ??
            (_integrationGameRepository = new GenericRepository<IntegrationGameDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameDetailDao> IntegrationGameDetailRepository
            => _integrationGameDetailRepository ??
               (_integrationGameDetailRepository = new GenericRepository<IntegrationGameDetailDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<IntegrationGameFeatureDao> IntegrationGameFeatureRepository
            => _integrationGameFeatureRepository ??
                (_integrationGameFeatureRepository = new GenericRepository<IntegrationGameFeatureDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureDetailLanguageDao> GameFeatureDetailLanguageRepository
            => _gameFeatureDetailLanguageRepository ??
               ((_gameFeatureDetailLanguageRepository = new GenericRepository<GameFeatureDetailLanguageDao>(_context)));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureLanguageDao> GameFeatureLanguageRepository
            => _gameFeatureLanguageRepository ??
               (_gameFeatureLanguageRepository = new GenericRepository<GameFeatureLanguageDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureDetailDao> GameFeatureDetailRepository
            => _gameFeatureDetailRepository ??
               (_gameFeatureDetailRepository = new GenericRepository<GameFeatureDetailDao>(_context));

        public WebBellwether.Repositories.Repositories.Abstract.IGenericRepository<GameFeatureDao> GameFeatureRepository
            => _gameFeatureRepository ??
               (_gameFeatureRepository = new GenericRepository<GameFeatureDao>(_context));

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(
                        $"{DateTime.Now}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    outputLines.AddRange(eve.ValidationErrors.Select(ve => $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\""));
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw;
            }
        }
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("AggregateRepositories is being disposed");
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
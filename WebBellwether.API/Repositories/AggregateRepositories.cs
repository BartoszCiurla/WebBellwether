using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Repositories.Abstract;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Entities.Version;


namespace WebBellwether.API.Repositories
{
    public class AggregateRepositories: IAggregateRepositories,IDisposable
    {
        private readonly EfDbContext _context;
        private IGenericRepository<IntegrationGameDao> _integrationGameRepository;
        private IGenericRepository<IntegrationGameDetailDao> _integrationGameDetailRepository;
        private IGenericRepository<IntegrationGameFeatureDao> _integrationGameFeatureRepository;
        private IGenericRepository<GameFeatureDetailLanguageDao> _gameFeatureDetailLanguageRepository;
        private IGenericRepository<GameFeatureLanguageDao> _gameFeatureLanguageRepository;
        private IGenericRepository<GameFeatureDetailDao> _gameFeatureDetailRepository;
        private IGenericRepository<GameFeatureDao> _gameFeatureRepository;
        private IGenericRepository<LanguageDao> _languageRepository;
        private IGenericRepository<JokeDao> _jokeRepository;
        private IGenericRepository<JokeDetailDao> _jokeDetailRepository;
        private IGenericRepository<JokeCategoryDao> _jokeCategoryRepository;
        private IGenericRepository<JokeCategoryDetailDao> _jokeCategoryDetailRepository;

        private IGenericRepository<LanguageVersionDao> _languageVersionRepository;
        private IGenericRepository<IntegrationGameVersionDao> _integrationGameVersionRepository;
        private IGenericRepository<JokeCategoryVersionDao> _jokeCategoryVersionRespository;
        private IGenericRepository<JokeVersionDao> _jokeVersionRepository;

        public AggregateRepositories()
        {
            _context = new EfDbContext();
        }

        public IGenericRepository<JokeVersionDao> JokeVersionRepository
            => _jokeVersionRepository ?? (_jokeVersionRepository = new GenericRepository<JokeVersionDao>(_context)); 

        public IGenericRepository<JokeCategoryVersionDao> JokeCategoryVersionRepository
            =>
                _jokeCategoryVersionRespository ??
                (_jokeCategoryVersionRespository = new GenericRepository<JokeCategoryVersionDao>(_context));

        public IGenericRepository<LanguageVersionDao> LanguageVersionRepository
            =>
                _languageVersionRepository ??
                (_languageVersionRepository = new GenericRepository<LanguageVersionDao>(_context));

        public IGenericRepository<IntegrationGameVersionDao> IntegrationGameVersionRepository
            =>
                _integrationGameVersionRepository ??
                (_integrationGameVersionRepository = new GenericRepository<IntegrationGameVersionDao>(_context)); 

        public IGenericRepository<JokeDao> JokeRepository => _jokeRepository ?? (_jokeRepository = new GenericRepository<JokeDao>(_context));
        public IGenericRepository<JokeDetailDao> JokeDetailRepository => _jokeDetailRepository ?? (_jokeDetailRepository = new GenericRepository<JokeDetailDao>(_context));
        public IGenericRepository<JokeCategoryDao> JokeCategoryRepository => _jokeCategoryRepository ?? (_jokeCategoryRepository = new GenericRepository<JokeCategoryDao>(_context));
        public IGenericRepository<JokeCategoryDetailDao> JokeCategoryDetailRepository => _jokeCategoryDetailRepository ?? (_jokeCategoryDetailRepository = new GenericRepository<JokeCategoryDetailDao>(_context));
        public IGenericRepository<LanguageDao> LanguageRepository => _languageRepository ?? (_languageRepository = new GenericRepository<LanguageDao>(_context));

        public IGenericRepository<IntegrationGameDao> IntegrationGameRepository
         => _integrationGameRepository ??
            (_integrationGameRepository = new GenericRepository<IntegrationGameDao>(_context));

        public IGenericRepository<IntegrationGameDetailDao> IntegrationGameDetailRepository
            => _integrationGameDetailRepository ??
               (_integrationGameDetailRepository = new GenericRepository<IntegrationGameDetailDao>(_context));

        public IGenericRepository<IntegrationGameFeatureDao> IntegrationGameFeatureRepository
            => _integrationGameFeatureRepository ??
                (_integrationGameFeatureRepository = new GenericRepository<IntegrationGameFeatureDao>(_context));

        public IGenericRepository<GameFeatureDetailLanguageDao> GameFeatureDetailLanguageRepository
            => _gameFeatureDetailLanguageRepository ??
               ((_gameFeatureDetailLanguageRepository = new GenericRepository<GameFeatureDetailLanguageDao>(_context)));

        public IGenericRepository<GameFeatureLanguageDao> GameFeatureLanguageRepository
            => _gameFeatureLanguageRepository ??
               (_gameFeatureLanguageRepository = new GenericRepository<GameFeatureLanguageDao>(_context));

        public IGenericRepository<GameFeatureDetailDao> GameFeatureDetailRepository
            => _gameFeatureDetailRepository ??
               (_gameFeatureDetailRepository = new GenericRepository<GameFeatureDetailDao>(_context));

        public IGenericRepository<GameFeatureDao> GameFeatureRepository
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
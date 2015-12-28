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


namespace WebBellwether.API.Repositories
{
    public class AggregateRepositories: IAggregateRepositories,IDisposable
    {
        private readonly EfDbContext _context;
        private IGenericRepository<IntegrationGame> _integrationGameRepository;
        private IGenericRepository<IntegrationGameDetail> _integrationGameDetailRepository;
        private IGenericRepository<IntegrationGameFeature> _integrationGameFeatureRepository;
        private IGenericRepository<GameFeatureDetailLanguage> _gameFeatureDetailLanguageRepository;
        private IGenericRepository<GameFeatureLanguage> _gameFeatureLanguageRepository;
        private IGenericRepository<GameFeatureDetail> _gameFeatureDetailRepository;
        private IGenericRepository<GameFeature> _gameFeatureRepository;
        private IGenericRepository<Language> _languageRepository;
        private IGenericRepository<Joke> _jokeRepository;
        private IGenericRepository<JokeDetail> _jokeDetailRepository;
        private IGenericRepository<JokeCategory> _jokeCategoryRepository;
        private IGenericRepository<JokeCategoryDetail> _jokeCategoryDetailRepository;

        public AggregateRepositories()
        {
            _context = new EfDbContext();
        }

        public IGenericRepository<Joke> JokeRepository => _jokeRepository ?? (_jokeRepository = new GenericRepository<Joke>(_context));
        public IGenericRepository<JokeDetail> JokeDetailRepository => _jokeDetailRepository ?? (_jokeDetailRepository = new GenericRepository<JokeDetail>(_context));
        public IGenericRepository<JokeCategory> JokeCategoryRepository => _jokeCategoryRepository ?? (_jokeCategoryRepository = new GenericRepository<JokeCategory>(_context));
        public IGenericRepository<JokeCategoryDetail> JokeCategoryDetailRepository => _jokeCategoryDetailRepository ?? (_jokeCategoryDetailRepository = new GenericRepository<JokeCategoryDetail>(_context));
        public IGenericRepository<Language> LanguageRepository => _languageRepository ?? (_languageRepository = new GenericRepository<Language>(_context));

        public IGenericRepository<IntegrationGame> IntegrationGameRepository
         => _integrationGameRepository ??
            (_integrationGameRepository = new GenericRepository<IntegrationGame>(_context));

        public IGenericRepository<IntegrationGameDetail> IntegrationGameDetailRepository
            => _integrationGameDetailRepository ??
               (_integrationGameDetailRepository = new GenericRepository<IntegrationGameDetail>(_context));

        public IGenericRepository<IntegrationGameFeature> IntegrationGameFeatureRepository
            => _integrationGameFeatureRepository ??
                (_integrationGameFeatureRepository = new GenericRepository<IntegrationGameFeature>(_context));

        public IGenericRepository<GameFeatureDetailLanguage> GameFeatureDetailLanguageRepository
            => _gameFeatureDetailLanguageRepository ??
               ((_gameFeatureDetailLanguageRepository = new GenericRepository<GameFeatureDetailLanguage>(_context)));

        public IGenericRepository<GameFeatureLanguage> GameFeatureLanguageRepository
            => _gameFeatureLanguageRepository ??
               (_gameFeatureLanguageRepository = new GenericRepository<GameFeatureLanguage>(_context));

        public IGenericRepository<GameFeatureDetail> GameFeatureDetailRepository
            => _gameFeatureDetailRepository ??
               (_gameFeatureDetailRepository = new GenericRepository<GameFeatureDetail>(_context));

        public IGenericRepository<GameFeature> GameFeatureRepository
            => _gameFeatureRepository ??
               (_gameFeatureRepository = new GenericRepository<GameFeature>(_context));

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
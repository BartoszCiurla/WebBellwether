using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Repositories;

namespace WebBellwether.API.UnitOfWork
{
    public class IntegrationGameUnitOfWork:IDisposable
    {
        private readonly EfDbContext _context;
        private GenericRepository<IntegrationGame> _integrationGameRepository;
        private GenericRepository<IntegrationGameDetail> _integrationGameDetailRepository;
        private GenericRepository<IntegrationGameFeature> _integrationGameFeatureRepository;
        private GenericRepository<GameFeatureDetailLanguage> _gameFeatureDetailLanguageRepository;
        private GenericRepository<GameFeatureLanguage> _gameFeatureLanguageRepository;
        private GenericRepository<GameFeatureDetail> _gameFeatureDetailRepository;
        private GenericRepository<GameFeature> _gameFeatureRepository;
        private GenericRepository<Language> _languageRepository;

        public IntegrationGameUnitOfWork()
        {
            _context = new EfDbContext();
        }

        public GenericRepository<IntegrationGame> IntegrationGameRepository
            => _integrationGameRepository ??
               (_integrationGameRepository = new GenericRepository<IntegrationGame>(_context));

        public GenericRepository<IntegrationGameDetail> IntegrationGameDetailRepository
            => _integrationGameDetailRepository ??
               (_integrationGameDetailRepository = new GenericRepository<IntegrationGameDetail>(_context));

        public GenericRepository<IntegrationGameFeature> IntegrationGameFeatureRepository
            => _integrationGameFeatureRepository ??
                (_integrationGameFeatureRepository = new GenericRepository<IntegrationGameFeature>(_context));

        public GenericRepository<GameFeatureDetailLanguage> GameFeatureDetailLanguageRepository
            => _gameFeatureDetailLanguageRepository ??
               ((_gameFeatureDetailLanguageRepository = new GenericRepository<GameFeatureDetailLanguage>(_context)));

        public GenericRepository<GameFeatureLanguage> GameFeatureLanguageRepository
            => _gameFeatureLanguageRepository ??
               (_gameFeatureLanguageRepository = new GenericRepository<GameFeatureLanguage>(_context));

        public GenericRepository<GameFeatureDetail> GameFeatureDetailRepository
            => _gameFeatureDetailRepository ??
               (_gameFeatureDetailRepository = new GenericRepository<GameFeatureDetail>(_context));

        public GenericRepository<GameFeature> GameFeatureRepository
            => _gameFeatureRepository ??
               (_gameFeatureRepository = new GenericRepository<GameFeature>(_context));

        public GenericRepository<Language> LanguageRepository
            => _languageRepository ??
               (_languageRepository = new GenericRepository<Language>(_context));

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
                    Debug.WriteLine("UnitOfWork is being disposed");
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

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using WebBellwether.API.Context;
using WebBellwether.API.Repositories;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.UnitOfWork
{
    public class JokeUnitOfWork : IDisposable
    {
        private readonly EfDbContext _context;
        private GenericRepository<Joke> _jokeRepository;
        private GenericRepository<JokeDetail> _jokeDetailRepository;
        private GenericRepository<JokeCategory> _jokeCategoryRepository;
        private GenericRepository<JokeCategoryDetail> _jokeCategoryDetailRepository;
        private GenericRepository<Language> _languageRepository;

        public JokeUnitOfWork()
        {
            _context = new EfDbContext();
        }

        public GenericRepository<Joke> JokeRepository => _jokeRepository ?? (_jokeRepository = new GenericRepository<Joke>(_context));
        public GenericRepository<JokeDetail> JokeDetailRepository => _jokeDetailRepository ?? (_jokeDetailRepository = new GenericRepository<JokeDetail>(_context));
        public GenericRepository<JokeCategory> JokeCategoryRepository => _jokeCategoryRepository ?? (_jokeCategoryRepository = new GenericRepository<JokeCategory>(_context));
        public GenericRepository<JokeCategoryDetail> JokeCategoryDetailRepository => _jokeCategoryDetailRepository ?? (_jokeCategoryDetailRepository = new GenericRepository<JokeCategoryDetail>(_context));
        public GenericRepository<Language> LanguageRepository=> _languageRepository ?? (_languageRepository = new GenericRepository<Language>(_context));

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

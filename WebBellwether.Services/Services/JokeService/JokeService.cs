using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Results;
using WebBellwether.Models.ViewModels.Joke;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IJokeService
    {
        JokeViewModel[] GetJokes(int language);
        JokeViewModel[] GetJokesWithAvailableLanguages(int language);
        JokeCategoryViewModel[] GetJokeCategoriesWithAvailableLanguage(int language);
    }
    public class JokeService : IJokeService
    {
        private readonly IJokeTranslationService _jokeTranslationService;
        private readonly IJokeCategoryTranslationService _jokeCategoryTranslationService;

        public JokeService(IJokeTranslationService jokeTranslationService,IJokeCategoryTranslationService jokeCategoryTranslationService)
        {
            _jokeTranslationService = jokeTranslationService;
            _jokeCategoryTranslationService = jokeCategoryTranslationService;
        }
        public JokeViewModel[] GetJokes(int language)
        {
            return
                RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == language).Select(x => new JokeViewModel
                {
                    Id = x.Joke.Id,
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id
                }).ToArray();
        }   

        public JokeViewModel[] GetJokesWithAvailableLanguages(int language)
        {
            var jokes = new List<JokeViewModel>();
            var jokesDao = RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == language).ToList();
            if (!jokesDao.Any())
                throw new Exception(ResultMessage.JokeCategoryNotExists.ToString());
            jokesDao.ForEach(x =>
            {
                jokes.Add(new JokeViewModel
                {
                    Id = x.Joke.Id,
                    JokeId = x.Id,
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryName = x.JokeCategoryDetail.JokeCategoryName,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id,
                    JokeCategoryDetailId = x.JokeCategoryDetail.Id,
                    JokeTranslations = _jokeTranslationService.FillAvailableTranslation(x.Joke.Id)
                });
            });
            return jokes.ToArray();
        }
              
        public JokeCategoryViewModel[] GetJokeCategoriesWithAvailableLanguage(int language)
        {
            var jokeCategoriesDao =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.Language.Id == language).ToList();
            if (!jokeCategoriesDao.Any())
                return null;
            var jokeCategories = new List<JokeCategoryViewModel>();
            jokeCategoriesDao.ForEach(
                x =>
                {
                    jokeCategories.Add(new JokeCategoryViewModel
                    {
                        Id = x.JokeCategory.Id,
                        JokeCategoryId = x.Id,
                        JokeCategoryName = x.JokeCategoryName,
                        LanguageId = x.Language.Id,
                        JokeCategoryTranslations = _jokeCategoryTranslationService.FillAvailableJokeCategoryTranslation(x.JokeCategory.Id)
                    });
                });
            return jokeCategories.ToArray();
        }     
    }
}

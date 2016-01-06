using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IManagementJokeService
    {
        ResultStateContainer InsertJoke(JokeModel joke);
        ResultStateContainer DeleteJoke(JokeModel joke);
        JokeModel GetJokeTranslation(int jokeId, int languageId);
        ResultStateContainer PutJoke(JokeModel joke);
        ResultStateContainer InsertSingleLanguageJoke(JokeModel joke);
        JokeCategoryDetailDao GetJokeCategory(int jokeCategory, int languageId);
        ResultStateContainer InsertSeveralLanguageJoke(JokeModel joke);
        LanguageDao GetLanguage(int id);
    }
    public class ManagementJokeService: IManagementJokeService
    {
        public ResultStateContainer InsertJoke(JokeModel joke)
        {
            if (RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                return new ResultStateContainer { ResultState = ResultState.Failure , ResultMessage = ResultMessage.JokeExists };
            if (joke.Id == 0)//new joke
                return InsertSingleLanguageJoke(joke);
            return InsertSeveralLanguageJoke(joke);//joke translation
        }
        public ResultStateContainer DeleteJoke(JokeModel joke)
        {
            try
            {
                //all translation
                if (joke.TemporarySeveralTranslationDelete)
                {
                    var jokeDetails = RepositoryFactory.Context.JokeDetails.Where(x => x.Joke.Id == joke.Id);
                    if (!jokeDetails.Any())
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeDetailNotExists };
                    RepositoryFactory.Context.JokeDetails.RemoveRange(jokeDetails);
                    var mainJoke = RepositoryFactory.Context.Jokes.FirstOrDefault(x => x.Id == joke.Id);
                    if (mainJoke == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage= ResultMessage.JokeNotExists };
                    RepositoryFactory.Context.Jokes.Remove(mainJoke);
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeDeleted };
                }
                else
                {
                    var jokeDetail = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Id == joke.JokeId);
                    if (jokeDetail != null)
                        RepositoryFactory.Context.JokeDetails.Remove(jokeDetail);
                    int jokeTranslationCount = 0;
                    if (!joke.JokeTranslations.Any())
                        joke.JokeTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                jokeTranslationCount++;
                        });
                    if (jokeTranslationCount == 1)//have only one translation , can delete main id for joke . Safe is safe ...
                    {
                        var mainJoke = RepositoryFactory.Context.Jokes.FirstOrDefault(x => x.Id == joke.Id);
                        if (mainJoke != null)
                            RepositoryFactory.Context.Jokes.Remove(mainJoke);
                    }
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage= ResultMessage.JokeDeleted };
                }
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.Error };
            }
        }

        public JokeModel GetJokeTranslation(int jokeId, int languageId)
        {
            var entity =
                RepositoryFactory.Context.JokeDetails.FirstOrDefault(
                    x => x.Joke.Id == jokeId && x.Language.Id == languageId);
            if (entity == null)
                return null;
            JokeModel joke = new JokeModel { Id = jokeId, JokeContent = entity.JokeContent, LanguageId = entity.Language.Id, JokeId = entity.Id };
            return joke;
        }

        public ResultStateContainer PutJoke(JokeModel joke)
        {
            try
            {
                var entity = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Id == joke.JokeId);
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeNotExists };

                if (entity.JokeCategoryDetail.JokeCategory.Id != joke.JokeCategoryId) // in edit i change category 
                {
                    JokeCategoryDetailDao categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                    if (categoryDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.JokeCategoryNotExists, ResultValue = GetLanguage(joke.LanguageId) };
                    entity.JokeCategoryDetail = categoryDetail;
                    foreach (AvailableLanguage al in joke.JokeTranslations)
                    {
                        if (al.HasTranslation && al.Language.Id != joke.LanguageId)
                        {
                            JokeCategoryDetailDao categoryDetailForOtherTranslation = GetJokeCategory(joke.JokeCategoryId, al.Language.Id);
                            if (categoryDetailForOtherTranslation == null)
                                return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryNotExists, ResultValue = al.Language.LanguageName };
                            var otherTranslationJoke =
                                RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Joke.Id == joke.Id);
                            if (otherTranslationJoke != null)
                                otherTranslationJoke.JokeCategoryDetail = categoryDetailForOtherTranslation;
                        }
                    }
                }
                else if(!entity.JokeContent.Equals(joke.JokeContent))
                    if (RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage= ResultMessage.JokeExists };
                entity.JokeContent = joke.JokeContent;
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeEdited };
            }
            catch (Exception)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }

        public ResultStateContainer InsertSingleLanguageJoke(JokeModel joke)
        {
            try
            {
                //first i check joke category 
                JokeCategoryDetailDao entityCategory = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                if (entityCategory == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryNotExists };
                JokeDetailDao entityDetail = new JokeDetailDao
                {
                    JokeContent = joke.JokeContent,
                    Language = GetLanguage(joke.LanguageId),
                    JokeCategoryDetail = entityCategory
                };
                if (entityDetail.Language == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage= ResultMessage.LanguageNotExists };
                JokeDao entity = new JokeDao
                {
                    CreationDate = DateTime.UtcNow,
                    JokeDetails = new List<JokeDetailDao>()
                };
                entity.JokeDetails.Add(entityDetail);
                RepositoryFactory.Context.Jokes.Add(entity);
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeAdded };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public JokeCategoryDetailDao GetJokeCategory(int jokeCategory, int languageId)
        {
            return
                RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                    x => x.JokeCategory.Id == jokeCategory && x.Language.Id == languageId);
        }
        public ResultStateContainer InsertSeveralLanguageJoke(JokeModel joke)
        {
            try
            {
                if (joke.JokeId != 0)
                {
                    var entityToEdit = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Id == joke.JokeId);
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeTranslationNotExists };
                    entityToEdit.JokeContent = joke.JokeContent;
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage=ResultMessage.JokeTranslationEdited };
                }
                if (RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.JokeExists };
                LanguageDao lang = GetLanguage(joke.LanguageId);
                if (lang == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.LanguageNotExists };
                JokeCategoryDetailDao categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                if (categoryDetail == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryNotExists,ResultValue =GetLanguage(joke.LanguageId).LanguageName};
                var entity = RepositoryFactory.Context.Jokes.FirstOrDefault(x => x.Id == joke.Id);
                entity?.JokeDetails.Add(new JokeDetailDao
                {
                    JokeContent = joke.JokeContent,
                    Language = lang,
                    JokeCategoryDetail = categoryDetail
                });
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage=ResultMessage.JokeAdded };

            }
            catch (Exception)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        //This function is often duplicated it a little disturbing
        public LanguageDao GetLanguage(int id)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == id);
        }
    }
}
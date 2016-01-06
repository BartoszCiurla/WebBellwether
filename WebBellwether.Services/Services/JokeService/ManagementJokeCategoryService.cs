using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IManagementJokeCategoryService
    {
        List<JokeCategoryModel> GetJokeCategories(int languageId);
        JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId);
        ResultStateContainer InsertJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer InsertSeveralLanguageJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer InsertSingleLanguageJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory);
        LanguageDao GetLanguage(int id);
    }
    public class ManagementJokeCategoryService:IManagementJokeCategoryService
    {
        public List<JokeCategoryModel> GetJokeCategories(int languageId)
        {
            List<JokeCategoryModel> result =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.Language.Id == languageId)
                    .Select(z=> new JokeCategoryModel
                    {
                        Id = z.JokeCategory.Id,
                        JokeCategoryId = z.Id,
                        JokeCategoryName = z.JokeCategoryName,
                        LanguageId = z.Language.Id
                    }).ToList();
            return result;
        }
        public JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId)
        {
            var entity =
                RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                    x => x.JokeCategory.Id == jokeCategoryId && x.Language.Id == languageId);
            if (entity == null)
                return null;
            return new JokeCategoryModel { Id = jokeCategoryId, JokeCategoryName = entity.JokeCategoryName, LanguageId = entity.Language.Id, JokeCategoryId = entity.Id };
        }
        public ResultStateContainer InsertJokeCategory(JokeCategoryModel jokeCategory)
        {
            if (RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                return new ResultStateContainer { ResultState = ResultState.Success };
            if (jokeCategory.Id == 0)
                return InsertSingleLanguageJokeCategory(jokeCategory);
            return InsertSeveralLanguageJokeCategory(jokeCategory);
        }
        public ResultStateContainer InsertSeveralLanguageJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                if(jokeCategory.JokeCategoryId != 0)
                {
                    var entityToEdit =
                        RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                            x => x.Id == jokeCategory.JokeCategoryId);
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryTranslationNotExists };
                    entityToEdit.JokeCategoryName = jokeCategory.JokeCategoryName;
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeCategoryTranslationEdited };
                }
                if (RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryExistsInDb };
                var entity = RepositoryFactory.Context.JokeCategories.FirstOrDefault(x => x.Id == jokeCategory.Id);
                entity?.JokeCategoryDetail.Add(new JokeCategoryDetailDao { JokeCategoryName = jokeCategory.JokeCategoryName, Language =RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == jokeCategory.LanguageId)});
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage=ResultMessage.JokeCategoryTranslationAdded };
            }
            catch(Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultStateContainer InsertSingleLanguageJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                JokeCategoryDao entity = new JokeCategoryDao
                {
                    CreationDate = DateTime.UtcNow,
                    JokeCategoryDetail = new List<JokeCategoryDetailDao>
                    {
                        new JokeCategoryDetailDao
                        {
                            JokeCategoryName = jokeCategory.JokeCategoryName,
                            Language = GetLanguage(jokeCategory.LanguageId)
                        }
                    }
                };
                RepositoryFactory.Context.JokeCategories.Add(entity);
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage= ResultMessage.JokeCategoryAdded, ResultValue = jokeCategory.JokeCategoryName };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                var entity =
                    RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                        x => x.Id == jokeCategory.JokeCategoryId);
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure , ResultMessage = ResultMessage.JokeCategoryNotExists };
                if (RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName))!= null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.ThisJokeCategoryExists };
                entity.JokeCategoryName = jokeCategory.JokeCategoryName;
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeCategoryEdited };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                //all translation
                if (jokeCategory.TemporarySeveralTranslationDelete)
                {
                    var jokeCategoryDetail =
                        RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.JokeCategory.Id == jokeCategory.Id);
                    RepositoryFactory.Context.JokeCategoryDetails.RemoveRange(jokeCategoryDetail);
                    var mainJokeCategory =
                        RepositoryFactory.Context.JokeCategories.FirstOrDefault(x => x.Id == jokeCategory.Id);
                    if (mainJokeCategory != null)
                        RepositoryFactory.Context.JokeCategories.Remove(mainJokeCategory);
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeCategoryDeleted };
                }

                else
                {
                    var jokeCategoryDetail =
                        RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                            x => x.Id == jokeCategory.JokeCategoryId);
                    if (jokeCategoryDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure , ResultMessage = ResultMessage.JokeCategoryTranslationNotExists };
                    RepositoryFactory.Context.JokeCategoryDetails.Remove(jokeCategoryDetail);
                    int jokeCategoryTranslationCount = 0;
                    if (jokeCategory.JokeCategoryTranslations != null)
                        jokeCategory.JokeCategoryTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                jokeCategoryTranslationCount++;
                        });
                    if (jokeCategoryTranslationCount == 1)//have only one translation , can delete main id for jokecategory . Safe is safe ...
                    {
                        var mainJokeCategory =
                            RepositoryFactory.Context.JokeCategories.FirstOrDefault(x => x.Id == jokeCategory.Id);
                        if (mainJokeCategory != null)
                            RepositoryFactory.Context.JokeCategories.Remove(mainJokeCategory);
                    }
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage= ResultMessage.JokeCategoryDeleted };

                }
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure , ResultMessage = ResultMessage.Error };
            }
        }
        //This function is often duplicated it a little disturbing . Do something with this 
        public LanguageDao GetLanguage(int id)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == id);
        }

    }
}

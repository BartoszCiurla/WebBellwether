using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.Results;
using WebBellwether.API.Services.JokeService.Abstract;
using System;
using WebBellwether.API.Repositories.Abstract;

namespace WebBellwether.API.Services.JokeService
{
    public class ManagementJokeCategoryService:IManagementJokeCategoryService
    {
        private readonly IAggregateRepositories _repository;
        public ManagementJokeCategoryService(IAggregateRepositories repository)
        {
            _repository = repository;
        }

        public List<JokeCategoryModel> GetJokeCategories(int languageId)
        {
            List<JokeCategoryModel> result = new List<JokeCategoryModel>();
            _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.Language.Id == languageId).ToList().ForEach(z =>
             {
                 result.Add(new JokeCategoryModel { Id=z.JokeCategory.Id,JokeCategoryId =z.Id,JokeCategoryName =z.JokeCategoryName , LanguageId = z.Language.Id });
             });
            return result;
        }
        public JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId)
        {
            var entity = _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategoryId && x.Language.Id == languageId).FirstOrDefault();
            if (entity == null)
                return null;
            JokeCategoryModel jokeCategory = new JokeCategoryModel { Id = jokeCategoryId, JokeCategoryName = entity.JokeCategoryName, LanguageId = entity.Language.Id, JokeCategoryId = entity.Id };
            return jokeCategory;
        }
        public ResultStateContainer InsertJokeCategory(JokeCategoryModel jokeCategory)
        {
            if (_repository.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryExistsInDb };
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
                    var entityToEdit = _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.Id == jokeCategory.JokeCategoryId).FirstOrDefault();
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationNotExists };
                    entityToEdit.JokeCategoryName = jokeCategory.JokeCategoryName;
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationEdited };
                }
                if (_repository.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryExistsInDb };
                var entity = _repository.JokeCategoryRepository.GetWithInclude(x => x.Id == jokeCategory.Id).FirstOrDefault();
                entity?.JokeCategoryDetail.Add(new JokeCategoryDetail { JokeCategoryName = jokeCategory.JokeCategoryName, Language = _repository.LanguageRepository.GetFirst(x => x.Id == jokeCategory.LanguageId) });
                _repository.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationAdded };
            }
            catch(Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer InsertSingleLanguageJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                JokeCategory entity = new JokeCategory
                {
                    CreationDate = DateTime.UtcNow,
                    JokeCategoryDetail = new List<JokeCategoryDetail>
                    {
                        new JokeCategoryDetail
                        {
                            JokeCategoryName = jokeCategory.JokeCategoryName,
                            Language = GetLanguage(jokeCategory.LanguageId)
                        }
                    }
                };
                _repository.JokeCategoryRepository.Insert(entity);
                _repository.Save();

                return new ResultStateContainer { ResultState = ResultState.JokeCategoryAdded, Value = jokeCategory.JokeCategoryName };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                var entity = _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.Id == jokeCategory.JokeCategoryId).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryNotExistsInDb };
                if (_repository.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                    return new ResultStateContainer { ResultState = ResultState.ThisJokeCategoryExists };
                entity.JokeCategoryName = jokeCategory.JokeCategoryName;
                _repository.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryEdited };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                //all translation
                if (jokeCategory.TemporarySeveralTranslationDelete)
                {
                    var jokeCategoryDetail = _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategory.Id);
                    if (jokeCategoryDetail != null)
                        jokeCategoryDetail.ToList().ForEach(x =>
                        {
                            _repository.JokeCategoryDetailRepository.Delete(x);
                        });
                    var mainJokeCategory = _repository.JokeCategoryRepository.GetWithInclude(x => x.Id == jokeCategory.Id).FirstOrDefault();
                    if (mainJokeCategory != null)
                        _repository.JokeCategoryRepository.Delete(mainJokeCategory);
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryDeleted };

                }

                else
                {
                    var jokeCategoryDetail = _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.Id == jokeCategory.JokeCategoryId).FirstOrDefault();
                    if (jokeCategoryDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationNotExists };
                    _repository.JokeCategoryDetailRepository.Delete(jokeCategoryDetail);
                    int jokeCategoryTranslationCount = 0;
                    if (jokeCategory.JokeCategoryTranslations != null)
                        jokeCategory.JokeCategoryTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                jokeCategoryTranslationCount++;
                        });
                    if (jokeCategoryTranslationCount == 1)//have only one translation , can delete main id for jokecategory . Safe is safe ...
                    {
                        var mainJokeCategory = _repository.JokeCategoryRepository.GetWithInclude(x => x.Id == jokeCategory.Id).FirstOrDefault();
                        if (mainJokeCategory != null)
                            _repository.JokeCategoryRepository.Delete(mainJokeCategory);
                    }
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryDeleted };

                }
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        //This function is often duplicated it a little disturbing . Do something with this 
        public Language GetLanguage(int id)
        {
            return _repository.LanguageRepository.Get(x => x.Id == id);
        }

    }
}

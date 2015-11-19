﻿using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.Results;
using WebBellwether.API.Services.JokeService.Abstract;
using System;
using WebBellwether.API.Repositories.Abstract;

namespace WebBellwether.API.Services.JokeService
{
    public class ManagementJokeService: IManagementJokeService
    {
        private readonly IAggregateRepositories _repository;
        public ManagementJokeService(IAggregateRepositories repository)
        {
            _repository = repository;
        }
        public ResultStateContainer InsertJoke(JokeModel joke)
        {
            if (_repository.JokeDetailRepository.GetFirst(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                //Such verification is very feeble outright stupid, but I do not have time for anything better
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
                    var jokeDetail = _repository.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == joke.Id);
                    if (jokeDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeDetailNotExists };
                    jokeDetail.ToList().ForEach(x =>
                    {
                        _repository.JokeDetailRepository.Delete(x);
                    });
                    var mainJoke = _repository.JokeRepository.GetWithInclude(x => x.Id == joke.Id).FirstOrDefault();
                    if (mainJoke == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage= ResultMessage.JokeNotExists };
                    _repository.JokeRepository.Delete(mainJoke);
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeDeleted };
                }
                else
                {
                    var jokeDetail = _repository.JokeDetailRepository.GetWithInclude(x => x.Id == joke.JokeId).FirstOrDefault();
                    if (jokeDetail != null)
                        _repository.JokeDetailRepository.Delete(jokeDetail);
                    int jokeTranslationCount = 0;
                    if (joke.JokeTranslations != null)
                        joke.JokeTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                jokeTranslationCount++;
                        });
                    if (jokeTranslationCount == 1)//have only one translation , can delete main id for joke . Safe is safe ...
                    {
                        var mainJoke = _repository.JokeRepository.GetWithInclude(x => x.Id == joke.Id).FirstOrDefault();
                        if (mainJoke != null)
                            _repository.JokeRepository.Delete(mainJoke);
                    }
                    _repository.Save();
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
            var entity = _repository.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == jokeId && x.Language.Id == languageId).FirstOrDefault();
            if (entity == null)
                return null;
            JokeModel joke = new JokeModel { Id = jokeId, JokeContent = entity.JokeContent, LanguageId = entity.Language.Id, JokeId = entity.Id };
            return joke;
        }

        public ResultStateContainer PutJoke(JokeModel joke)
        {
            try
            {
                var entity = _repository.JokeDetailRepository.GetWithInclude(x => x.Id == joke.JokeId).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeNotExists };

                if (entity.JokeCategoryDetail.JokeCategory.Id != joke.JokeCategoryId) // in edit i change category 
                {

                    JokeCategoryDetail categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                    if (categoryDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.JokeCategoryNotExistsInDb, ResultValue = GetLanguage(joke.LanguageId) };
                    entity.JokeCategoryDetail = categoryDetail;
                    foreach (AvailableLanguage al in joke.JokeTranslations)
                    {
                        if (al.HasTranslation && al.Language.Id != joke.LanguageId)
                        {
                            JokeCategoryDetail categoryDetailForOtherTranslation = GetJokeCategory(joke.JokeCategoryId, al.Language.Id);
                            if (categoryDetailForOtherTranslation == null)
                                return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryNotExistsInDb, ResultValue = al.Language.LanguageName };
                            var otherTranslationJoke = _repository.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == joke.Id).FirstOrDefault();
                            otherTranslationJoke.JokeCategoryDetail = categoryDetailForOtherTranslation;
                        }
                    }
                }
                else if(!entity.JokeContent.Equals(joke.JokeContent))
                    if (_repository.JokeDetailRepository.GetFirst(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage= ResultMessage.JokeExists };
                entity.JokeContent = joke.JokeContent;
                _repository.Save();
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
                JokeCategoryDetail entityCategory = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                if (entityCategory == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryNotExistsInDb };
                JokeDetail entityDetail = new JokeDetail
                {
                    JokeContent = joke.JokeContent,
                    Language = GetLanguage(joke.LanguageId),
                    JokeCategoryDetail = entityCategory
                };
                if (entityDetail.Language == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage= ResultMessage.LanguageNotExists };
                Joke entity = new Joke
                {
                    CreationDate = DateTime.UtcNow,
                    JokeDetails = new List<JokeDetail>()
                };
                entity.JokeDetails.Add(entityDetail);
                _repository.JokeRepository.Insert(entity);
                _repository.Save();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.JokeAdded };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public JokeCategoryDetail GetJokeCategory(int jokeCategory, int languageId)
        {
            return _repository.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategory && x.Language.Id == languageId).FirstOrDefault();
        }
        public ResultStateContainer InsertSeveralLanguageJoke(JokeModel joke)
        {
            try
            {
                if (joke.JokeId != 0)
                {
                    var entityToEdit = _repository.JokeDetailRepository.GetWithInclude(x => x.Id == joke.JokeId).FirstOrDefault();
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeTranslationNotExists };
                    entityToEdit.JokeContent = joke.JokeContent;
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage=ResultMessage.JokeTranslationEdited };
                }
                if (_repository.JokeDetailRepository.GetFirst(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.JokeExists };
                Language lang = GetLanguage(joke.LanguageId);
                if (lang == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.LanguageNotExists };
                JokeCategoryDetail categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                if (categoryDetail == null)
                    return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.JokeCategoryNotExistsInDb };
                var entity = _repository.JokeRepository.GetWithInclude(x => x.Id == joke.Id).FirstOrDefault();
                entity?.JokeDetails.Add(new JokeDetail
                {
                    JokeContent = joke.JokeContent,
                    Language = lang,
                    JokeCategoryDetail = categoryDetail
                });
                _repository.Save();
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage=ResultMessage.JokeAdded };

            }
            catch (Exception)
            {

                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        //This function is often duplicated it a little disturbing
        public Language GetLanguage(int id)
        {
            return _repository.LanguageRepository.Get(x => x.Id == id);
        }
    }
}
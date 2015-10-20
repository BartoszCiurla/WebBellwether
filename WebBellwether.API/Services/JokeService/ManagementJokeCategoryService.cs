using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;
using System;

namespace WebBellwether.API.Services.JokeService
{
    public class ManagementJokeCategoryService
    {
        private readonly JokeUnitOfWork _unitOfWork;
        public ManagementJokeCategoryService(JokeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultStateContainer InsertJokeCategory(JokeCategoryModel jokeCategory)
        {
            if (_unitOfWork.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                return new ResultStateContainer { ResultState =ResultState.JokeCategoryExistsInDb };
            if (jokeCategory.Id == 0)
                return InsertSingleLanguageJokeCategory(jokeCategory);
            return new ResultStateContainer();
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
                _unitOfWork.JokeCategoryRepository.Insert(entity);
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryAdded };
            }
            catch(Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public Language GetLanguage(int id)
        {
            return _unitOfWork.LanguageRepository.Get(x => x.Id == id);
        }

    }
}

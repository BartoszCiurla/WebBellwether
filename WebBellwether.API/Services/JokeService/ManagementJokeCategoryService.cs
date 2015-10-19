using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;

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
            return new ResultStateContainer();
        }

    }
}

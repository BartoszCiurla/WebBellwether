using System.Web.Http;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;
using WebBellwether.API.Services.IntegrationGameService;
using WebBellwether.API.Services.IntegrationGameService.Abstract;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGames")]
    public class IntegrationGamesController : ApiController
    {
        private readonly IIntegrationGameService _service;
        public IntegrationGamesController()
        {
            _service = new IntegrationGameService(new IntegrationGameUnitOfWork());
        }

        [AllowAnonymous]
        [Route("GetGameFeatureDetails")]
        public IHttpActionResult GetGameFeatureDetails(int language)
        {
            return Ok(_service.GetGameFeatureDetails(language));
        }

        [AllowAnonymous]
        [Route("GetGameFeatuesModelWithDetails")]
        public IHttpActionResult GetGameFeatuesModelWithDetails(int language)
        {
            return Ok(_service.GetGameFeatuesModelWithDetails(language));
        }

        [Authorize]
        [Route("PostIntegrationGame")]
        public IHttpActionResult PostIntegrationGame(NewIntegrationGameModel gameModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //this is stupid but works ehm this don't work i must return id ffss 
            ResultStateContainer result = _service.InsertIntegrationGame(gameModel);
            switch (result.ResultState)
            {
                //this i really stupid ... 
                case ResultState.GameAdded:
                    return Ok(result.Value);//standard 
                case ResultState.ThisGameExistsInDb:
                    return BadRequest(result.Value.ToString());//if game name exists in db
                case ResultState.SeveralLanguageGameAdded:
                    return Ok(result.Value); //standard user can add more game translation
                case ResultState.GameHaveTranslationForThisLanguage:
                    return BadRequest(result.Value.ToString()); // if game have translation for language
                case ResultState.GameTranslationEdited:
                    return Ok(result.Value);
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }
        [Authorize]
        [Route("PostEditIntegrationGame")]
        public IHttpActionResult PostEditIntegrationGame(IntegrationGameModel integrationGame)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ResultStateContainer result =  _service.PutIntegrationGame(integrationGame);
            switch (result.ResultState)
            {
                case ResultState.GameEdited:
                    return Ok();
                case ResultState.GameNotExists:
                    return BadRequest("GameNotExists");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }

        [Authorize]
        [Route("PostDeleteIntegrationGame")]
        public IHttpActionResult PostDeleteIntegrationGame(IntegrationGameModel integrationGame)
        {
            ResultStateContainer result = _service.DeleteIntegratiomGame(integrationGame);
            switch (result.ResultState)
            {
                case ResultState.GameRemoved:
                    return Ok(result);
                case ResultState.GameTranslationNotExists:
                    return BadRequest("GameTranslationNotExists"); 
                case ResultState.GameFeatureTranslationNotExists:
                    return BadRequest("GameFeatureTranslationNotExists");
                case ResultState.Error:
                    return BadRequest(result.Value.ToString());
                default:
                    return BadRequest("CriticalError");
            }
        }

        [Authorize]
        [Route("PostGameFeature")]
        public IHttpActionResult PostGameFeature(GameFeatureModel gameFeatureModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_service.PutGameFeature(gameFeatureModel) == ResultState.GameFeatureEdited)
                return Ok(gameFeatureModel);
            return NotFound();
        }

        [Authorize]
        [Route("PostGameFeatureDetail")]
        public IHttpActionResult PostGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_service.PutGameFeatureDetail(gameFeatureDetailModel) == ResultState.GameFeatureDetailEdited)
                return Ok(gameFeatureDetailModel);
            return NotFound();
        }

        [AllowAnonymous]
        [Route("GetIntegrationGames")]
        public IHttpActionResult GetIntegrationGames(int language)
        {
            return Ok(_service.GetIntegrationGames(language));
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetIntegrationGameTranslation")]
        public IHttpActionResult GetIntegrationGameTranslation(int id,int languageId)//here i have game main id and language id 
        {
           return Ok(_service.GetGameTranslation(id,languageId));
        }

        [Authorize]
        [Route("GetIntegrationGamesWithAvailableLanguage")]
        public IHttpActionResult GetIntegrationGamesWithAvailableLanguage(int language)
        {
            return Ok(_service.GetIntegrationGamesWithAvailableLanguages(language));
        }

        [AllowAnonymous]
        [Route("GetGameFeatures")]
        public IHttpActionResult GetGameFeatures(int language)
        {
            return Ok(_service.GetGameFeatures(language));
        }
    }
}

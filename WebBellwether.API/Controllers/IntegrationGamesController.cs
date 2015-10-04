using System.Web.Http;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Repositories;
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
            _service = new IntegrationGameService(new UnitOfWork());
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
                case ResultState.GameAdded:
                    return Ok(result.Value);//standard 
                case ResultState.ThisGameExistsInDb:
                    return BadRequest(result.Value.ToString());//if game name exists in db
                case ResultState.SeveralLanguageGameAdded:
                    return Ok(result.Value); //standard user can add more game translation
                case ResultState.GameHaveTranslationForThisLanguage:
                    return BadRequest(result.Value.ToString()); // if game have translation for language
                default:
                    return BadRequest();
            }
        }
        [Authorize]
        [Route("PostEditIntegrationGame")]
        public IHttpActionResult PostEditIntegrationGame(IntegrationGameModel integrationGame)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.PutIntegrationGame(integrationGame);
            return Ok();
        }
        [Authorize]
        [Route("PostDeleteIntegrationGame")]
        public IHttpActionResult PostDeleteIntegrationGame(IntegrationGameModel integrationGame)
        {
            _service.DeleteIntegratiomGame(integrationGame);
            return Ok();
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
        public IHttpActionResult GetIntegrationGames(int language)//tutaj bedzie treba przesyłać id jezyka ... 
        {
            return Ok(_service.GetIntegrationGames(language));
        }

        [Authorize]
        [Route("GetIntegrationGamesWithAvailableLanguage")]
        public IHttpActionResult GetIntegrationGamesWithAvailableLanguage(int language)
        {
            return Ok(_service.GetIntegrationGamesWithAvailableLanguages(language));
        }

        [AllowAnonymous]
        [Route("GetGameFeatures")]
        public IHttpActionResult GetGameFeatures(int language)//tutaj bedzie treba przesyłać id jezyka ... 
        {
            return Ok(_service.GetGameFeatures(language));
        }
    }
}

﻿using System.Collections.Generic;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Models.Translation;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.IntegrationGameService.Abstract
{
    public interface IIntegrationGameService
    {
        List<IntegrationGameModel> GetIntegrationGames(int language);
        List<IntegrationGameModel> GetIntegrationGamesWithAvailableLanguages(int language);
        ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel);
        ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel);
        List<GameFeatureDetailModel> GetGameFeatureDetails(int language);
        List<AvailableLanguage> FillAvailableTranslation(int gameId, List<Language> allLanguages);
        List<IntegrationGameDetailModel> FillGameDetailModel(int integrationGameDetailId);
        ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game);
        List<GameFeatureModel> GetGameFeatures(int language);
        List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language);
        ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame);
        ResultStateContainer DeleteIntegratiomGame(IntegrationGameModel integrationGame);
        IntegrationGameModel GetGameTranslation(int gameId,int languageId);

    }
}

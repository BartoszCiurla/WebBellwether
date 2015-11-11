﻿namespace WebBellwether.API.Results
{
    public enum ResultState
    {
        //aby tutaj mniej tego było w przyszlośći trzeba to podzielić na kilka kluczowych resultstatów a nie takie z dupy rozdrabnianie 
        GameAdded,
        GameAddFailure,
        GameTranslationAdded,
        GameTranslationNotExists,
        GameFeatureTranslationNotExists,
        GameEdited,
        GameNotEdited,
        GameNotExists,
        RemoveGameError,
        GameRemoved,
        ThisGameExistsInDb,
        GameFeatureEdited,
        GameFeatureNotExists,
        GameFeatureDetailEdited,
        GameFeatureDetailNotExists,
        GameCanBeAdded,
        GameHaveTranslationForThisLanguage,
        SeveralLanguageGameAdded,
        GameTranslationEdited,    
        JokeCategoryExistsInDb,   
        JokeCategoryAdded,
        JokeCategoryNotExistsInDb,
        JokeCategoryEdited,
        JokeCategoryTranslationNotExists,
        JokeCategoryDeleted,
        ThisJokeCategoryExists,
        JokeCategoryTranslationEdited,
        JokeCategoryTranslationAdded,
        JokeExists,
        LanguageNotExists,
        JokeAdded,
        JokeDeleted,
        JokeDetailNotExists,
        JokeNotExists,
        JokeTranslationNotExists,
        JokeTranslationEdited,
        JokeEdited,
        JokeNotEdited,
        LanguageKeyValueEdited,
        Error
    }
}
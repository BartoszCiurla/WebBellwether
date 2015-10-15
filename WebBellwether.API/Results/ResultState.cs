namespace WebBellwether.API.Results
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
        //w przyszłości beda uzywane tylko te 
        Added,
        NotExists,
        Error

    }
}
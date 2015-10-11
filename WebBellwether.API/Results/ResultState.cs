namespace WebBellwether.API.Results
{
    public enum ResultState
    {
        GameAdded,
        GameAddFailure,
        GameTranslationAdded,
        GameTranslationNotExists,
        GameFeatureTranslationNotExists,
        RemoveGameError,
        GameRemoved,
        ThisGameExistsInDb,
        GameFeatureEdited,
        GameFeatureNotExists,
        GameFeatureDetailEdited,
        GameFeatureDetailNotExists,
        GameCanBeAdded,
        GameHaveTranslationForThisLanguage,
        SeveralLanguageGameAdded
    }
}
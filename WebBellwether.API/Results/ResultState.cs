namespace WebBellwether.API.Results
{
    public enum ResultState
    {
        GameAdded,
        GameAddFailure,
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
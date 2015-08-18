namespace WebBellwether.API.Results
{
    public enum ResultState
    {
        GameAdded,
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
namespace WebBellwether.API.Models
{
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    };

    public enum ResultState
    {
        GameAdded,
        ThisGameExistsInDb,
        GameFeatureEdited,
        GameFeatureNotExists

    };
}
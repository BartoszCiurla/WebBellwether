using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBellwether.API.Results
{
    public enum ResultMessage
    {
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
        LanguageEdited,
        OnlyOnePublicLanguage,
        LanguageHasBeenPublished,
        LanguageHasBeenNonpublic,
        EmptyKeysExists,
        LanguageFileNotExists,
        LanguageFileRemoved,
        LanguageRemoved,
        LanguageFileAdded,
        LanguageExists,
        LanguageAdded,
        LanguageCanNotBeRemoved,
        Error
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.IntegrationGames;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Context
{
    public static class InitSeed
    {
        public static IEnumerable<Language> BuildLanguagesList()
        {
            return new List<Language>
            {

                new Language
                {
                    LanguageName = "English",
                    // ShortLanguageName = "en"
                },
                new Language
                {
                    LanguageName = "Polish",
                    // ShortLanguageName = "pl"
                },
            };
        }
    }
}

//public static IEnumerable<GameCategory> BuildGameCategories(List<Language> languages)
        //{
        //    var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
        //    var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));
        //    var newGameCategories = new List<GameCategory>
        //    {
        //        new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Integration",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Integracja",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //         new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Concentration",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Koncentracja",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //              new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Creativity",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Kreatywność",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //            new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Gone to group",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Podział na grupy",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //         new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Let us learn to",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Poznajmy się",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //         new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Team work",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Praca w grupie",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //        new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Discharge energy",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Rozładowanie energii",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //        new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Competition",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Współzawodnictwo",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //         new GameCategory
        //        {
        //            GameCategoryLanguages = new Collection<GameCategoryLanguage>
        //            {
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Dexterity",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new GameCategoryLanguage
        //                {
        //                    GameCategoryName = "Zręczność",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //    };

            //var gameCategories = new List<GameCategory>
            //{

            //    new GameCategory
            //    {
            //        GameCategoryName = "Integration",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Integracja",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //     new GameCategory
            //    {
            //        GameCategoryName = "Concentration",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Koncentracja",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //      new GameCategory
            //    {
            //        GameCategoryName = "Creativity",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Kreatywność",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //       new GameCategory
            //    {
            //        GameCategoryName = "Gone to group",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Podział na grupy",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //             new GameCategory
            //    {
            //        GameCategoryName = "Let us learn to",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Poznajmy się",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //                new GameCategory
            //    {
            //        GameCategoryName = "Team work",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Praca w grupie",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //                  new GameCategory
            //    {
            //        GameCategoryName = "Discharge energy",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Rozładowanie energii",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //                      new GameCategory
            //    {
            //        GameCategoryName = "Competition",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Współzawodnictwo",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //                       new GameCategory
            //    {
            //        GameCategoryName = "Dexterity",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
            //    },
            //    new GameCategory
            //    {
            //        GameCategoryName = "Zręczność",
            //        Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
            //    },
            //};
            //return gameCategories;
        //    return newGameCategories;
        //}


        //public static IEnumerable<PreparationFun> BuildPreparationFuns(List<Language> languages)
        //{
        //    var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
        //    var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));
        //    var newPreparationFun = new List<PreparationFun>
        //    {
        //        new PreparationFun
        //        {
        //            PreparationFunLanguages = new Collection<PreparationFunLanguage>
        //            {
        //                new PreparationFunLanguage
        //                {
        //                    PreparationFunName = "Lack",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new PreparationFunLanguage
        //                {
        //                    PreparationFunName = "Brak",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //        new PreparationFun
        //        {
        //            PreparationFunLanguages = new Collection<PreparationFunLanguage>
        //            {
        //                new PreparationFunLanguage
        //                {
        //                    PreparationFunName = "Minimum",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new PreparationFunLanguage
        //                {
        //                    PreparationFunName = "Minimalne",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //         new PreparationFun
        //        {
        //            PreparationFunLanguages = new Collection<PreparationFunLanguage>
        //            {
        //                new PreparationFunLanguage
        //                {
        //                    PreparationFunName = "Required",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new PreparationFunLanguage
        //                {
        //                    PreparationFunName = "Wymagane",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        }
        //    };
        //    return newPreparationFun;
            //var preparationFuns = new List<PreparationFun>
            //{
            //   new PreparationFun
            //    {
            //        PreparationFunName = "Lack",Language = languages.FirstOrDefault(x=>x.LanguageName.StartsWith("English"))
            //    },
            //    new PreparationFun
            //    {
            //        PreparationFunName = "Brak",Language = languages.FirstOrDefault(x=>x.LanguageName.StartsWith("Polish"))
            //    },
            //     new PreparationFun
            //    {
            //        PreparationFunName = "Minimum",Language = languages.FirstOrDefault(x=>x.LanguageName.StartsWith("English"))
            //    },
            //    new PreparationFun
            //    {
            //        PreparationFunName = "Minimalne",Language = languages.FirstOrDefault(x=>x.LanguageName.StartsWith("Polish"))
            //    },
            //      new PreparationFun
            //    {
            //        PreparationFunName = "Required",Language = languages.FirstOrDefault(x=>x.LanguageName.StartsWith("English"))
            //    },
            //    new PreparationFun
            //    {
            //        PreparationFunName = "Wymagane",Language = languages.FirstOrDefault(x=>x.LanguageName.StartsWith("Polish"))
            //    },
            //};
            //return preparationFuns;
        

        //public static IEnumerable<PaceOfPlay> BuildPaceOfPlays(List<Language> languages)
        //{
        //    var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
        //    var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));

        //    var newPaceOfPlay = new List<PaceOfPlay>
        //    {
        //        new PaceOfPlay
        //        {
        //            PaceOfPlayLanguages = new Collection<PaceOfPlayLanguage>
        //            {
        //                new PaceOfPlayLanguage
        //                {
        //                    PaceOfPlayName = "Dynamic",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new PaceOfPlayLanguage
        //                {
        //                    PaceOfPlayName = "Dynamiczne",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //         new PaceOfPlay
        //        {
        //            PaceOfPlayLanguages = new Collection<PaceOfPlayLanguage>
        //            {
        //                new PaceOfPlayLanguage
        //                {
        //                    PaceOfPlayName = "Static",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new PaceOfPlayLanguage
        //                {
        //                    PaceOfPlayName = "Statyczne",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //           new PaceOfPlay
        //        {
        //            PaceOfPlayLanguages = new Collection<PaceOfPlayLanguage>
        //            {
        //                new PaceOfPlayLanguage
        //                {
        //                    PaceOfPlayName = "Living",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new PaceOfPlayLanguage
        //                {
        //                    PaceOfPlayName = "Żywe",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //    };
        //    return newPaceOfPlay;
        //    //var paceOfPlays = new List<PaceOfPlay>
        //    //{
        //    //    new PaceOfPlay
        //    //    {
        //    //        PaceOfPlayName = "Dynamic",Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))                   
        //    //    },
        //    //     new PaceOfPlay
        //    //    {
        //    //       PaceOfPlayName = "Dynamiczne",Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))                   
        //    //    },
        //    //     new PaceOfPlay
        //    //    {
        //    //        PaceOfPlayName = "Static",Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))                   
        //    //    },
        //    //     new PaceOfPlay
        //    //    {
        //    //       PaceOfPlayName = "Statyczne",Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))                   
        //    //    },
        //    //     new PaceOfPlay
        //    //    {
        //    //        PaceOfPlayName = "Living",Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))                   
        //    //    },
        //    //     new PaceOfPlay
        //    //    {
        //    //       PaceOfPlayName = "Żywe",Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))                   
        //    //    }                  
        //    //};
        //    //return paceOfPlays;
        //}

        //public static IEnumerable<GameFeature> BuildGameFeatures(List<Language> languages)
        //{
        //    var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
        //    var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));

        //    var newGameFeatures = new List<GameFeature>
        //    {
        //        new GameFeature
        //        {
        //            GameFeatureLanguages = new Collection<GameFeatureLanguage>
        //            {
        //                new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Category game",
        //                    Language = en.LanguageName,
        //                    LanguageId = en.Id
        //                },
        //                 new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Kategoria zabawy",
        //                    Language = pl.LanguageName,
        //                    LanguageId = pl.Id
        //                }
        //            },
        //            GameFeatureDetails = new Collection<GameFeatureDetail>
        //            {
        //                new GameFeatureDetail
        //                {
        //                    GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
        //                    {
        //                        new GameFeatureDetailLanguage
        //                        {
        //                            GameFeatureDetailName = "Integration",
        //                            LanguageId = en.Id,
        //                            Language = en.LanguageName
        //                        },
        //                         new GameFeatureDetailLanguage
        //                        {
        //                            GameFeatureDetailName = "Integration",
        //                            LanguageId = en.Id,
        //                            Language = en.LanguageName
        //                        },

        //                    }
        //                }
        //            }

                   
        //        },
        //          new GameFeature
        //        {
        //            GameFeatureLanguages = new Collection<GameFeatureLanguage>
        //            {
        //                new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Pace of play",
        //                    Language = en.LanguageName,
        //                    LanguageId = en.Id
        //                },
        //                 new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Tempo zabawy",
        //                    Language = pl.LanguageName,
        //                    LanguageId = pl.Id
        //                }
        //            }
        //        },
        //           new GameFeature
        //        {
        //            GameFeatureLanguages = new Collection<GameFeatureLanguage>
        //            {
        //                new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Number of player",
        //                    Language = en.LanguageName,
        //                    LanguageId = en.Id
        //                },
        //                 new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Liczba graczy",
        //                    Language = pl.LanguageName,
        //                    LanguageId = pl.Id
        //                }
        //            }
        //        },
        //          new GameFeature
        //        {
        //            GameFeatureLanguages = new Collection<GameFeatureLanguage>
        //            {
        //                new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Preparation fun",
        //                    Language = en.LanguageName,
        //                    LanguageId = en.Id
        //                },
        //                 new GameFeatureLanguage
        //                {
        //                    GameFeatureName = "Przygotowanie zabawy",
        //                    Language = pl.LanguageName,
        //                    LanguageId = pl.Id
        //                }
        //            }
        //        }
        //    };
        //    return newGameFeatures;
        //}

        //public static IEnumerable<NumberOfPlayer> BuildNumberOfPlayers(List<Language> languages)
        //{
        //    var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
        //    var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));

        //    var newNumberOfPlayers = new List<NumberOfPlayer>
        //    {
        //        new NumberOfPlayer
        //        {
        //            NumberOfPlayerLanguages = new Collection<NumberOfPlayerLanguage>
        //            {
        //                new NumberOfPlayerLanguage
        //                {
        //                    NumberOfPlayerName = "Optional",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new NumberOfPlayerLanguage
        //                {
        //                    NumberOfPlayerName = "Dowolna",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        },
        //        new NumberOfPlayer
        //        {
        //            NumberOfPlayerLanguages = new Collection<NumberOfPlayerLanguage>
        //            {
        //                new NumberOfPlayerLanguage
        //                {
        //                    NumberOfPlayerName = "More than 20",
        //                    LanguageId = en.Id,
        //                    Language = en.LanguageName
        //                },
        //                new NumberOfPlayerLanguage
        //                {
        //                    NumberOfPlayerName = "Powyżej 20",
        //                    LanguageId = pl.Id,
        //                    Language = pl.LanguageName
        //                }
        //            }
        //        }
        //    };
        //    return newNumberOfPlayers;

        //    //var numberOfPlayers = new List<NumberOfPlayer>
        //    //{
        //    //    new NumberOfPlayer
        //    //    {
        //    //        NumberOfPlayerName=  "Optional", Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
        //    //    },
        //    //    new NumberOfPlayer
        //    //    {
        //    //        NumberOfPlayerName =  "Dowolna", Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
        //    //    },

        //    //      new NumberOfPlayer
        //    //    {
        //    //        NumberOfPlayerName= "More than 20", Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"))
        //    //    },
        //    //    new NumberOfPlayer
        //    //    {
        //    //        NumberOfPlayerName =  "Powyżej 20", Language = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"))
        //    //    }                      
        //    //};
        //    //return numberOfPlayers;
        //}

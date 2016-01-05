using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Repositories.Context
{
    public static class InitSeed
    {
        public static IEnumerable<LanguageDao> BuildLanguagesList()
        {
            return new List<LanguageDao>
            {

                new LanguageDao
                {
                    LanguageName = "English",
                    LanguageShortName = "en",
                    IsPublic = true
                },
                new LanguageDao
                {
                    LanguageName = "Polish",
                    LanguageShortName = "pl",
                    IsPublic = true
                },
            };
        }

        public static IEnumerable<GameFeatureDao> BuildGameFeatures(List<LanguageDao> languages)
        {
            var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
            var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));

            var newGameFeatures = new List<GameFeatureDao>
            {
                new GameFeatureDao
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguageDao>
                    {
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Category game",
                            Language = en
                        },
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Kategoria zabawy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetailDao>
                    {
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Integration",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Integracja",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Concentration",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Koncentracja",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Creativity",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Kreatywność",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Gone to group",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Podział na grupy",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Let us learn to",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Poznajmy się",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Team work",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Praca w grupie",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Discharge energy",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Rozładowanie energii",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Competition",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Współzawodnictwo",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Dexterity",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Zręczność",
                                    Language = pl
                                },
                            }
                        },
                    }
                },
                new GameFeatureDao
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguageDao>
                    {
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Pace of play",
                            Language = en
                        },
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Tempo zabawy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetailDao>
                    {
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Dynamic",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Dynamiczne",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Static",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Statyczne",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Living",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Żywe",
                                    Language = pl
                                },
                            }
                        }
                    }
                },
                new GameFeatureDao
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguageDao>
                    {
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Number of player",
                            Language = en
                        },
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Liczba graczy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetailDao>
                    {
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Optional",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Dowolna",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "More than 20",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Powyżej 20",
                                    Language = pl
                                },
                            }
                        }
                    }
                },
                new GameFeatureDao
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguageDao>
                    {
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Preparation fun",
                            Language = en
                        },
                        new GameFeatureLanguageDao
                        {
                            GameFeatureName = "Przygotowanie zabawy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetailDao>
                    {
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Lack",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Brak",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Minimum",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Minimalne",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetailDao
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>
                            {
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Required",
                                    Language = en
                                },
                                new GameFeatureDetailLanguageDao
                                {
                                    GameFeatureDetailName = "Wymagane",
                                    Language = pl
                                },
                            }
                        }
                    }
                }
            };





            return newGameFeatures;
        }
    }
}



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.IntegrationGame;
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
                },
                new Language
                {
                    LanguageName = "Polish",
                },
            };
        }

        public static IEnumerable<GameFeature> BuildGameFeatures(List<Language> languages)
        {
            var en = languages.FirstOrDefault(x => x.LanguageName.StartsWith("English"));
            var pl = languages.FirstOrDefault(x => x.LanguageName.StartsWith("Polish"));

            var newGameFeatures = new List<GameFeature>();

            newGameFeatures.Add(new GameFeature
            {
                GameFeatureLanguages = new Collection<GameFeatureLanguage>
                {
                    new GameFeatureLanguage
                    {
                        GameFeatureName = "Category game",
                        Language = en
                        
                    },
                    new GameFeatureLanguage
                    {
                        GameFeatureName = "Kategoria zabawy",
                        Language = pl
                    }
                },
                GameFeatureDetails = new Collection<GameFeatureDetail>
                {
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Integration",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Integracja",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Concentration",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Koncentracja",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Creativity",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Kreatywność",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Gone to group",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Podział na grupy",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Let us learn to",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Poznajmy się",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Team work",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Praca w grupie",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Discharge energy",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Rozładowanie energii",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Competition",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Współzawodnictwo",
                                Language = pl
                            },
                        }
                    },
                    new GameFeatureDetail
                    {
                        GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                        {
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Dexterity",
                                Language = en
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Zręczność",
                                Language = pl
                            },
                        }
                    },

                }
            });
            newGameFeatures.Add(
                new GameFeature
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguage>
                    {
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Pace of play",
                            Language = en
                        },
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Tempo zabawy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetail>
                    {
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Dynamic",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Dynamiczne",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Static",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Statyczne",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName= "Living",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Żywe",
                                    Language = pl
                                },
                            }
                        }
                    }
                });


            newGameFeatures.Add(
                new GameFeature
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguage>
                    {
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Number of player",
                            Language = en
                        },
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Liczba graczy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetail>
                    {
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Optional",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Dowolna",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "More than 20",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName= "Powyżej 20",
                                    Language = pl
                                },
                            }
                        }
                    }

                });
            newGameFeatures.Add(
                new GameFeature
                {
                    GameFeatureLanguages = new Collection<GameFeatureLanguage>
                    {
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Preparation fun",
                            Language = en
                        },
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Przygotowanie zabawy",
                            Language = pl
                        }
                    },
                    GameFeatureDetails = new Collection<GameFeatureDetail>
                    {
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Lack",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Brak",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName= "Minimum",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                {GameFeatureDetailName = "Minimalne",
                                    Language = pl
                                },
                            }
                        },
                        new GameFeatureDetail
                        {
                            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguage>
                            {
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Required",
                                    Language = en
                                },
                                new GameFeatureDetailLanguage
                                { GameFeatureDetailName = "Wymagane",
                                    Language = pl
                                },
                            }
                        }
                    }
                });


            return newGameFeatures;
        }
    }
}



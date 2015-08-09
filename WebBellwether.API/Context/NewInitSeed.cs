using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Entities.IntegrationGames;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Context
{
    public static class NewInitSeed
    {
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
                        Language = en.LanguageName,
                        LanguageId = en.Id
                    },
                    new GameFeatureLanguage
                    {
                        GameFeatureName = "Kategoria zabawy",
                        Language = pl.LanguageName,
                        LanguageId = pl.Id
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Integracja",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Koncentracja",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Kreatywność",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Podział na grupy",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Poznajmy się",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Praca w grupie",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Rozładowanie energii",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Współzawodnictwo",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                                LanguageId = en.Id,
                                Language = en.LanguageName
                            },
                            new GameFeatureDetailLanguage
                            {
                                GameFeatureDetailName = "Zręczność",
                                LanguageId = pl.Id,
                                Language = pl.LanguageName
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
                            Language = en.LanguageName,
                            LanguageId = en.Id
                        },
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Tempo zabawy",
                            Language = pl.LanguageName,
                            LanguageId = pl.Id
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {
                                    GameFeatureDetailName = "Dynamiczne",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {
                                      GameFeatureDetailName = "Statyczne",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {
                                      GameFeatureDetailName = "Żywe",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            Language = en.LanguageName,
                            LanguageId = en.Id
                        },
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Liczba graczy",
                            Language = pl.LanguageName,
                            LanguageId = pl.Id
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {
                                     GameFeatureDetailName = "Dowolna",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {
                                      GameFeatureDetailName= "Powyżej 20",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            Language = en.LanguageName,
                            LanguageId = en.Id
                        },
                        new GameFeatureLanguage
                        {
                            GameFeatureName = "Przygotowanie zabawy",
                            Language = pl.LanguageName,
                            LanguageId = pl.Id
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {
                                     GameFeatureDetailName = "Brak",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                {GameFeatureDetailName = "Minimalne",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
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
                            LanguageId = en.Id,
                            Language = en.LanguageName
                                },
                                new GameFeatureDetailLanguage
                                { GameFeatureDetailName = "Wymagane",
                            LanguageId = pl.Id,
                            Language = pl.LanguageName
                                },
                            }
                        }
                    }
                });


            return newGameFeatures;
        }
    }
}

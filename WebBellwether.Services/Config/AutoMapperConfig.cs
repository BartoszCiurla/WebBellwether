using AutoMapper;
using WebBellwether.Models.Models.Auth;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Repositories.Entities.Auth;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Services.Config
{
    public class AutoMapperConfig
    {
        public AutoMapperConfig()
        {
            Mapper.CreateMap<Language, LanguageDao>();
            Mapper.CreateMap<LanguageDao, Language>();

            Mapper.CreateMap<Client, ClientDao>();
            Mapper.CreateMap<ClientDao, Client>();

            Mapper.CreateMap<RefreshToken, RefreshTokenDao>();
            Mapper.CreateMap<RefreshTokenDao, RefreshToken>();
        }
    }
}

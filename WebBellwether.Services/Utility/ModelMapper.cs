using WebBellwether.Services.Config;
using AutoMapper;

namespace WebBellwether.Services.Utility
{
    public static class ModelMapper
    {
        public static AutoMapperConfig AutoMapperConfig { get; set; }
        public static TModelDestination Map<TModelDestination, TModelSource>(TModelSource source)
        {
            if (AutoMapperConfig == null)
            {
                AutoMapperConfig = new AutoMapperConfig();
            }

            return Mapper.Map<TModelSource, TModelDestination>(source);
        }
    }
}

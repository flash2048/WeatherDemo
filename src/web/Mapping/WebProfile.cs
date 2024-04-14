using AutoMapper;
using Infrastructure.EF.Entities;
using Infrastructure.Helpers;
using Models;

namespace Weather.Mapping
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ForecastEntity, Forecast>().ReverseMap();

            CreateMap<ForecastEntity, ForecastView>()
                .ForMember(dst => dst.Description, opt => opt.Ignore())
                .AfterMap((src, dst, _) =>
            {
                dst.Description = ForecastConverter.ConvertTempToDescription((src.MinTemperature + src.MaxTemperature) / 2);
            });
        }
    }
}

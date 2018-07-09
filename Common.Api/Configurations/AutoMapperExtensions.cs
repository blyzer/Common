using AutoMapper;
using Common.Api.Models;
using Common.Entities.Models;
using Microsoft.AspNetCore.Builder;

namespace Common.Api.Configurations
{
    public static class AutoMapperExtensions
    {
        public static void ConfigureMaps(this IApplicationBuilder app)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserViewModel>().ReverseMap();
                config.CreateMap<Role, RolViewModel>().ReverseMap();
                config.CreateMap<Province, ProvinceViewModel>().ReverseMap();
                config.CreateMap<Municipality, MunicipalityViewModel>().ReverseMap();
                config.CreateMap<District, DistrictViewModel>().ReverseMap();
                config.CreateMap<Address, AddressViewModel>().ReverseMap();
                config.CreateMap<ContactInformation, ContactInfomationViewModel>().ReverseMap();                
            });


        }
        public static TOut Map<TIn, TOut>(this TIn currentModel)
        where TOut : class
        {
            return Mapper.Map<TIn, TOut>(currentModel);
        }
    }
}

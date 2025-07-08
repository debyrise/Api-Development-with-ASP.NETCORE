using AutoMapper;
using WebApiDemo.Model;
using WebApiDemo.Model.Dto;

namespace WebApiDemo
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //CreateMap<Villa, VillaDto>().ReverseMap();

            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();


            CreateMap<VillaNumberDto, VillaNumber>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();


        }
    }
}

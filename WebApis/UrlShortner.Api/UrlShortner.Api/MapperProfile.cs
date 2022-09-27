using AutoMapper;
using UrlShortner.Api.Model;
using UrlShortner.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<LongUrlDto, LongUrl>();
    }
}
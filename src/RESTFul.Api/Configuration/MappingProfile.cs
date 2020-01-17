using AutoMapper;
using RESTFul.Api.Models;
using RESTFul.Api.ViewModels;

namespace RESTFul.Api.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserViewModel>();
        }


    }
}

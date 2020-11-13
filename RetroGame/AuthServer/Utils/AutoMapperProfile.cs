using AuthServer.Dtos;
using AuthServer.Models;
using AutoMapper;

namespace AuthServer.Utils
{
    public class AutoMapperProfile : Profile
    {
        #region Constructor

        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }

        #endregion
    }
}
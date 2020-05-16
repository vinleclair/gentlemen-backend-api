using AutoMapper;

namespace Gentlemen.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Customer, User>(MemberList.None);
        }
    }
}
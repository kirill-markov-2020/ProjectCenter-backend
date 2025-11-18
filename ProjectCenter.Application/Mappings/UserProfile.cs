using AutoMapper;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Core.Entities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src =>
                    src.IsAdmin ? "Admin" :
                    src.Teacher != null ? "Teacher" :
                    src.Student != null ? "Student" : "User"))
            .ForMember(dest => dest.Photo,
                opt => opt.MapFrom(src => src.Photo))
            .ForMember(dest => dest.GroupName,
                opt => opt.MapFrom(src =>
                    src.Student != null ? src.Student.Group.Name : null))
            .ForMember(dest => dest.CuratorName,
                opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.Teacher != null
                        ? $"{src.Student.Teacher.User.Surname} {src.Student.Teacher.User.Name}"
                        : null));
    }
}

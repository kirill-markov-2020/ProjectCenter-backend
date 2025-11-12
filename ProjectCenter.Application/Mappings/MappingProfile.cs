using AutoMapper;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.UpdateUser;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.User.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.User.Name))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserFullName,
                           opt => opt.MapFrom(src => $"{src.User.Surname} {src.User.Name}"))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));


            CreateMap<User, UpdateUserResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Surname} {src.Name} {src.Patronymic}".Trim()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                    src.IsAdmin ? "Admin" :
                    src.Teacher != null ? "Teacher" :
                    src.Student != null ? "Student" : "User"))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo));
        }
    }
}

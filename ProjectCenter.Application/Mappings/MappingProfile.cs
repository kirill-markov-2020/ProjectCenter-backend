using AutoMapper;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Project, ProjectDto>()
                 .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                      $"{src.Student.User.Surname} {src.Student.User.Name} {src.Student.User.Patronymic}".Trim()))
                 .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                      src.Teacher != null && src.Teacher.User != null
                          ? $"{src.Teacher.User.Surname} {src.Teacher.User.Name} {src.Teacher.User.Patronymic}".Trim()
                          : null))
                 .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                 .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                 .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                 .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserFullName,
                           opt => opt.MapFrom(src => $"{src.User.Surname} {src.User.Name} {src.User.Patronymic}".Trim()))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        }
    }
}

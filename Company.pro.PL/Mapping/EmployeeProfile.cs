using AutoMapper;
using Company.pro.DAL.Models;
using Company.pro.PL.Dtos;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Company.pro.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();

            CreateMap<Employee, CreateEmployeeDto>()
                //.ForMember(d => d.DepartmentName,o => o.MapFrom(S => S.Department.Name))
                ;

        }
    }
}

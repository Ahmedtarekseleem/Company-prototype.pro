using AutoMapper;
using Company.pro.DAL.Models;
using Company.pro.PL.Dtos;

namespace Company.pro.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department, CreateDepartmentDto>();
            
        }
    }
}

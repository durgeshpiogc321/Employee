using AutoMapper;
using Repositories.DatabaseModels.ResultSet;
using Shared.Models;

namespace Employee.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetAllEmployee_Result, GetAllEmployeeDto>();
        }
    }
}

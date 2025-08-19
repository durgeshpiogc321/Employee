using AutoMapper;
using Repositories.DatabaseModels;
using Repositories.IRepository;
using Services.IServices;
using Shared.Common;
using Shared.Models;
using System.Net;

namespace Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAllEmployeeDto>> GetAllEmployee(EmployeeRequestModel model)
        {
            var result = await employeeRepository.GetAllEmployee(model);

            return _mapper.Map<List<GetAllEmployeeDto>>(result);

            //return result.Select(a => new GetAllEmployeeDto()
            //{
            //    Id = a.Id,
            //    Name = a.Name,
            //    Email = a.Email,
            //    Address = a.Address,
            //    Dob = a.Dob,
            //    PhoneNumber = a.PhoneNumber,
            //    ProfilePic = a.ProfilePic,
            //    CreatedDate = a.CreatedDate,
            //    IsActive = a.IsActive,
            //    TotalRecords = a.TotalRecords
            //}).ToList();

        }

        public async Task<List<GetAllEmployeeDto>> GetAllEmployee_V1(EmployeeRequestModel model)
        {
            var result = await employeeRepository.GetAllEmployee_V1(model);
            return _mapper.Map<List<GetAllEmployeeDto>>(result);

            //if (result != null && result.Count > 0)
            //{
            //    return result.Select(a => new GetAllEmployeeDto()
            //    {
            //        Id = a.Id,
            //        Name = a.Name,
            //        Email = a.Email,
            //        Address = a.Address,
            //        Dob = a.Dob,
            //        PhoneNumber = a.PhoneNumber,
            //        ProfilePic = a.ProfilePic,
            //        TotalRecords = a.TotalRecords
            //    }).ToList();
            //}
            //else
            //{
            //    return new List<GetAllEmployeeDto>();
            //}
        }


        public async Task<EmployeeViewModel> GetEmployee(long id)
        {
            var result = await employeeRepository.GetEmployee(id);
            if (result != null && result.Id > 0)
            {
                return new EmployeeViewModel()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                    Address = result.Address,
                    Dob = result.Dob?.ToString("dd MMM, yyyy") ?? "",
                    PhoneNumber = result.PhoneNumber,
                    ProfilePic = result.ProfilePic,
                    IsActive = result.IsActive ?? false
                };
            }
            else
            {
                return new EmployeeViewModel();
            }
        }


        public async Task<Responses<long>> SaveEmployee(EmployeeViewModel model)
        {


            var aaaaa = "anc";
            model.ProfilePic = "" == null ? null : null;
            Employee employee = new Employee()
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
                Dob = Convert.ToDateTime(model.Dob ?? null),
                PhoneNumber = model.PhoneNumber,
                ProfilePic = model.ProfilePic,
                IsActive = model.IsActive
            };
            var result = await employeeRepository.SaveEmployee(employee);
            if (model.Id == 0)
            {
                return new Responses<long>(data: result, statusCode: HttpStatusCode.OK, message: "Employee created successfully");
            }
            else if (model.Id > 0)
            {
                return new Responses<long>(data: result, statusCode: HttpStatusCode.OK, message: "Employee updated successfully");
            }
            else
            {
                return new Responses<long>(data: result, statusCode: HttpStatusCode.OK, message: "Something went wrong");
            }
        }

        public async Task<Responses<bool>> DeleteEmployee(long id)
        {
            var result = await employeeRepository.DeleteEmployee(id);
            if (result)
            {
                return new Responses<bool>(data: result, statusCode: HttpStatusCode.OK, message: "Employee has been deleted successfully");
            }
            else
            {
                return new Responses<bool>(data: result, statusCode: HttpStatusCode.OK, message: "Something went wrong");
            }
        }

        public async Task<Responses<bool>> ActiveEmployee(long id)
        {
            var status = await employeeRepository.ActiveEmployee(id);
            if (status != null)
            {
                string message = status ?? false ? "de-activated" : "activated";
                return new Responses<bool>(data: status ?? false, statusCode: HttpStatusCode.OK, message: "Employee has been " + message + " successfully");
            }
            else
            {
                return new Responses<bool>(data: status ?? false, statusCode: HttpStatusCode.OK, message: "Something went wrong");
            }
        }

        public async Task<bool> IsEmailExist(string email, long id)
        {
            return await employeeRepository.IsEmailExist(email, id);
        }
    }
}

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
            // Input validation
            if (model == null)
            {
                return new List<GetAllEmployeeDto>();
            }

            var result = await employeeRepository.GetAllEmployee(model);
            return _mapper.Map<List<GetAllEmployeeDto>>(result);
        }

        public async Task<List<GetAllEmployeeDto>> GetAllEmployee_V1(EmployeeRequestModel model)
        {
            // Input validation
            if (model == null)
            {
                return new List<GetAllEmployeeDto>();
            }

            var result = await employeeRepository.GetAllEmployee_V1(model);
            return _mapper.Map<List<GetAllEmployeeDto>>(result);
        }


        public async Task<EmployeeViewModel> GetEmployee(long id)
        {
            // Input validation
            if (id <= 0)
            {
                return new EmployeeViewModel();
            }

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
            try
            {
                // Input validation
                if (model == null)
                {
                    return new Responses<long>(data: 0, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Employee data is required");
                }

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    return new Responses<long>(data: 0, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Email is required");
                }

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return new Responses<long>(data: 0, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Name is required");
                }

                // Validate business rules
                if (await IsEmailExist(model.Email, model.Id))
                {
                    return new Responses<long>(data: 0, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Email already exists");
                }

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
                    return new Responses<long>(data: result, statusCode: HttpStatusCode.Created, 
                                             message: "Employee created successfully");
                }
                else
                {
                    return new Responses<long>(data: result, statusCode: HttpStatusCode.OK, 
                                             message: "Employee updated successfully");
                }
            }
            catch (Exception)
            {
                // Log exception
                return new Responses<long>(data: 0, statusCode: HttpStatusCode.InternalServerError, 
                                         message: "An error occurred while saving employee");
            }
        }

        public async Task<Responses<bool>> DeleteEmployee(long id)
        {
            try
            {
                // Input validation
                if (id <= 0)
                {
                    return new Responses<bool>(data: false, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Invalid employee ID");
                }

                var result = await employeeRepository.DeleteEmployee(id);
                if (result)
                {
                    return new Responses<bool>(data: result, statusCode: HttpStatusCode.OK, 
                                             message: "Employee has been deleted successfully");
                }
                else
                {
                    return new Responses<bool>(data: result, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Failed to delete employee");
                }
            }
            catch (Exception)
            {
                return new Responses<bool>(data: false, statusCode: HttpStatusCode.InternalServerError, 
                                         message: "An error occurred while deleting employee");
            }
        }

        public async Task<Responses<bool>> ActiveEmployee(long id)
        {
            try
            {
                // Input validation
                if (id <= 0)
                {
                    return new Responses<bool>(data: false, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Invalid employee ID");
                }

                var status = await employeeRepository.ActiveEmployee(id);
                if (status != null)
                {
                    string message = status ?? false ? "de-activated" : "activated";
                    return new Responses<bool>(data: status ?? false, statusCode: HttpStatusCode.OK, 
                                             message: "Employee has been " + message + " successfully");
                }
                else
                {
                    return new Responses<bool>(data: false, statusCode: HttpStatusCode.BadRequest, 
                                             message: "Failed to update employee status");
                }
            }
            catch (Exception)
            {
                return new Responses<bool>(data: false, statusCode: HttpStatusCode.InternalServerError, 
                                         message: "An error occurred while updating employee status");
            }
        }

        public async Task<bool> IsEmailExist(string email, long id)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return await employeeRepository.IsEmailExist(email, id);
        }
    }
}

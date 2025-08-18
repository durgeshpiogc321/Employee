using Shared.Common;
using Shared.Models;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IEmployeeService
    {
        Task<List<GetAllEmployeeDto>> GetAllEmployee(EmployeeRequestModel model);
        Task<List<GetAllEmployeeDto>> GetAllEmployee_V1(EmployeeRequestModel model);
        Task<EmployeeViewModel> GetEmployee(long id);
        Task<Responses<long>> SaveEmployee(EmployeeViewModel model); 
        Task<Responses<bool>> DeleteEmployee(long id);
        Task<Responses<bool>> ActiveEmployee(long id);
        Task<bool> IsEmailExist(string email, long id);
    }
}

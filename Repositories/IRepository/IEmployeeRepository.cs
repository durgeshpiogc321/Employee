using Repositories.DatabaseModels;
using Repositories.DatabaseModels.ResultSet;
using Shared.Models;

namespace Repositories.IRepository
{
    public interface IEmployeeRepository
    {
        Task<List<GetAllEmployee_Result>> GetAllEmployee(EmployeeRequestModel model);
        Task<List<GetAllEmployee_Result>> GetAllEmployee_V1(EmployeeRequestModel model);
        Task<Employee> GetEmployee(long id);
        Task<long> SaveEmployee(Employee model);
        Task<bool> DeleteEmployee(long id);
        Task<bool?> ActiveEmployee(long id);
        Task<bool> IsEmailExist(string email, long id);
    }
}

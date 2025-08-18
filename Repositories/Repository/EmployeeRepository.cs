using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseModels;
using Repositories.DatabaseModels.ResultSet;
using Repositories.IRepository;
using Shared.Models;
using System;
using System.Globalization;

namespace Repositories.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public async Task<List<GetAllEmployee_Result>> GetAllEmployee(EmployeeRequestModel model)
        {
            SqlParameter[] parms = {
                new SqlParameter("@PageStart", model.PageStart),
                new SqlParameter("@PageSize", model.PageSize),
                new SqlParameter("@SearchKeyword",(model.SearchKeyword??"").Trim()),
                new SqlParameter("@SortColumn", model.SortColumn??""),
                new SqlParameter("@SortOrder", model.SortOrder??""),
                new SqlParameter("@Email", model.Email??""),
                new SqlParameter("@PhoneNumber", model.Phone??""),
                new SqlParameter("@SearchColumn", model.SearchColumn??""),
                new SqlParameter("@SearchColumnValue", model.SearchColumnValue??"")
            };

            string query = "EXEC dbo.GetAllEmployee @PageStart,@PageSize,@SearchKeyword,@SortColumn,@SortOrder,@Email,@PhoneNumber,@SearchColumn,@SearchColumnValue";

            using (var db = new EmployeeDbContext())
            {
                return await db.GetAllEmployee_Results.FromSqlRaw(query, parms).ToListAsync();
            }
        }

        public async Task<List<GetAllEmployee_Result>> GetAllEmployee_V1(EmployeeRequestModel model)
        {
            using (var db = new EmployeeDbContext())
            {
                IQueryable<Employee> query = null;

                query = model.SortColumn switch
                {
                    "Name" => (model.SortOrder == "ASC" ?
                               db.Employees.OrderByDescending(c => c.Name) :
                               db.Employees.OrderBy(c => c.Name)),

                    "Email" => (model.SortOrder == "ASC" ?
                                db.Employees.OrderByDescending(c => c.Email) :
                                db.Employees.OrderBy(c => c.Email)),

                    "Address" => (model.SortOrder == "ASC" ?
                                db.Employees.OrderByDescending(c => c.Address) :
                                db.Employees.OrderBy(c => c.Address)),

                    "PhoneNumber" => (model.SortOrder == "ASC" ?
                               db.Employees.OrderByDescending(c => c.PhoneNumber) :
                               db.Employees.OrderBy(c => c.PhoneNumber)),

                    "Dob" => (model.SortOrder == "ASC" ?
                              db.Employees.OrderByDescending(c => c.Dob) :
                              db.Employees.OrderBy(c => c.Dob)),
                    "CreatedDate" => (model.SortOrder == "ASC" ?
                              db.Employees.OrderByDescending(c => c.CreatedDate) :
                              db.Employees.OrderBy(c => c.Dob)),

                    _ => db.Employees.OrderByDescending(c => c.Id)
                };

                DateTime temp;

                DateTime.TryParseExact("07 Jun, 2023", "dd MMM, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp);

                query = query.Where(a => (string.Concat(a.Name + " ", a.Email + " ", a.Address + " ", a.PhoneNumber + " ") + (a.Dob == null ? "N/A" : "")).Contains((model.SearchKeyword ?? "").Trim()) 
               // || a.Dob.Value.ToString().Contains((temp.ToString()).Trim())
                );





                //query = query.Where(a => a.Dob.Value.ToString("dd MMM, yyyy").Contains((model.SearchKeyword ?? "").Trim())
                //);

                int totalRecords = await query.CountAsync();

                query = query.Skip(model.PageStart).Take(model.PageSize);

                List<Employee> employee = await query.ToListAsync();

                return employee?.Select(a => new GetAllEmployee_Result()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email,
                    Address = a.Address,
                    Dob = a.Dob == null ? "N/A" : a.Dob?.ToString("dd MMM, yyyy"),
                    PhoneNumber = a.PhoneNumber,
                    ProfilePic = a.ProfilePic,
                    CreatedDate = a.CreatedDate == null ? "N/A" : a.CreatedDate?.ToString("dd MMM, yyyy"),
                    IsActive = a.IsActive ?? false,
                    TotalRecords = totalRecords
                }).ToList();
            }
        }

        public async Task<Employee> GetEmployee(long id)
        {
            using (var db = new EmployeeDbContext())
            {
                return await db.Employees.Where(a => a.Id == id && a.IsDeleted == false).FirstOrDefaultAsync();
            }
        }

        public async Task<long> SaveEmployee(Employee model)
        {
            using (var db = new EmployeeDbContext())
            {
                Employee dbEmployee = await db.Employees.FirstOrDefaultAsync(a => a.Id == model.Id && a.IsDeleted == false) ?? new Employee();

                dbEmployee.Name = model.Name;
                dbEmployee.Email = model.Email;
                dbEmployee.Address = model.Address;
                dbEmployee.Dob = model.Dob;
                dbEmployee.PhoneNumber = model.PhoneNumber;
                dbEmployee.ProfilePic = model.ProfilePic;
                dbEmployee.IsActive = model.IsActive;

                if (dbEmployee.Id == 0)
                {
                    await db.Employees.AddAsync(dbEmployee);
                }
                else
                {
                    dbEmployee.UpdatedDate = DateTime.UtcNow;
                }
                db.SaveChanges();

                return dbEmployee.Id;
            }
        }

        public async Task<bool> DeleteEmployee(long id)
        {
            using (var db = new EmployeeDbContext())
            {
                Employee dbEmployee = await db.Employees.FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false) ?? new Employee();

                if (dbEmployee.Id > 0)
                {
                    dbEmployee.UpdatedDate = DateTime.UtcNow;
                    dbEmployee.IsDeleted = true;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool?> ActiveEmployee(long id)
        {
            using (var db = new EmployeeDbContext())
            {
                Employee dbEmployee = await db.Employees.FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false) ?? new Employee();

                if (dbEmployee.Id > 0)
                {
                    dbEmployee.UpdatedDate = DateTime.UtcNow;
                    dbEmployee.IsActive = !(dbEmployee.IsActive ?? false);
                    db.SaveChanges();
                    return !(dbEmployee.IsActive ?? false);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> IsEmailExist(string email, long id)
        {
            using (var db = new EmployeeDbContext())
            {
                Employee dbEmployee = await db.Employees.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower() && a.IsDeleted == false && a.Id != id) ?? new Employee();

                if (dbEmployee.Id > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}

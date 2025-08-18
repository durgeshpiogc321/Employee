using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using Shared.Common;
using Shared.Models;

namespace Employee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetAllEmployee(DataTableParameters param, string email, string phone, string searchKeyword, string searchColumn, string searchColumnValue)
        {
            EmployeeRequestModel model = new EmployeeRequestModel(param, email, phone, searchColumn: searchColumn, searchColumnValue: searchColumnValue);

            if (string.IsNullOrEmpty(model.SearchKeyword))
                model.SearchKeyword = searchKeyword;

            //var items = await employeeService.GetAllEmployee_V1(model);
            var items = await employeeService.GetAllEmployee(model);

            var result = new DataTableResult<GetAllEmployeeDto>
            {
                Draw = param.Draw,
                Data = items,
                RecordsFiltered = items.FirstOrDefault()?.TotalRecords ?? 0,
                RecordsTotal = items.Count()
            };

            return Json(result);
        }


        public async Task<IActionResult> _Create(long id)
        {
            var model = new EmployeeViewModel();
            if (id > 0)
            {
                model = await employeeService.GetEmployee(id);
            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            var result = await employeeService.SaveEmployee(model);
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await employeeService.DeleteEmployee(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Active(long id)
        {
            var result = await employeeService.ActiveEmployee(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> IsEmailExists(string email, long id)
        {
            var IsEmailExist = await employeeService.IsEmailExist(email, id);
            if (IsEmailExist)
                return Json(data: false);
            return Json(data: true);
        }
    }
}

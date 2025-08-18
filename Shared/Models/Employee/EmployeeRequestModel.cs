using Shared.Common;

namespace Shared.Models
{
    public class EmployeeRequestModel: DataTableRequestModel 
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SearchColumn { get; set; }
        public string SearchColumnValue { get; set; }
        public EmployeeRequestModel(DataTableParameters param, string email, string phone, string searchKeyword = "", string searchColumn = "", string searchColumnValue = "") :base(param) 
        {
            Email = email;
            Phone = phone;
            SearchColumn = searchColumn;
            SearchColumnValue = searchColumnValue;
        }
    }
}

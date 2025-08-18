using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DatabaseModels.ResultSet
{
    public class GetAllEmployee_Result
    {
        public long Id { get; set;}
        public string Name { get; set;}
        public string Email { get; set;}
        public string Address { get; set;}
        public string Dob { get; set;}
        public string PhoneNumber { get; set;}
        public string ProfilePic { get; set; }
        public string CreatedDate { get; set; }
        public bool IsActive { get; set; }   
        public int TotalRecords { get; set; }
    }
}

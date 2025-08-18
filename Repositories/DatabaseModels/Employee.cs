using System;
using System.Collections.Generic;

namespace Repositories.DatabaseModels;

public partial class Employee
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public DateTime? Dob { get; set; }

    public string PhoneNumber { get; set; }

    public string ProfilePic { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

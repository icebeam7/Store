using System;
using System.Collections.Generic;

namespace MyStore.Models
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FullName => $"{LastName} {FirstName}";
    }
}

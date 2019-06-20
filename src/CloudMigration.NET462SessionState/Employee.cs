using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigration.NET462SessionState
{
    /// <summary>
    /// Working with .NET objects (NOT serialized objects)
    /// </summary>
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Employee(string EmployeeId, string Name, int Age)
        {
            this.Id = EmployeeId;
            this.Name = Name;
            this.Age = Age;
        }
    }
}

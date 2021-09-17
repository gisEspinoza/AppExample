using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exampleAPP
{
    class Department
    {
        public int deparmentId { get; set; }
        public string deparmentName { get; set; }

        public static List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>
            {
              new Department { deparmentId=2001, deparmentName="Contabilidad"},
              new Department { deparmentId=2002, deparmentName="Recepcion"},
              new Department { deparmentId=2003, deparmentName="Recursos Humanos"},
              new Department { deparmentId=2004, deparmentName="Contrataciones"}
            };

            return departments;
        }
    }
}

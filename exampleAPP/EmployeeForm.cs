using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exampleAPP
{
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            lblCod.Text = Form1.cod; //consultados el valor enviado de Form1 y lo asignamos al label
            employeeData();
        }

        private void employeeData()
        {
            var query =
                (
                    from employee in Employee.GetEmployees()
                    join department in Department.GetDepartments() on
                    employee.deparmentId equals department.deparmentId
                    where employee.employeeId == lblCod.Text.ToString() //filtramos los datos de acuerdo con el codigo enviado de Form1
                    select new
                    {
                        employeeId= employee.employeeId,
                        employeeFullName = employee.firstName + " "+employee.lastName,
                        employeeBirthDate = employee.birthDate,
                        employeeAge= DateTime.Now.Year- employee.birthDate.Year,
                        employeeHireDate= employee.hire_date,
                        employeeEmail = employee.email,
                        employeeDepartment = department.deparmentName
                    }
                );

            //mostrar los datos en los controles
            foreach(var emp in query)
            {
                lblNameEmployee.Text = emp.employeeFullName;
                lblFullname.Text = emp.employeeFullName;
                lblbirthDate.Text = emp.employeeBirthDate.ToString();
                lblAge.Text = emp.employeeAge.ToString();
                lblhireDate.Text = emp.employeeHireDate.ToString();
                lblEmail.Text = emp.employeeEmail;
                lblDepto.Text = emp.employeeDepartment;
            }

            //consultar los titulos del empleado
            var result = Employee.GetEmployees()
                .Where(emp => emp.employeeId == lblCod.Text)
                .SelectMany(empTitles => empTitles.titles); //permite consultar secuencias dentro de una secuencia

            //llenar el ListView
            foreach (var title in result)
                lvTitles.Items.Add(title);
        }
    }
}

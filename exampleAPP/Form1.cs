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
    public partial class Form1 : Form
    {
        //variable global y de ambito publico
        public static string cod = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //llamdo al metodo utilizado para llenar el combobox
            fillCombobox();


            //llamado al metodo para mostrar datos en el DataGridView
            fillDataGridView();
        }

        //metodo para obtener los nombres de los departamentos
        private void fillCombobox()
        {
            List<string> departments = (
                //consulta con LINQ
                from department in Department.GetDepartments()
                select department.deparmentName //seleccionamos el elemento a retornar
                ).ToList();

            //llenar el combobox
            foreach (var depto in departments)
                cboDepartments.Items.Add(depto);
        }

        //metodo para consultar los datos del empleado
        //y el departamento al que pertenece
        //utilizando operadores
        //dentro del datagridview se mostrara:
        //codigo del empleado, nombre completo, fecha de contratacion, nombre del departamento
        private void fillDataGridView()
        {
            //consulta para unir las dos colecciones(employee, deparment)
            var joinData = Employee.GetEmployees()
                .Join                          //operador de union
                (
                    Department.GetDepartments(),
                    //campos coincidentes, o datos coincidentes
                    employee => employee.deparmentId,
                    deparment => deparment.deparmentId,
                    (employee, deparment) => new
                    {
                        //elementos a seleccionar
                        employeeId = employee.employeeId,
                        employeeFullName = employee.firstName + " " + employee.lastName, //concatenamos firstName y lastName
                        employeeHireDate = employee.hire_date.Year, //extraemos el año de fecha de contratacion
                        employeeDeparment = deparment.deparmentName
                    }
                ).ToList();


            //llenar DataGridView
            //agregar columnas
            dgData.Columns.Add("employeeId", "ID EMPLEADO");
            dgData.Columns.Add("employeeFullName", "NOMBRE");
            dgData.Columns.Add("hireDate", "AÑO CONTRATO");
            dgData.Columns.Add("departmentName", "DEPARTAMENTO");

            //agregar filas al DataGridView
            foreach (var data in joinData)
            {
                dgData.Rows.Add(
                    data.employeeId,
                    data.employeeFullName,
                    data.employeeHireDate,
                    data.employeeDeparment
                    );
            }

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            //llamado al metodo para limpiar DataGridView
            clearDataGridView();
            filterbyDepartment(); //llamado al metodo para filtrar los datos por departamento
        }

        //metodo para limpiar el datagridview
        private void clearDataGridView()
        {
            dgData.Columns.Clear(); //limpia las columnas/elimina las columnas
            dgData.Rows.Clear(); //eliminar filas
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            clearDataGridView(); //limpiamos DataGridView y luego cargamos nuevamente los datos
            fillDataGridView(); //llenado de DataGridView
        }

        //metodo para filtrar empleados por departamento
        private void filterbyDepartment()
        {
            var employeebyDepartment = (
                        from employee in Employee.GetEmployees()
                        join department in Department.GetDepartments() on 
                        employee.deparmentId equals department.deparmentId //operador equals, evalua si los elementos son iguales
                        where department.deparmentName == cboDepartments.Text //comparar el nombre del departamento seleccionado por el usaurio
                        orderby department.deparmentName descending //ordena los nombres de z - a, en forma descendente
                        select new
                        { 
                            employeeId = employee.employeeId,
                            employeeFullName = employee.firstName + " " + employee.lastName,
                            employeeHireDate = employee.hire_date.Year,
                            employeeDepartment = department.deparmentName
                        }
                ).ToList();

            //llenado del DataGridView
            //agregar columnas
            dgData.Columns.Add("employeeId", "ID EMPLEADO");
            dgData.Columns.Add("employeeFullName", "NOMBRE COMPLETO");
            dgData.Columns.Add("employeeHireDate", "AÑO DE CONTRATACION");
            dgData.Columns.Add("employeeDepartment", "DEPARTAMENTO");

            //agregar las filas
            foreach (var employee in employeebyDepartment)
            {
                dgData.Rows.Add(
                    employee.employeeId,
                    employee.employeeFullName,
                    employee.employeeHireDate,
                    employee.employeeDepartment
                    );
            }
        }

        private void dgData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //pasamos el codigo del empleado al formulario EmployeeForm
            cod = dgData.CurrentRow.Cells[0].Value.ToString();

            //cargar el formulario EmployeeForm
            EmployeeForm employee = new EmployeeForm();
            employee.Show(); //mostrar formulario EmployeeForm
        }
    }

}

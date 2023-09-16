using MercadoIT.Web.Controllers;
using MercadoIT.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MercadoIT.Web.UnitTest
{
    [TestClass]
    public class EmployeesControllerTest
    {
        private DbContextOptions<NorthwindContext> _options;
        private NorthwindContext _context;
        private EmployeesController _employeesControllerTest;

        [TestInitialize]
        public void Inicializar()
        {
            _options = new DbContextOptionsBuilder<NorthwindContext>()
               .UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=Northwind;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True")
           .Options;

            _context = new NorthwindContext(_options);

            _employeesControllerTest = new EmployeesController(_context);
        }


        /// <summary>
        /// Prueba si 'Create' retorna una View.
        /// </summary>
        [TestMethod]
        public void CreateReturnsView()
        {
            //Arrange

            //Act
            var result = _employeesControllerTest.Create() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Prueba si 'Create' inserta en la base de datos.
        /// </summary>
        [TestMethod]
        public async Task EmployeesCreateCreatesAsync()
        {
            //Arrange
            Employee employeeTest = new Employee() { LastName = "test", FirstName = "test" };
            var expected = _context.Employees.Count() + 1;

            //Act
            await _employeesControllerTest.Create(employeeTest);
            var actual = _context.Employees.Count();

            //Assert
            Assert.AreEqual(expected, actual);

            DeleteTestFromDb();
        }

        /// <summary>
        /// Prueba si 'Delete' devuelve una View
        /// </summary>
        [TestMethod]
        public async Task EmployeesDeleteReturnsViewAsync()
        {
            //Arrange
            Employee employeeTest = new Employee() { LastName = "test", FirstName = "test" };
            await _employeesControllerTest.Create(employeeTest);

            //Act
            var result = _employeesControllerTest.Delete(employeeTest.EmployeeID);

            //Assert
            Assert.IsNotNull(result);

            DeleteTestFromDb();
        }

        /// <summary>
        /// Prueba si 'Delete' elimina.
        /// </summary>
        [TestMethod]
        public async Task EmployeesDeleteDeletesAsync()
        {
            //Arrange
            Employee employeeTest = new Employee() { LastName = "test", FirstName = "test" };
            await _employeesControllerTest.Create(employeeTest);
            int expected = _context.Employees.Count() - 1;

            //Act
            await _employeesControllerTest.DeleteConfirmed(employeeTest.EmployeeID);
            int actual = _context.Employees.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Prueba si 'Details' devuelve una View
        /// </summary>
        [TestMethod]
        public async Task EmployeesDetailsReturnsViewAsync()
        {
            //Arrange
            Employee employeeTest = new Employee() { LastName = "test", FirstName = "test" };
            await _employeesControllerTest.Create(employeeTest);

            //Act
            var result = _employeesControllerTest.Details(employeeTest.EmployeeID);

            //Assert
            Assert.IsNotNull(result);

            DeleteTestFromDb();
        }

        /// <summary>
        /// Prueba si 'Edit' devuelve una View
        /// </summary>
        [TestMethod]
        public async Task EmployeesEditReturnsViewAsync()
        {
            //Arrange
            Employee employeeTest = new Employee() { FirstName = "test", LastName = "test" };
            await _employeesControllerTest.Create(employeeTest);

            //Act
            var result = _employeesControllerTest.Edit(employeeTest.EmployeeID);

            //Assert
            Assert.IsNotNull(result);

            DeleteTestFromDb();
        }

        /// <summary>
        /// Prueba si 'Edit' edita.
        /// </summary>
        [TestMethod]
        public async Task EmployeesEditEditsAsync()
        {
            //Arrange
            Employee employeeTest = new Employee() { LastName = "test1", FirstName = "test1" };
            await _employeesControllerTest.Create(employeeTest);

            var expectedEmployee = _context.Employees.Find(employeeTest.EmployeeID);

            employeeTest.LastName = "test2";
            employeeTest.FirstName = "test2";

            //Act
            await _employeesControllerTest.Edit(employeeTest.EmployeeID);

            _context.Entry(employeeTest).State = EntityState.Modified; 

            var actualEmployee = _context.Employees.Find(employeeTest.EmployeeID);

            //Assert
            Assert.AreEqual(expectedEmployee, actualEmployee);

            DeleteTestFromDb();
        }

        /// <summary>
        /// Prueba si 'Index' devuelve una View
        /// </summary>
        [TestMethod]
        public void EmployeesIndexReturnsView()
        {
            //Arrange

            //Act
            var result = _employeesControllerTest.Index();

            //Assert
            Assert.IsNotNull(result);
        }


        private void DeleteTestFromDb()
        {
            var employees = _context.Employees.Where(e => e.LastName.Contains("test"));
            _context.Employees.RemoveRange(employees);
            _context.SaveChanges();
        }
    }
}

using MercadoIT.Web.Controllers;
using MercadoIT.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace MercadoIT.Web.UnitTest.Controllers
{
    [TestClass()]
    public class EmployeesControllerTestWithMock
    {
        /// <summary>
        /// Esta prueba confirmará que el método devuelve "NotFound" cuando se le da un ID nulo.
        /// </summary>
        [TestMethod]
        public async Task Details_NullId_ReturnsNotFound()
        {
            // Arrange
            var mockContext = new Mock<NorthwindContext>();
            var controller = new EmployeesController(mockContext.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        //[TestMethod]
        public async Task Index_ReturnsViewWithEmployeesList()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployees = new Mock<DbSet<Employee>>();
            mockContext.Setup(ctx => ctx.Employees).Returns(mockEmployees.Object);
            var controller = new EmployeesController(mockContext.Object);

            // Act (Actuar)
            var result = await controller.Index();

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<Employee>));
        }


        //[TestMethod]
        public async Task Details_ValidId_ReturnsEmployeeDetails()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployee = new Employee { EmployeeID = 1 };
            mockContext.Setup(ctx => ctx.Employees.FindAsync(1)).ReturnsAsync(mockEmployee);
            var controller = new EmployeesController(mockContext.Object);

            // Act (Actuar)
            var result = await controller.Details(1);

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual(mockEmployee, viewResult.Model);
        }

        [TestMethod]
        public void Create_Get_ReturnsView()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var controller = new EmployeesController(mockContext.Object);

            // Act (Actuar)
            var result = controller.Create();

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Create_ValidEmployee_RedirectsToIndex()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployees = new Mock<DbSet<Employee>>();
            mockContext.Setup(ctx => ctx.Employees).Returns(mockEmployees.Object);
            var controller = new EmployeesController(mockContext.Object);
            var employee = new Employee();

            // Act (Actuar)
            var result = await controller.Create(employee);

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);


        }

        [TestMethod]
        public async Task Edit_GetValidId_ReturnsViewWithEmployeeDetails()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployee = new Employee { EmployeeID = 1 };
            mockContext.Setup(ctx => ctx.Employees.FindAsync(1)).ReturnsAsync(mockEmployee);
            var controller = new EmployeesController(mockContext.Object);

            // Act (Actuar)
            var result = await controller.Edit(1);

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual(mockEmployee, viewResult.Model);
        }


        //[TestMethod]
        public async Task Edit_ValidEmployee_RedirectsToIndex()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployees = new Mock<DbSet<Employee>>();
            mockContext.Setup(ctx => ctx.Employees).Returns(mockEmployees.Object);
            var controller = new EmployeesController(mockContext.Object);
            var employee = new Employee();

            // Act (Actuar)
            var result = await controller.Edit(1, employee);

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }


        //[TestMethod]
        public async Task Delete_GetValidId_ReturnsViewWithEmployeeDetails()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployee = new Employee { EmployeeID = 1 };
            mockContext.Setup(ctx => ctx.Employees.FindAsync(1)).ReturnsAsync(mockEmployee);

            var controller = new EmployeesController(mockContext.Object);

            // Act (Actuar)
            var result = await controller.Delete(1);

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual(mockEmployee, viewResult.Model);
        }

        [TestMethod]
        public async Task DeleteConfirmed_ValidEmployee_RedirectsToIndex()
        {
            // Arrange (Preparar)
            var mockContext = new Mock<NorthwindContext>();
            var mockEmployee = new Employee { EmployeeID = 1 };
            mockContext.Setup(ctx => ctx.Employees.FindAsync(1)).ReturnsAsync(mockEmployee);
            var controller = new EmployeesController(mockContext.Object);

            // Act (Actuar)
            var result = await controller.DeleteConfirmed(1);

            // Assert (Afirmar)
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }


    }
}

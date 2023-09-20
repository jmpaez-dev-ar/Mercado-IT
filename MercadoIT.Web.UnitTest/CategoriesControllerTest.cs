using MercadoIT.Web.Controllers;
using MercadoIT.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace MercadoIT.Web.UnitTest
{
    [TestClass]
    public class CategoriesControllerTest
    {
        private NorthwindContext _context;
        private CategoriesController _categoriesControllerTest;

        [TestInitialize]
        public void Inicializar()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=Northwind;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True")
            .Options;

            _context = new NorthwindContext(options);
            _categoriesControllerTest = new CategoriesController(_context);
        }

        /// <summary>
        /// Verifica qué devuelve la acción Index.
        /// </summary>
        [TestMethod]
        public void CategoryReturnsIndexView()
        {
            //Arrage

            //Act
            var result = _categoriesControllerTest.Index();

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Verifica qué retorna la acción Index.
        /// </summary>
        [TestMethod]
        public void CategoryReturnsIndexViewResult()
        {
            //Arrage

            //Act
            var result = _categoriesControllerTest.Index();

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Verifica qué retorna la acción Details.
        /// </summary>
        [TestMethod]
        public async Task CategoryReturnsDetails()
        {
            //Arrage
            Category categoryTest = new Category() { CategoryID = 1, CategoryName = "Confections", Description = "Desserts, candies, and sweet breads" };
            
            //Act
            var result = await _categoriesControllerTest.Details(categoryTest.CategoryID);

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Prueba si 'create' devuelve una vista.
        /// </summary>
        [TestMethod]
        public void CreateReturnsViewCategory()
        {
            //Arrange

            //Act
            var result = _categoriesControllerTest.Create();

            //Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Prueba si 'create' inserta en la base de datos.
        /// </summary>
        [TestMethod]
        public async Task CategoryCreate()
        {
            //Arrange
            Category categoryTest = new Category() { CategoryName = "Prueba", Description = "Es una Prueba" };

            //Act
            var expected = _context.Categories.Count() + 1;
            await _categoriesControllerTest.Create(categoryTest);
            var actual = _context.Categories.Count();
            var category = _context.Categories.Where(c => c.CategoryName == categoryTest.CategoryName);

            //Assert
            Assert.AreEqual(expected, actual);

            _context.Categories.RemoveRange(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// Prueba si 'delete' retorna una vista.
        /// </summary>
        [TestMethod]
        public async Task CategoryDeleteReturnsView()
        {
            //Arrange
            Category categoryTest = new Category() { CategoryName = "Prueba", Description = "Es una Prueba" };
            await _categoriesControllerTest.Create(categoryTest);

            //Act
            var result = _categoriesControllerTest.Delete(categoryTest.CategoryID);

            //Assert
            Assert.IsNotNull(result);

            var category = _context.Categories.Where(c => c.CategoryName == categoryTest.CategoryName);
            _context.Categories.RemoveRange(category);
            _context.SaveChanges();
        }

        /// <summary>
        /// "Prueba si 'delete' elimina."
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async System.Threading.Tasks.Task CategoryDeleteDeletes()
        {
            //Arrange
            Category categoryTest = new Category() { CategoryName = "Prueba", Description = "Es una Prueba" };
            await _categoriesControllerTest.Create(categoryTest);
            int expected = _context.Categories.Count() - 1;

            //Act
            await _categoriesControllerTest.DeleteConfirmed(categoryTest.CategoryID);
            int actual = _context.Categories.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Prueba si 'edit' funciona.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CategoryEditEditsWithoutClosingContext()
        {
            //Arrange
            Category categoryTest = new Category() { CategoryName = "Prueba1", Description = "Es una Prueba" };
            await _categoriesControllerTest.Create(categoryTest);
            _context.Entry(categoryTest).State = EntityState.Added;

            var expectedCategory = _context.Categories.Find(categoryTest.CategoryID);

            // Simulate user edit without disposing the context
            expectedCategory.CategoryName = "Prueba2";

            //Act
            await _categoriesControllerTest.Edit(categoryTest.CategoryID);
            _context.Entry(expectedCategory).State = EntityState.Modified;
            _context.SaveChanges(); // Save the changes to the database

            var actualCategory = _context.Categories.Find(categoryTest.CategoryID);

            //Assert
            Assert.AreEqual(expectedCategory, actualCategory);

            var category = _context.Categories.Where(c => (c.CategoryName == "Prueba2") || (c.CategoryName == "Prueba1"));
            _context.Categories.RemoveRange(category);
            _context.SaveChanges();
        }


        // [TestMethod]
        /// <summary>
        /// Prueba si 'edit' funciona. 
        ///  TODO: No funciona. Revisar.
        /// </summary>
        /// <returns></returns>
        public async Task CategoriesEditEditsAsync()
        {
            //Arrange
            Category categoryTest = new Category() { CategoryName = "test1", Description = "test1" };
            await _categoriesControllerTest.Create(categoryTest);

            var expectedCategory = _context.Categories.Find(categoryTest.CategoryID);

            categoryTest.CategoryName = "test2";
            categoryTest.Description = "test2";

            //Act
            await _categoriesControllerTest.Edit(categoryTest.CategoryID);

            _context.Entry(categoryTest).State = EntityState.Modified;

            var actualCategory = _context.Categories.Find(categoryTest.CategoryID);

            //Assert
            Assert.AreEqual(expectedCategory, actualCategory);

            DeleteTestFromDb();
        }

        private void DeleteTestFromDb()
        {
            var categories = _context.Categories.Where(e => e.CategoryName.Contains("test"));
            _context.Categories.RemoveRange(categories);
            _context.SaveChanges();
        }

    }
}

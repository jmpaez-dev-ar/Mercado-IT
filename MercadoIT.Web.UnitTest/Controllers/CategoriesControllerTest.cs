using MercadoIT.Web.Controllers;
using MercadoIT.Web.DataAccess.Interfaces;
using MercadoIT.Web.DataAccess.Services;
using MercadoIT.Web.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Protocol.Core.Types;

namespace MercadoIT.Web.UnitTest.Controllers
{
	[TestClass]
	public class CategoriesControllerTest
	{
		protected IRepositoryAsync<Category> _repository;
		private CategoriesController _categoriesControllerTest;

		[TestInitialize]
		public void Inicializar()
		{
			var options = new DbContextOptionsBuilder<NorthwindContext>()
				.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=Northwind;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True")
			.Options;

			_repository = new RepositoryAsync<Category>(new NorthwindContext(options));
			_categoriesControllerTest = new CategoriesController(_repository);
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

			// Assert (Afirmar)
			//Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		/// <summary>
		/// Verifica qué retorna la acción Index.
		/// </summary>
		[TestMethod]
		public async Task CategoryReturnsIndexViewResult()
		{
			// Arrage (Preparar)

			// Act (Actuar)
			var result = await _categoriesControllerTest.Index();
			var viewResult = result as ViewResult; 

			// Assert (Afirmar)
			Assert.IsNotNull(viewResult);
			Assert.IsInstanceOfType(viewResult, typeof(ViewResult));
		}

		/// <summary>
		/// Verifica qué retorna la acción Index.
		/// </summary>
		[TestMethod]
		public async Task CategoryReturnsIndexViewResultWithListCategories()
		{
			// Arrage (Preparar)

			// Act (Actuar)
			var result = await _categoriesControllerTest.Index();
			var viewResult = result as ViewResult; 

			// Assert (Afirmar)
			Assert.IsNotNull(viewResult);
			Assert.IsInstanceOfType(viewResult, typeof(ViewResult));
			Assert.IsInstanceOfType(viewResult.Model, typeof(List<Category>));
		}


		/// <summary>
		/// Verifica qué retorna la acción Details.
		/// </summary>
		[TestMethod]
		public async Task CategoryReturnsDetails()
		{
			//Arrage
			Category categoryTest = new Category()
			{ CategoryID = 1, CategoryName = "Confections", Description = "Desserts, candies, and sweet breads" };

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
			var expected = await _repository.CountAsync() + 1;
			await _categoriesControllerTest.Create(categoryTest);
			var actual = await _repository.CountAsync();
			var category = await _repository.Find(c => c.CategoryName == categoryTest.CategoryName);

			//Assert
			Assert.AreEqual(expected, actual);

			await _repository.Delete(category);
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

			var category = await _repository.Find(c => c.CategoryName == categoryTest.CategoryName);
			await _repository.Delete(category);
		}

		/// <summary>
		/// "Prueba si 'delete' elimina."
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task CategoryDeleteDeletes()
		{
			//Arrange
			Category categoryTest = new Category() { CategoryName = "Prueba", Description = "Es una Prueba" };
			await _categoriesControllerTest.Create(categoryTest);
			var expected = await _repository.CountAsync() - 1;

			//Act
			await _categoriesControllerTest.DeleteConfirmed(categoryTest.CategoryID);
			int actual = await _repository.CountAsync();

			//Assert
			Assert.AreEqual(expected, actual);
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

			var expectedCategory = await _repository.GetById(categoryTest.CategoryID);
			categoryTest.CategoryName = "test2";
			categoryTest.Description = "test2";

			//Act
			await _categoriesControllerTest.Edit(categoryTest.CategoryID);

			var actualCategory = await _repository.GetById(categoryTest.CategoryID);

			//Assert
			Assert.AreEqual(expectedCategory, actualCategory);

			DeleteTestFromDbAsync();
		}

		private async Task DeleteTestFromDbAsync()
		{
			var categories = await _repository.Get(c => c.CategoryName == "test1" || c.CategoryName == "test2");
			foreach (var category in categories)
			{
				_repository.Delete(category);
			}
		}
	}
}

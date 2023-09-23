using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;
using System.Linq.Expressions;

namespace MercadoIT.Web.Controllers
{
    public class ProductsController : Controller
	{
		protected readonly IRepositoryAsync<Product> _repositoryProducts;
		protected readonly IRepositoryAsync<Category> _repositoryCategories;
		protected readonly IRepositoryAsync<Supplier> _repositorySuppliers;

		public ProductsController(IRepositoryAsync<Product> repositoryProducts, IRepositoryAsync<Category> repositoryCategories, IRepositoryAsync<Supplier> repositorySuppliers)
		{
			_repositoryProducts = repositoryProducts;
			_repositoryCategories = repositoryCategories;
			_repositorySuppliers = repositorySuppliers;
		}

		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var totalItems = await _repositoryProducts.CountAsync();

			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			var products = await _repositoryProducts.GetPaged(
				orderBy: q => q.OrderBy(p => p.ProductID),
				skip: (page - 1) * pageSize,
				take: pageSize,
				includes: new Expression<Func<Product, object>>[]
					{
						p => p.Category,
						p => p.Supplier
					}
				);

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;
			ViewBag.PageSize = pageSize;

			return View(products);
		}

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _repositoryProducts.GetById(id, includes: new Expression<Func<Product, object>>[]
					{
						p => p.Category,
						p => p.Supplier
					});

			return View(product);
		}

		// GET: Products/Create
		public async Task<IActionResult> Create()
		{
			var lstCategories = await _repositoryCategories.GetAll();
			var lstSuppliers = await _repositorySuppliers.GetAll();

			ViewData["CategoryID"] = new SelectList(lstCategories, "CategoryID", "CategoryID");
			ViewData["SupplierID"] = new SelectList(lstSuppliers, "SupplierID", "SupplierID");

			return View();
		}

		// POST: Products/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
		{
			if (ModelState.IsValid)
			{
				await _repositoryProducts.Insert(product);
				return RedirectToAction(nameof(Index));
			}
			var lstCategories = await _repositoryCategories.GetAll();
			var lstSuppliers = await _repositorySuppliers.GetAll();

			ViewData["CategoryID"] = new SelectList(lstCategories, "CategoryID", "CategoryID");
			ViewData["SupplierID"] = new SelectList(lstSuppliers, "SupplierID", "SupplierID");

			return View(product);
		}

		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _repositoryProducts.GetById(id);
			if (product == null)
			{
				return NotFound();
			}
			var lstCategories = await _repositoryCategories.GetAll();
			var lstSuppliers = await _repositorySuppliers.GetAll();

            ViewBag.CategoryID = new SelectList(lstCategories, "CategoryID", "CategoryName");
			ViewData["SupplierID"] = new SelectList(lstSuppliers, "SupplierID", "CompanyName");

			return View(product);
		}

		// POST: Products/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
		{
			if (id != product.ProductID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_repositoryProducts.Update(product);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await ProductExistsAsync(product.ProductID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			var lstCategories = _repositoryCategories.GetAll().Result.ToList();
			var lstSuppliers = _repositorySuppliers.GetAll().Result.ToList();

			ViewData["CategoryID"] = new SelectList(lstCategories, "CategoryID", "CategoryID");
			ViewData["SupplierID"] = new SelectList(lstSuppliers, "SupplierID", "SupplierID");
			return View(product);
		}

		// GET: Products/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _repositoryProducts.GetById(id, includes: new Expression<Func<Product, object>>[]
						{
						p => p.Category,
						p => p.Supplier
						});
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (!await ProductExistsAsync(id))
			{
				return NotFound();
			}
			await _repositoryProducts.Delete(id);
			return RedirectToAction(nameof(Index));
		}

		private async Task<bool> ProductExistsAsync(int? id)
		{
			bool exists = false;
			if (id != null)
			{
				exists = await _repositoryProducts.ExistById(id);
			}
			return exists;
		}
	}
}

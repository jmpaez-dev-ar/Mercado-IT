using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;

namespace MercadoIT.Web.Controllers
{
	public class CategoriesController : Controller
	{
		protected readonly IRepositoryAsync<Category> _repository;

		public CategoriesController(IRepositoryAsync<Category> repository)
		{
			_repository = repository;
		}

		// GET: Categories
		public async Task<IActionResult> Index()
		{
			var categories = await _repository.GetAll();
			return View(categories);
		}

		// GET: Categories/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _repository.GetById(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// GET: Categories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Categories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,Description,Picture")] Category category)
		{
			if (ModelState.IsValid)
			{
				await _repository.Insert(category);
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		// GET: Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _repository.GetById(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Categories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName,Description,Picture")] Category category)
		{
			if (id != category.CategoryID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					await _repository.Update(category);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await CategoryExistsAsync(category.CategoryID))
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
			return View(category);
		}

		// GET: Categories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _repository.GetById(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (!await CategoryExistsAsync(id))
			{
				return NotFound();
			}
			await _repository.Delete(id);
			return RedirectToAction(nameof(Index));
		}

		private async Task<bool> CategoryExistsAsync(int? id)
		{
			bool exists = false;
			if (id != null)
			{
				exists = await _repository.ExistById(id);
			}
			return exists;
		}
	}
}

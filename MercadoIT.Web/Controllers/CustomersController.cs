using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;

namespace MercadoIT.Web.Controllers
{
	public class CustomersController : Controller
	{
		protected readonly IRepositoryAsync<Customer> _repository;


		public CustomersController(IRepositoryAsync<Customer> repository)
		{
			_repository = repository;
		}

		// GET: Customers
		public async Task<IActionResult> Index()
		{
			var customers = await _repository.GetAll();
			return View(customers);
		}

		// GET: Customers/Details/5
		public async Task<IActionResult> Details(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _repository.GetById(id);
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		// GET: Customers/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Customers/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
		{
			if (ModelState.IsValid)
			{
				await _repository.Insert(customer);
				return RedirectToAction(nameof(Index));
			}
			return View(customer);
		}

		// GET: Customers/Edit/5
		public async Task<IActionResult> Edit(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _repository.GetById(id);
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		// POST: Customers/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
		{
			if (id != customer.CustomerID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_repository.Update(customer);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await CustomerExistsAsync(customer.CustomerID))
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
			return View(customer);
		}

		// GET: Customers/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var customer = await _repository.GetById(id);
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		// POST: Customers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (!await CustomerExistsAsync(id))
			{
				return NotFound();
			}
			await _repository.Delete(id);
			return RedirectToAction(nameof(Index));
		}

		private async Task<bool> CustomerExistsAsync(string? id)
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

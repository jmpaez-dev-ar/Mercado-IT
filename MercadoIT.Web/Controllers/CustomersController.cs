using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;
using System.Linq.Expressions;
using ClosedXML.Excel;
using System.Linq;

namespace MercadoIT.Web.Controllers
{
	public class CustomersController : Controller
	{
		protected readonly IRepositoryAsync<Customer> _repository;
		private readonly ILogger<CustomersController> _logger;

		public CustomersController(ILogger<CustomersController> logger, IRepositoryAsync<Customer> repository)
		{
			_logger = logger;
			_repository = repository;
		}

		// GET: Customers
		//public async Task<IActionResult> Index()
		//{
		//	var customers = await _repository.GetAll();
		//	return View(customers);
		//}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			_logger.LogInformation("Index");
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetData(int start = 0, int length = 10, string orderBy = "Id", bool ascending = true, string search = "")
		{
			// Aplicar filtro de búsqueda
			Expression<Func<Customer, bool>> filter = x =>
				(string.IsNullOrEmpty(search) || x.CompanyName.Contains(search) || x.CustomerID.Contains(search));

			// Obtener datos paginados y filtrados
			var pagedData = await _repository.GetPagedDataAsync(filter, start, length, orderBy, ascending);

			// Convertir a formato DataTables
			var result = new
			{
				draw = HttpContext.Request.Query["draw"].FirstOrDefault(),
				recordsFiltered = pagedData.TotalRecords,
				recordsTotal = pagedData.TotalRecords,
				data = pagedData.Data
			};

			return Json(result);
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



		[HttpGet]
		public async Task<IActionResult> ExportToExcel()
		{
			var customers = await _repository.GetAll();

			using (var workbook = new XLWorkbook())
			{
				var worksheet = workbook.Worksheets.Add("Customers");

				worksheet.Columns().AdjustToContents();

				var currentRow = 1;
				worksheet.Cell(currentRow, 1).Value = "CustomerID";
				worksheet.Cell(currentRow, 2).Value = "CompanyName";
				worksheet.Cell(currentRow, 3).Value = "ContactName";


				foreach (var customer in customers)
				{
					currentRow++;
					worksheet.Cell(currentRow, 1).Value = customer.CustomerID;
					worksheet.Cell(currentRow, 2).Value = customer.CompanyName;
					worksheet.Cell(currentRow, 3).Value = customer.ContactName;
				}


				// Calcular el rango dinámicamente
				var range = worksheet.Range(
					worksheet.Cell(1, 1),
					worksheet.Cell(customers.Count() + 1 , 3)  // +1 porque la primera fila son los encabezados
				);

				// Aplicar el estilo de borde al rango calculado
				range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
				range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					var content = stream.ToArray();
					return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
				}
			}
		}

	}
}

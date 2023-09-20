using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;

namespace MercadoIT.Web.Controllers
{
	public class EmployeesController : Controller
    {
        protected readonly IRepositoryAsync<Employee> _repository;

        public EmployeesController(IRepositoryAsync<Employee> repository)
        {
            _repository = repository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _repository.GetAll();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            
            var employee = await _repository.GetById(id);
            if (employee == null)
            {
				return NotFound();
			}
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,PhotoPath")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _repository.Insert(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var employee = await _repository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,PhotoPath")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					await _repository.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EmployeeExistsAsync(employee.EmployeeID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var employee = await _repository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			if (!await EmployeeExistsAsync(id))
			{
				return NotFound();
			}
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmployeeExistsAsync(int? id)
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

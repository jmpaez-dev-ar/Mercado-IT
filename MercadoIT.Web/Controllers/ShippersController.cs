using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;

namespace MercadoIT.Web.Controllers
{
    public class ShippersController : Controller
    {
        protected readonly IRepositoryAsync<Shipper> _repository;

        public ShippersController(IRepositoryAsync<Shipper> repository)
        {
            _repository = repository;
        }

        // GET: Shippers
        public async Task<IActionResult> Index()
        {
            var shippers = await _repository.GetAll();
            return View(shippers);
        }

        // GET: Shippers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipper = await _repository.GetById(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        // GET: Shippers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shippers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipperID,CompanyName,Phone")] Shipper shipper)
        {
            if (ModelState.IsValid)
            {
                await _repository.Insert(shipper);
                return RedirectToAction(nameof(Index));
            }
            return View(shipper);
        }

        // GET: Shippers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipper = await _repository.GetById(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        // POST: Shippers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipperID,CompanyName,Phone")] Shipper shipper)
        {
            if (id != shipper.ShipperID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(shipper);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ShipperExistsAsync(shipper.ShipperID))
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
            return View(shipper);
        }

        // GET: Shippers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipper = await _repository.GetById(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        // POST: Shippers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await ShipperExistsAsync(id))
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ShipperExistsAsync(int? id)
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

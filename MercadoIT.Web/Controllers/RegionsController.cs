using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;

namespace MercadoIT.Web.Controllers
{
    public class RegionsController : Controller
    {
        protected readonly IRepositoryAsync<Region> _repository;

        public RegionsController(IRepositoryAsync<Region> repository)
        {
            _repository = repository;
        }

        // GET: Regions
        public async Task<IActionResult> Index()
        {
            var regions = await _repository.GetAll();
            return View(regions);
        }

        // GET: Regions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var region = await _repository.GetById(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        // GET: Regions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegionID,RegionDescription")] Region region)
        {
            if (ModelState.IsValid)
            {
                await _repository.Insert(region);
                return RedirectToAction(nameof(Index));
            }
            return View(region);
        }

        // GET: Regions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _repository.GetById(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        // POST: Regions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegionID,RegionDescription")] Region region)
        {
            if (id != region.RegionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(region);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RegionExistsAsync(region.RegionID))
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
            return View(region);
        }

        // GET: Regions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _repository.GetById(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await RegionExistsAsync(id))
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RegionExistsAsync(int? id)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;
using System.Collections;

namespace MercadoIT.Web.Controllers
{
	public class TerritoriesController : Controller
	{
		private readonly IRepositoryAsync<Territory> _repositoryTerritories;
		private readonly IRepositoryAsync<Region> _repositoryRegions;

		public TerritoriesController(IRepositoryAsync<Territory> repositoryTerritories, IRepositoryAsync<Region> repositoryRegions)
		{
			_repositoryTerritories = repositoryTerritories;
			_repositoryRegions = repositoryRegions;
		}

		// GET: Territories
		public async Task<IActionResult> Index()
		{
			var territories = await _repositoryTerritories.GetAll();
			return View(territories);
		}

		// GET: Territories/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var territory = await _repositoryTerritories.GetById(id);
			if (territory == null)
			{
				return NotFound();
			}
			return View(territory);
		}

		// GET: Territories/Create
		public async Task<IActionResult> Create()
		{
			var regions = await _repositoryRegions.GetAll();
			ViewData["RegionID"] = new SelectList(regions, "RegionID", "RegionID");
			return View();
		}

		// POST: Territories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("TerritoryID,TerritoryDescription,RegionID")] Territory territory)
		{
			if (ModelState.IsValid)
			{
				await _repositoryTerritories.Insert(territory);
				return RedirectToAction(nameof(Index));
			}
			var regions = (IEnumerable)_repositoryRegions.GetAll();
			ViewData["RegionID"] = new SelectList(regions, "RegionID", "RegionID");

			return View(territory);
		}

		// GET: Territories/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var territory = await _repositoryTerritories.GetById(id);
			if (territory == null)
			{
				return NotFound();
			}

			var regions = (IEnumerable)_repositoryRegions.GetAll();
			ViewData["RegionID"] = new SelectList(regions, "RegionID", "RegionID");

			return View(territory);
		}

		// POST: Territories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("TerritoryID,TerritoryDescription,RegionID")] Territory territory)
		{
			if (id != territory.TerritoryID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					await _repositoryTerritories.Update(territory);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await TerritoryExistsAsync(territory.TerritoryID))
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
			var regions = (IEnumerable)_repositoryRegions.GetAll();
			ViewData["RegionID"] = new SelectList(regions, "RegionID", "RegionID");
			return View(territory);
		}

		// GET: Territories/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var territory = await _repositoryTerritories.GetById(id);
			if (territory == null)
			{
				return NotFound();
			}
			return View(territory);
		}

		// POST: Territories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (!await TerritoryExistsAsync(id))
			{
				return NotFound();
			}
			await _repositoryTerritories.Delete(id);
			return RedirectToAction(nameof(Index));
		}

		private async Task<bool> TerritoryExistsAsync(string id)
		{
			bool exists = false;
			if (id != null)
			{
				exists = await _repositoryTerritories.ExistById(id);
			}
			return exists;
		}
	}
}

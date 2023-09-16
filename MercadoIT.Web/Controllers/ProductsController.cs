﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MercadoIT.Web.Entities;
using MercadoIT.Web.DataAccess.Interfaces;
using NuGet.Protocol.Core.Types;
using System.Linq.Expressions;
using MercadoIT.Web.DataAccess.Services;

namespace MercadoIT.Web.Controllers
{
    public class ProductsController : Controller
    {
        protected readonly IRepositoryAsync<Product> _repository;

        protected readonly IRepositoryAsync<Category> _repositoryCategories;
        protected readonly IRepositoryAsync<Supplier> _repositorySuppliers;

        public ProductsController(IRepositoryAsync<Product> repository, IRepositoryAsync<Category> repositoryCategories, IRepositoryAsync<Supplier> repositorySuppliers)
        {
            _repository = repository;
            _repositoryCategories = repositoryCategories;
            _repositorySuppliers = repositorySuppliers;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAll(includes: new Expression<Func<Product, object>>[]
                    {
                        p => p.Category,
                        p => p.Supplier
                    });
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var product = await _repository.GetByID(id, includes: new Expression<Func<Product, object>>[]
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _repository.Insert(product);
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
            if (id == null )
            {
                return NotFound();
            }

            var product = await _repository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            var lstCategories = _repositoryCategories.GetAll().Result.ToList();
            var lstSuppliers = _repositorySuppliers.GetAll().Result.ToList();

            ViewData["CategoryID"] = new SelectList(lstCategories, "CategoryID", "CategoryID");
            ViewData["SupplierID"] = new SelectList(lstSuppliers, "SupplierID", "SupplierID");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _repository.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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

            var product = await _repository.GetByID(id, includes: new Expression<Func<Product, object>>[]
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
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }

            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int? id)
        {
            bool exists = false;
            if (id != null)
            {
                var entity = _repository.GetByID(id);
                exists = entity != null;
            }
            return exists;
        }

    }
}

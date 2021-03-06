﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaUTH.Models;

namespace SistemaUTH.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly SistemaUTHContext _context;

        public EstudiantesController(SistemaUTHContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;//obtiene la obicacion actual;

            ViewData["MatriculaSortParm"] = String.IsNullOrEmpty(sortOrder) ? "matricula_desc" : "";
            ViewData["Ap1SortParm"] = sortOrder == "ap1_asc" ? "ap1_desc" : "ap1_asc";
            ViewData["CurrentFilter"] = searchString; // obtiene el valor a buscar en el input

            if (searchString != null)
            {
                page = 1;
            }//validacion de la busqueda muestra, si hay resultado o noes lo que muestra.
            else
            {
                searchString = currentFilter;
            }
            var estudiantes = from s in _context.Estudiante select s; //un select referente a una query

            if (!String.IsNullOrEmpty(searchString))//verificar si la var Serchstring tiene nombre o descripcio
            {
                estudiantes = estudiantes.Where(s => s.Matricula.Contains(searchString) || s.Ap_paterno.Contains(searchString));
            }
            switch (sortOrder) // ordena lascategorias
            {
                case "matricula_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.Matricula);
                    break;
                case "ap1_asc":
                    estudiantes = estudiantes.OrderBy(s => s.Ap_paterno);
                    break;
                case "ap1_desc":
                    estudiantes = estudiantes.OrderByDescending(s => s.Ap_paterno);
                    break;
                default:
                    estudiantes = estudiantes.OrderBy(s => s.Matricula);
                    break;
            }
            //return View(await _context.Categoria.ToListAsync());
            // regresa la ista con el ordenamiento realizado a la coleccion Categorias
            //return View(await categorias.AsNoTracking().ToListAsync());

            int pageSize = 5;//visualisa el nuemero de elementos que muestra una vista
            return View(await Paginacion<Estudiante>.CreatesAsync(estudiantes.AsNoTracking(), page ?? 1, pageSize));//regresa el total de resultado de elemento.
            //return View(await _context.Estudiante.ToListAsync());
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante
                .FirstOrDefaultAsync(m => m.EstudianteID == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstudianteID,Name,Matricula,Ap_paterno,Ap_materno")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstudianteID,Name,Matricula,Ap_paterno,Ap_materno")] Estudiante estudiante)
        {
            if (id != estudiante.EstudianteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.EstudianteID))
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
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiante
                .FirstOrDefaultAsync(m => m.EstudianteID == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiante.FindAsync(id);
            _context.Estudiante.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiante.Any(e => e.EstudianteID == id);
        }
    }
}

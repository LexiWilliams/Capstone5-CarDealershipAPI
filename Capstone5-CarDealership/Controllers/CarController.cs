﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone5_CarDealership.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone5_CarDealership.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly CarDbContext _context;
        public CarController(CarDbContext context)
        {
            _context = context;
            if (_context.Cars.Count() == 0)
            {
                _context.Cars.Add(new Cars { Make = "Jeep", Model = "Grand Cherokee", Year = 1995, Color = "Blue" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cars>>> GetCars()
        {
            var carList = await _context.Cars.ToListAsync();
            return carList;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Cars>> GetCarById(int id)
        {
            var found = await _context.Cars.FindAsync(id);
            if (found == null)
            {
                return NotFound();
            }
            return found;
        }
        [HttpPost]
        public async Task<ActionResult<Cars>> PostCar(Cars car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Cars>> PutCar(int id, Cars car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cars>> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
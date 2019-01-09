using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{

    [Route("")]
    [ApiController]

    
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id:int}", Name = "GetById")]

        //IActionResult return type with the parameters of id. 
        public IActionResult GetById (int id)
        {
            //Sets variable to search for Id's...
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();
            // Sets the satellites property to any Celestial Object id to the OrbitedObject Id. (converts it to a list).
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();

            // Returns the value of the matching Id parameter. 
            return Ok(celestialObject);
            
        }

        [HttpGet("{name}")]
        //IActionResult return type with the parameters of name.
        public IActionResult GetByName(string name)
        {
            //Creates variable to search for name...
            var celestialObjects= _context.CelestialObjects.Where(e => e.Name == name).ToList();

            // Returns not found when there is no property name that matches the parameter...
            if (celestialObjects.Any())
                return NotFound();
            foreach (var celestialObject in celestialObjects)
            {
                // Searches for matching id's.
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
               
            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //Returns all Celestial Objects...by Id.
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
        }
}

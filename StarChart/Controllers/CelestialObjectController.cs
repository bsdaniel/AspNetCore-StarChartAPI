using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
        public IActionResult GetById(int id)
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
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();

            // Returns not found when there is no property name that matches the parameter...
            if (!celestialObjects.Any())
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


        [HttpPost]

        // Return type that accepts parameter [FromBody]CelestialObject...
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            //Adds to data to the database...
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            //Adds new object with a new Id to the database...
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        // Passes the parameters
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject
        {
            //Searches for an existing object by id (Find)...
            var existingObject = _context.CelestialObjects.Find(id);
            if (existingObject == null)
                return NotFound();

            // If found then the name, period, and id are all updated...
            existingObject.Name = celestialObject.Name;
            existingObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existingObject.Id = celestialObject.Id;

            // Updates and saves the changes made...
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }

        // Specifies the which areas that are needed to be fixed...
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            // Searches for existing object by id (Find)...
            var existingObject = _context.CelestialObjects.Find(id);
            if (existingObject == null)
                return NotFound();

            // Only updates the name...(as specified in the parameters)...
            existingObject.Name = name;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();


        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Gets entire Celestial Object listings who's id's matches the parameters in the method...
            var celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);

            // Looks if there are any matches in the database...
            if (!celestialObjects.Any())
                return NotFound();

            //Deletes that entry and saves the changes...
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }

    }
}

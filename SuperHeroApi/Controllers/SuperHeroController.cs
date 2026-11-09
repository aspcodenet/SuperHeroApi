// RAST TILL 11:30 - TILL KL 12 - lägga på databas
// kl 13:00 TESTA APIer på riktigt!!!

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperHeroApi.Data;
using SuperHeroApi.Models;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]  //prefix för URL  - "api/superhero" - HTTP GET (i webläsaren surfar)
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        public SuperHeroController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //private static readonly List<SuperHero> heroes = new List<SuperHero>
        //{
        //    new SuperHero
        //    {
        //        Id = 1,
        //        Name = "Spiderman",
        //        FirstName = "Peter",
        //        SurName = "Parker",
        //        City = "New York"
        //    },
        //    new SuperHero
        //    {
        //        Id = 2,
        //        Name = "Ironman",
        //        FirstName = "Tony",
        //        SurName = "Stark",
        //        City = "New York"
        //    }
        //};
        private readonly ApplicationDbContext dbContext;

        // LISTA ALLA HEROS
        [HttpGet] // api/superhero
        public async Task<ActionResult<List<SuperHero>>> GetAll()
        {
            return Ok(dbContext.SuperHeroes); // listan returneras som JSON och statuskod 200 OK
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SuperHero>> Delete(int id)
        {
            var hero = dbContext.SuperHeroes.FirstOrDefault(h => h.Id == id);
            if (hero == null)
            {
                return NotFound(); // 404
            }
            dbContext.SuperHeroes.Remove(hero);
            dbContext.SaveChanges();
            //heroes.Remove(hero); // HARD DELETE - tar bort från listan
            return NoContent(); // 204 No Content
        }



            [HttpPut("{id}")]
        public async Task<ActionResult<SuperHero>> UpdateHero(int id, UpdateSuperHero updateHero)
        {
            var hero = dbContext.SuperHeroes.FirstOrDefault(h => h.Id == id);
            if (hero == null)
            {
                return NotFound(); // 404
            }

            hero.Name = updateHero.Name;
            hero.FirstName = updateHero.FirstName;
            hero.SurName = updateHero.SurName;
            hero.City = updateHero.City;

            dbContext.SaveChanges();

            return Ok(hero);
        }

        [HttpPost] // /api/superhero
        public async Task<ActionResult<SuperHero>> CreateNew(NewSuperHero newHero) // Skapa en ny - och den nya ska returneras
        {
            var hero = new SuperHero
            {
//                Id = heroes.Max(h => h.Id) + 1, // auto increment sötts från databasen
                Name = newHero.Name,
                FirstName = newHero.FirstName,
                SurName = newHero.SurName,
                City = newHero.City
            };
            dbContext.SuperHeroes.Add(hero);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetOne), new { id = hero.Id }, hero); // 201 Created
        }


        [HttpGet("{id:int}")] // api/superhero/2 
        public async Task<ActionResult<SuperHero>> GetOne(int id)
        {
            var hero = dbContext.SuperHeroes.FirstOrDefault(h => h.Id == id);
            if(hero == null)
            {
                // returnera felkod 404
                return NotFound();
            }
            return Ok(hero); // 200
        }
    }
}

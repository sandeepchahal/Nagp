using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/brand")]
public class BrandController(IMongoCollection<BrandDb> brandCollection):ControllerBase
{
    // GET: api/brand
    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<BrandView>>> GetAll()
    {
        var brands = await brandCollection.Find(_ => true)
            .Project(b => new BrandView { Id = b.Id, Name = b.Name })
            .ToListAsync();
        return Ok(brands);
    }

    // GET: api/brand/{id}
    [HttpGet("get/{id}")]
    public async Task<ActionResult<BrandView>> GetById(string id)
    {
        var brand = await brandCollection.Find(b => b.Id == id)
            .Project(b => new BrandView { Id = b.Id, Name = b.Name })
            .FirstOrDefaultAsync();

        if (brand == null)
        {
            return NotFound();
        }

        return Ok(brand);
    }

    // POST: api/brand
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] BrandCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            return BadRequest("Brand name is required.");
        }

        var result = await brandCollection.FindAsync(col => col.Name == command.Name);
        if (result.FirstOrDefaultAsync().Result != null)
            return BadRequest("Brand already exists");
        var newBrand = new BrandDb { Name = command.Name };
        
        await brandCollection.InsertOneAsync(newBrand);

        return CreatedAtAction(nameof(GetById), new { id = newBrand.Id }, new BrandView { Id = newBrand.Id, Name = newBrand.Name });
    }
    
}
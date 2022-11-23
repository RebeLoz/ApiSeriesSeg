using ApiSeries;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSeries.DTOs;
using ApiSeries.Entidades;
using ApiSeries.Migrations;

namespace ApiSeries.Controllers
{
    [ApiController]
    [Route("categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CategoriasController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("/listadoCategoria")]
        public async Task<ActionResult<List<Categoria>>> GetAll()
        {
            return await dbContext.Categorias.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerCategoria")]
        public async Task<ActionResult<CategoriaDTOConSeries>> GetById(int id)
        {
            var categoria = await dbContext.Categorias
                .Include(categoriaDB => categoriaDB.SerieCategoria)
                .ThenInclude(serieCategoriaDB => serieCategoriaDB.Serie)
                .Include(tipoDB => tipoDB.Tipos)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.SerieCategoria = categoria.SerieCategoria.OrderBy(x => x.Orden).ToList();

            return mapper.Map<CategoriaDTOConSeries>(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaCreacionDTO categoriaCreacionDTO)
        {

            if (categoriaCreacionDTO.SeriesIds == null)
            {
                return BadRequest("No se puede crear una categoria sin series.");
            }

            var seriesIds = await dbContext.Series
                .Where(serieBD => categoriaCreacionDTO.SeriesIds.Contains(serieBD.Id)).Select(x => x.Id).ToListAsync();

            if (categoriaCreacionDTO.SeriesIds.Count != seriesIds.Count)
            {
                return BadRequest("No existe uno de los series enviados");
            }

            var categoria = mapper.Map<Categoria>(categoriaCreacionDTO);

            OrdenarPorSeries(categoria);

            dbContext.Add(categoria);
            await dbContext.SaveChangesAsync();

            var categoriaDTO = mapper.Map<CategoriaDTO>(categoria);

            return CreatedAtRoute("obtenerCategoria", new { id = categoria.Id }, categoriaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoriaDB = await dbContext.Categorias
                .Include(x => x.SerieCategoria)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaDB == null)
            {
                return NotFound();
            }

            categoriaDB = mapper.Map(categoriaCreacionDTO, categoriaDB);

            OrdenarPorSeries(categoriaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Categorias.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }
            dbContext.Remove(new Categoria { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        private void OrdenarPorSeries(Categoria categoria)
        {
            if (categoria.SerieCategoria != null)
            {
                for (int i = 0; i < categoria.SerieCategoria.Count; i++)
                {
                    categoria.SerieCategoria[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CategoriaPatchDTO> patchDocument)
        {
            if (patchDocument == null) { return BadRequest(); }

            var categoriaDB = await dbContext.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaDB == null) { return NotFound(); }

            var categoriaDTO = mapper.Map<CategoriaPatchDTO>(categoriaDB);

            patchDocument.ApplyTo(categoriaDTO);

            var isValid = TryValidateModel(categoriaDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(categoriaDTO, categoriaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
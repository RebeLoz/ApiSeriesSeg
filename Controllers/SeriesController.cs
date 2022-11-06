using ApiSeries;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSeries.DTOs;
using ApiSeries.Entidades;

namespace ApiSeries.Controllers
{
    [ApiController]
    [Route("series")]
    public class SeriesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public SeriesController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetSerieDTO>>> Get()
        {
            var series = await dbContext.Series.ToListAsync();
            return mapper.Map<List<GetSerieDTO>>(series);
        }


        [HttpGet("{id:int}", Name = "obtenerserie")] 
        public async Task<ActionResult<SerieDTOConCategorias>> Get(int id)
        {
            var serie = await dbContext.Series
                .Include(serieDB => serieDB.SerieCategoria)
                .ThenInclude(serieCategoriaDB => serieCategoriaDB.Categoria)
                .FirstOrDefaultAsync(serieDB => serieDB.Id == id);

            if (serie == null)
            {
                return NotFound();
            }

            return mapper.Map<SerieDTOConCategorias>(serie);

        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetSerieDTO>>> Get([FromRoute] string nombre)
        {
            var series = await dbContext.Series.Where(serieBD => serieBD.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<GetSerieDTO>>(series);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SerieDTO serieDto)
        {
            //Ejemplo para validar desde el controlador con la BD con ayuda del dbContext

            var existeSerieMismoNombre = await dbContext.Series.AnyAsync(x => x.Name == serieDto.Name);

            if (existeSerieMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {serieDto.Name}");
            }

            var serie = mapper.Map<Serie>(serieDto);

            dbContext.Add(serie);
            await dbContext.SaveChangesAsync();

            var serieDTO = mapper.Map<GetSerieDTO>(serie);

            return CreatedAtRoute("obtenerserie", new { id = serie.Id }, serieDTO);
        }

        [HttpPut("{id:int}")] // api/series/1
        public async Task<ActionResult> Put(SerieDTO serieCreacionDTO, int id)
        {
            var exist = await dbContext.Series.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var serie = mapper.Map<Serie>(serieCreacionDTO);
            serie.Id = id;

            dbContext.Update(serie);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Series.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Serie()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
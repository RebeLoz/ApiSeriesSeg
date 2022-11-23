using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using ApiSeries.DTOs;
using ApiSeries.Entidades;


namespace ApiSeries.Controllers
{
    [ApiController]
    [Route("categorias/{categoriaId:int}/tipos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TiposController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public TiposController(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoDTO>>> Get(int categoriaId)
        {
            var existeCategoria = await dbContext.Categorias.AnyAsync(categoriaDB => categoriaDB.Id == categoriaId);
            if (!existeCategoria)
            {
                return NotFound();
            }
            var tipos = await dbContext.Tipos.Where(tipoDB => tipoDB.CategoriaId == categoriaId).ToListAsync();
            return mapper.Map<List<TipoDTO>>(tipos);
        }

        [HttpGet("{id:int}", Name = "obtenerTipo")]
        public async Task<ActionResult<TipoDTO>> GetById(int id)
        {
            var tipo = await dbContext.Tipos.FirstOrDefaultAsync(tipoDB => tipoDB.Id == id);
            if (tipo == null)
            {
                return NotFound();
            }
            return mapper.Map<TipoDTO>(tipo);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(int categoriaId, TipoCreacionDTO tipoCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var existeCategoria = await dbContext.Categorias.AnyAsync(categoriaDB => categoriaDB.Id == categoriaId);
            if (!existeCategoria)
            {
                return NotFound();
            }
            var tipo = mapper.Map<Tipos>(tipoCreacionDTO);
            tipo.CategoriaId = categoriaId;
            tipo.UsuarioId = usuarioId;
            dbContext.Add(tipo);
            await dbContext.SaveChangesAsync();
            var tipoDTO = mapper.Map<TipoDTO>(tipo);
            return CreatedAtRoute("obtenerTipo", new { id = tipo.Id, categoriaId = categoriaId }, tipoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int categoriaId, int id, TipoCreacionDTO tipoCreacionDTO)
        {
            var existeCategoria = await dbContext.Categorias.AnyAsync(categoriaDB => categoriaDB.Id == categoriaId);
            if (!existeCategoria)
            {
                return NotFound();
            }
            var existeTipo = await dbContext.Tipos.AnyAsync(tipoDB => tipoDB.Id == id);
            if (!existeTipo)
            {
                return NotFound();
            }
            var tipo = mapper.Map<Tipos>(tipoCreacionDTO);
            tipo.Id = id;
            tipo.CategoriaId = categoriaId;
            dbContext.Update(tipo);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
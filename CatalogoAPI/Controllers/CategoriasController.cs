using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using AutoMapper;
using CatalogoAPI.DTOs;
using Canducci.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace CatalogoAPI.Controllers
{
   // [Authorize(AuthenticationSchemes="Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    
   
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriasController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categorias
        /// <summary>
        /// Obtem todas as Categorias.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var categorias = await _context.Categorias.Include(x => x.Produtos).AsNoTracking().ToListAsync();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        /// <summary>
        /// Obtem todas as Categorias.
        /// </summary>

        [HttpGet("page/{page?}")]
        public async Task<IActionResult> GetSourcePaginated(int? page )
        {
            page ??= 1;
            if (page <= 0) page = 1;



             var categorias = await _context.Categorias.Include(x => x.Produtos).AsNoTracking().ToListAsync();
            var categoriasDTo = _mapper.Map<List<CategoriaDTO>>(categorias);

             var result= categoriasDTo
               .OrderBy(c => c.CategoriaId)
               .ToPaginatedRest(page.Value, 10);

           
           return Ok(result);


        }



        // GET: api/Categorias/5
        /// <summary>
        /// Obtem todas as Categorias.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
             
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Obtem todas as Categorias.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromBody]CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Obtem todas as Categorias.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria( [FromBody]CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();


            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return CreatedAtAction("GetCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
        }

        // DELETE: api/Categorias/5
        /// <summary>
        /// Obtem todas as Categorias.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CategoriaId == id);
        }
      
    }
}

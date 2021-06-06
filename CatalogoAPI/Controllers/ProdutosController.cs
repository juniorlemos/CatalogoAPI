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

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProdutosController(AppDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
        {
            var produtos= await _context.Produtos.ToListAsync();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }
        // Ação de paginação dos dados

        [HttpGet("page/{page?}")]
        public async Task<IActionResult> GetSourcePaginated(int? page)
        {
            page ??= 1;
            if (page <= 0) page = 1;

            var produtos = await _context.Produtos.ToListAsync();
            var produtosDTo = _mapper.Map<List<ProdutoDTO>>(produtos);

             var result= produtosDTo
               .OrderBy(c => c.ProdutoId)
               .ToPaginatedRest(page.Value, 10);

           
            return Ok(result);
        }



        // PUT: api/Produtos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id,[FromBody] ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);
            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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

        // POST: api/Produtos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto([FromBody]ProdutoDTO produtoDto)
        {

            var produto = _mapper.Map<Produto>(produtoDto);

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return CreatedAtAction("GetProduto", new { id = produto.ProdutoId }, produtoDTO);
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }
    }
}

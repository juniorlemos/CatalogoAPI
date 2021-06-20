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

namespace CatalogoAPI.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Produces ("application/json")] 
   [ApiConventionType(typeof(DefaultApiConventions))]
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


        /// <summary>
        /// Obtem todos os Produtos.
        /// </summary>
        /// <returns> Todos os Objetos Produtos </returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
        {
            var produtos= await _context.Produtos.ToListAsync();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }
        
        /// <summary>
        /// Obtem um Produto pelo seu id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Produtos</returns>
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

        /// <summary>
        /// Obtem  Produtos através da página especificada.
        /// </summary>
        /// /// <param name="Page"></param>
        /// <returns>10 Objetos Produtos da pagina </returns>
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
        /// <summary>
        /// Modifica um Produto através do id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produtoDto"></param>
        /// <returns>retorna 400 ou 200</returns>
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
        /// <summary>
        /// Insere um novo produto
        /// </summary>
        /// /// <remarks>
        /// Exemplo de  request:
        ///
        ///     POST /produtos
        ///     {
        ///        "Produtoid": 1,
        ///        "name": "Categoria",
        ///        "Descricao": "Um simples produto",
        ///        "Preco": "50,00",
        ///        "ImagemURL": "http://teste.net/1.jpg" 
        ///        "CategoriaId": "1",
        ///     }
        /// </remarks>
        /// <returns>O objeto Produto incluido</returns>
        /// <remarks>Retorna um objeto Produto incluído</remarks>

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

        /// <summary>
        /// Deleta um Produto.
        /// </summary>
        ///  <param name="id"> Código do produto </param>
        /// <returns></returns>
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

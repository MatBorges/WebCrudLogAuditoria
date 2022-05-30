using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCrudLogAuditoria.Data;
using WebCrudLogAuditoria.Models;

namespace WebCrudLogAuditoria.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            _context.LogAuditoria.Add(
                new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = $"Entrou na tela de Listagem de Produtos"
                }
            );
            _context.SaveChanges();
            return View(await _context.Produto.ToListAsync());
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.LogAuditoria.Add(
                new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = $"Entrou na tela de Detalhes do Produto: {produto.Id} - {produto.Nome} na Data {DateTime.Now.ToLongDateString()}"
                }
            );
            _context.SaveChanges();

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            _context.LogAuditoria.Add(
                new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = "Entrou na tela de cadastro"
                }
                );
            _context.SaveChanges();
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                //Log de cadastro de produto, registra o produto e a data do cadastro
                _context.LogAuditoria.Add(
                        new LogAuditoria
                        {
                            EmailUsuario = User.Identity.Name,
                            DetalhesAuditoria = $"Cadastrou o produto: {produto.Nome} Data do Cadastro: {DateTime.Now.ToLongDateString()}"
                        }
                    );
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            _context.LogAuditoria.Add(
                new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = $"Entrou na tela de Edição do Produto: {produto.Id} - {produto.Nome} na Data {DateTime.Now.ToLongDateString()}"
                }
            );
            _context.SaveChanges();
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                    _context.LogAuditoria.Add(
                        new LogAuditoria
                        {
                            EmailUsuario = User.Identity.Name,
                            DetalhesAuditoria = $"Atualizou o produto: {produto.Nome} Data do Atualização: {DateTime.Now.ToLongDateString()}"
                        }
                    );
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();

            _context.LogAuditoria.Add(
                new LogAuditoria
                {
                    EmailUsuario = User.Identity.Name,
                    DetalhesAuditoria = $"Deletou o produto: {produto.Nome} Data do Exclusão: {DateTime.Now.ToLongDateString()}"
                }
            );
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.Id == id);
        }
    }
}

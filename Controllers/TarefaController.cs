using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            // Buscar o Id no banco utilizando o EF
            var tarefa = await _context.Tarefas.FindAsync(id);

            // Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            if (tarefa == null)
            {
                return NotFound();
            }
            return Ok(tarefa);
        }

        public async Task<IActionResult> ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando o EF
            var tarefas = await _context.Tarefas.ToListAsync();

            // Retornar a lista de tarefas
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            // Buscar as tarefas no banco que contenham o título recebido por parâmetro
            var tarefas = await _context.Tarefas
                                        .Where(t => t.Titulo.Contains(titulo))
                                        .ToListAsync();

            // Retornar a lista de tarefas encontradas
            return Ok(tarefas);
        }


        // Outro endpoint de exemplo
        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            // Exemplo: Buscar tarefas pela data (caso exista esse endpoint para referência)
            var tarefas = await _context.Tarefas
                                        .Where(t => t.Data == data)
                                        .ToListAsync();

            return Ok(tarefas);
        }


        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar as tarefas no banco que contenham o status recebido por parâmetro
            var tarefas = await _context.Tarefas
                                        .Where(t => t.Status == status)
                                        .ToListAsync();

            // Retornar a lista de tarefas encontradas
            return Ok(tarefas);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

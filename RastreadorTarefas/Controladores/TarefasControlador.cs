using Microsoft.AspNetCore.Mvc;
using RastreadorTarefas.Modelos;
using RastreadorTarefas.Repositorios;

namespace RastreadorTarefas.Controladores;


[ApiController]
[Route("api/tarefas")]
[Produces("application/json")]
public class TarefasControlador : ControllerBase
{
    private readonly RepositorioTarefa _repositorio;

    public TarefasControlador(RepositorioTarefa repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemTarefa>), StatusCodes.Status200OK)]
    public IActionResult ListarTodas([FromQuery] StatusTarefa? status = null)
    {
        List<ItemTarefa> tarefas = _repositorio.ObterPorStatus(status);
        return Ok(tarefas);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ItemTarefa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObterPorId(int id)
    {
        ItemTarefa? tarefa = _repositorio.ObterPorId(id);

        if (tarefa is null)
            return NotFound(new { mensagem = $"Tarefa com Id {id} não encontrada." });

        return Ok(tarefa);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemTarefa), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Adicionar([FromBody] RequisicaoCriarTarefa requisicao)
    {
        if (string.IsNullOrWhiteSpace(requisicao.Descricao))
            return BadRequest(new { mensagem = "A descrição da tarefa não pode ser vazia." });

        ItemTarefa tarefa = _repositorio.Adicionar(requisicao.Descricao);
        return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ItemTarefa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Atualizar(int id, [FromBody] RequisicaoCriarTarefa requisicao)
    {
        if (string.IsNullOrWhiteSpace(requisicao.Descricao))
            return BadRequest(new { mensagem = "A descrição da tarefa não pode ser vazia." });

        ItemTarefa? tarefa = _repositorio.Atualizar(id, requisicao.Descricao);

        if (tarefa is null)
            return NotFound(new { mensagem = $"Tarefa com Id {id} não encontrada." });

        return Ok(tarefa);
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ItemTarefa), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AlterarStatus(int id, [FromBody] RequisicaoAlterarStatus requisicao)
    {
        ItemTarefa? tarefa = _repositorio.AlterarStatus(id, requisicao.Status);

        if (tarefa is null)
            return NotFound(new { mensagem = $"Tarefa com Id {id} não encontrada." });

        return Ok(tarefa);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Remover(int id)
    {
        bool removida = _repositorio.Remover(id);

        if (!removida)
            return NotFound(new { mensagem = $"Tarefa com Id {id} não encontrada." });

        return NoContent();
    }

 
    [HttpDelete("limpar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult LimparTodas()
    {
        _repositorio.LimparTodas();
        return NoContent();
    }
}
public record RequisicaoCriarTarefa(string Descricao);
public record RequisicaoAlterarStatus(StatusTarefa Status);

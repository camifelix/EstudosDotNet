using System.Text;
using System.Text.Json;
using RastreadorTarefas.Modelos;

namespace RastreadorTarefas.Repositorios;

public class RepositorioTarefa
{
    private readonly string _caminhoArquivo;
    private readonly object _bloqueio = new();

    private static readonly JsonSerializerOptions _opcoesJson = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public RepositorioTarefa(string caminhoArquivo = "tarefas.json")
    {
        _caminhoArquivo = caminhoArquivo;
    }


    public List<ItemTarefa> ObterTodas()
    {
        lock (_bloqueio)
        {
            if (!File.Exists(_caminhoArquivo))
                return new List<ItemTarefa>();

            string json = File.ReadAllText(_caminhoArquivo, Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(json))
                return new List<ItemTarefa>();

            return JsonSerializer.Deserialize<List<ItemTarefa>>(json, _opcoesJson)
                   ?? new List<ItemTarefa>();
        }
    }

    public ItemTarefa? ObterPorId(int id)
    {
        return ObterTodas().FirstOrDefault(t => t.Id == id);
    }
    public List<ItemTarefa> ObterPorStatus(StatusTarefa? status)
    {
        List<ItemTarefa> tarefas = ObterTodas();
        return status.HasValue
            ? tarefas.Where(t => t.Status == status.Value).ToList()
            : tarefas;
    }
    public ItemTarefa Adicionar(string descricao)
    {
        lock (_bloqueio)
        {
            List<ItemTarefa> tarefas = ObterTodasSemBloqueio();

            int proximoId = tarefas.Count > 0 ? tarefas.Max(t => t.Id) + 1 : 1;

            var tarefa = new ItemTarefa
            {
                Id           = proximoId,
                Descricao    = descricao,
                Status       = StatusTarefa.Pendente,
                CriadoEm    = DateTime.Now,
                AtualizadoEm = DateTime.Now
            };

            tarefas.Add(tarefa);
            SalvarTodas(tarefas);
            return tarefa;
        }
    }
    public ItemTarefa? Atualizar(int id, string novaDescricao)
    {
        lock (_bloqueio)
        {
            List<ItemTarefa> tarefas = ObterTodasSemBloqueio();
            ItemTarefa? tarefa = tarefas.FirstOrDefault(t => t.Id == id);

            if (tarefa is null) return null;

            tarefa.Descricao    = novaDescricao;
            tarefa.AtualizadoEm = DateTime.Now;

            SalvarTodas(tarefas);
            return tarefa;
        }
    }
    public ItemTarefa? AlterarStatus(int id, StatusTarefa novoStatus)
    {
        lock (_bloqueio)
        {
            List<ItemTarefa> tarefas = ObterTodasSemBloqueio();
            ItemTarefa? tarefa = tarefas.FirstOrDefault(t => t.Id == id);

            if (tarefa is null) return null;

            tarefa.Status       = novoStatus;
            tarefa.AtualizadoEm = DateTime.Now;

            SalvarTodas(tarefas);
            return tarefa;
        }
    }
    public bool Remover(int id)
    {
        lock (_bloqueio)
        {
            List<ItemTarefa> tarefas = ObterTodasSemBloqueio();
            ItemTarefa? tarefa = tarefas.FirstOrDefault(t => t.Id == id);

            if (tarefa is null) return false;

            tarefas.Remove(tarefa);
            SalvarTodas(tarefas);
            return true;
        }
    }

    public void LimparTodas()
    {
        lock (_bloqueio)
        {
            if (File.Exists(_caminhoArquivo))
                File.Delete(_caminhoArquivo);
        }
    }

    private List<ItemTarefa> ObterTodasSemBloqueio()
    {
        if (!File.Exists(_caminhoArquivo))
            return new List<ItemTarefa>();

        string json = File.ReadAllText(_caminhoArquivo, Encoding.UTF8);

        if (string.IsNullOrWhiteSpace(json))
            return new List<ItemTarefa>();

        return JsonSerializer.Deserialize<List<ItemTarefa>>(json, _opcoesJson)
               ?? new List<ItemTarefa>();
    }
    private void SalvarTodas(List<ItemTarefa> tarefas)
    {
        string json = JsonSerializer.Serialize(tarefas, _opcoesJson);
        File.WriteAllText(_caminhoArquivo, json, Encoding.UTF8);
    }
}

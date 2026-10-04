namespace RastreadorTarefas.Modelos;


public enum StatusTarefa
{
    Pendente,
    EmAndamento,
    Concluida
}

public class ItemTarefa
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}

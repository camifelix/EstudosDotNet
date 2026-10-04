using System.Text.Json.Serialization;
using RastreadorTarefas.Repositorios;

var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddSingleton<RepositorioTarefa>();

construtor.Services.AddControllers()
    .AddJsonOptions(opcoes =>
    {
        opcoes.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen(opcoes =>
{
    opcoes.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title       = "Rastreador de Tarefas",
        Description = "API para gerenciar tarefas: adicionar, atualizar, remover e acompanhar status."
    });

    var arquivoXml = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var caminhoXml = Path.Combine(AppContext.BaseDirectory, arquivoXml);
    if (File.Exists(caminhoXml))
        opcoes.IncludeXmlComments(caminhoXml);
});

var app = construtor.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opcoes =>
    {
        opcoes.SwaggerEndpoint("/swagger/v1/swagger.json", "Rastreador de Tarefas v1");
        opcoes.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

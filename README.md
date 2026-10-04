# Rastreador de Tarefas

API REST para gerenciar tarefas, desenvolvida em **ASP.NET Core .NET 8** com documentação via **Swagger**.  
Os dados são persistidos localmente em um arquivo `tarefas.json`, sem uso de banco de dados.

---

## Como executar

```bash
dotnet run --project RastreadorTarefas\RastreadorTarefas.csproj
```

Após iniciar, acesse a interface do Swagger em:

```
http://localhost:5098/swagger
```

---

## Endpoints

### Tarefas — `api/tarefas`

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/tarefas` | Lista todas as tarefas. Aceita filtro opcional por status. |
| `GET` | `/api/tarefas/{id}` | Retorna uma tarefa específica pelo seu Id. |
| `POST` | `/api/tarefas` | Cria uma nova tarefa com status inicial `Pendente`. |
| `PUT` | `/api/tarefas/{id}` | Atualiza a descrição de uma tarefa existente. |
| `PATCH` | `/api/tarefas/{id}/status` | Altera o status de uma tarefa. |
| `DELETE` | `/api/tarefas/{id}` | Remove uma tarefa pelo Id. |
| `DELETE` | `/api/tarefas/limpar` | Remove **todas** as tarefas de uma vez. |

---

### Detalhes de cada endpoint

#### `GET /api/tarefas`
Lista todas as tarefas cadastradas.  
Aceita o parâmetro de query `status` para filtrar os resultados:

| Valor | Descrição |
|-------|-----------|
| `Pendente` | Tarefas ainda não iniciadas |
| `EmAndamento` | Tarefas em execução |
| `Concluida` | Tarefas finalizadas |

**Exemplo:**
```
GET /api/tarefas?status=Pendente
```

---

#### `GET /api/tarefas/{id}`
Retorna os dados de uma tarefa específica.  
Responde com `404 Not Found` se o Id não existir.

---

#### `POST /api/tarefas`
Cria uma nova tarefa. A tarefa começa sempre com status `Pendente`.  
Responde com `400 Bad Request` se a descrição estiver vazia.

**Body:**
```json
{
  "descricao": "Estudar C#"
}
```

---

#### `PUT /api/tarefas/{id}`
Atualiza a descrição de uma tarefa existente.  
Responde com `404 Not Found` se o Id não existir.

**Body:**
```json
{
  "descricao": "Estudar C# e ASP.NET Core"
}
```

---

#### `PATCH /api/tarefas/{id}/status`
Altera o status de uma tarefa sem mexer na descrição.  
Responde com `404 Not Found` se o Id não existir.

**Body:**
```json
{
  "status": "EmAndamento"
}
```

Valores aceitos: `Pendente`, `EmAndamento`, `Concluida`

---

#### `DELETE /api/tarefas/{id}`
Remove permanentemente uma tarefa.  
Responde com `204 No Content` em caso de sucesso e `404 Not Found` se o Id não existir.

---

#### `DELETE /api/tarefas/limpar`
Remove **todas** as tarefas e apaga o arquivo `tarefas.json`.  
Na próxima inserção, os Ids recomeçam do 1.  
Responde com `204 No Content`.

---

## Modelo de dados

```json
{
  "id": 1,
  "descricao": "Estudar C#",
  "status": "Pendente",
  "criadoEm": "2026-10-04T10:00:00",
  "atualizadoEm": "2026-10-04T10:00:00"
}
```

---

## Estrutura do projeto

```
RastreadorTarefas/
├── Modelos/
│   └── ItemTarefa.cs          # Modelo da tarefa e enum StatusTarefa
├── Repositorios/
│   └── RepositorioTarefa.cs   # Leitura e escrita no arquivo JSON
├── Controladores/
│   └── TarefasControlador.cs  # Endpoints da API
└── Program.cs                 # Configuração da aplicação e Swagger
```

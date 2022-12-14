using CRUDminimalAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Método Post para devolver un objeto con su ID
app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

//Método Put para hacer un UPDATE de un registro que este en nuestra base de datos

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if(todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete= inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

//Método DELETE para eliminar de la tabla el registro
app.MapDelete("/todoitems", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});


//Método GET para cuándo se haga una solicitud de tipo get a todoitems, me devuelva todos los items de la tabla
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());


//Método GET para filtrar unicamente los items completos
app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

//Método GET para devolver el registro por ID
//Recibimos el ID, contexto y vamos a buscar con EF ese ID en la tabla Todo y no esta retornar NotFound

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());


app.Run();

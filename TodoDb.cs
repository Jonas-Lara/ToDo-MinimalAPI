//Contexto que necesita EF (Simula una base de datos)

using Microsoft.EntityFrameworkCore;

namespace CRUDminimalAPI
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options)
            : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>(); //Equivalente a la tabla
    }
}

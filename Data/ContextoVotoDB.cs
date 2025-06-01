using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ContextoVotoDB : DbContext
{
    public ContextoVotoDB(DbContextOptions<ContextoVotoDB> o) : base(o) { }

    public DbSet<Propuesta> Propuestas => Set<Propuesta>();
    public DbSet<Voto> Votos => Set<Voto>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Inversion> Inversiones => Set<Inversion>();
}
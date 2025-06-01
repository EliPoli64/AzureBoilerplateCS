namespace Data.Entities;
public class Voto
{
    public int Id { get; set; }
    public int PropuestaId { get; set; }
    public int UsuarioId { get; set; }
    public string VotoCifrado { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}
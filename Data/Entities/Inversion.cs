namespace Data.Entities;
public class Inversion
{
    public int Id { get; set; }
    public int PropuestaId { get; set; }
    public int UsuarioId { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}
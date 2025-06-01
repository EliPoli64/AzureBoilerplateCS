namespace Data.Entities;
public class Propuesta
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string Estado { get; set; } = "pendiente";
}
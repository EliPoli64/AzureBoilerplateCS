namespace Data.Entities;
public class Comentario
{
    public int Id { get; set; }
    public int PropuestaId { get; set; }
    public int UsuarioId { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public string Estado { get; set; } = "pendiente";
}
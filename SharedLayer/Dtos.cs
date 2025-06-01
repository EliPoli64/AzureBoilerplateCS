namespace SharedLayer;
public record PropuestaDto(int Id, string Titulo, string Descripcion, int UsuarioId);
public record InversionDto(int PropuestaId, int UsuarioId, decimal Monto);
public record VotoDto(int PropuestaId, int UsuarioId, string VotoPlano);
public record ComentarioDto(int PropuestaId, int UsuarioId, string Contenido);
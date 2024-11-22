using System;
using System.Collections.Generic;

namespace PlataformaCursos.Models;

public partial class TareasPublicacion
{
    public int PublicacionId { get; set; }

    public int? CursoId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }

    public virtual Curso? Curso { get; set; }
}

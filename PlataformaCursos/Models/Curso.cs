using System;
using System.Collections.Generic;

namespace PlataformaCursos.Models;

public partial class Curso
{
    public int CursoId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }

    public int? ProfesorId { get; set; }

    public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

    public virtual Usuario? Profesor { get; set; }

    public virtual ICollection<TareasPublicacion> TareasPublicaciones { get; set; } = new List<TareasPublicacion>();
}

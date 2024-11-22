using System;
using System.Collections.Generic;

namespace PlataformaCursos.Models;

public partial class Inscripcion
{
    public int InscripcionId { get; set; }

    public int? CursoId { get; set; }

    public int? AlumnoId { get; set; }

    public string? NumeroTarjeta { get; set; }

    public virtual Usuario? Alumno { get; set; }

    public virtual Curso? Curso { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Practica20250411.AppMVCCore.Models;

public partial class Bodega
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    public string Nombre { get; set; } = null!;

 
    public string? Notas { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}

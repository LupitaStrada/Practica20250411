using System;
using System.Collections.Generic;

namespace Practica20250411.AppMVCCore.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int? BodegaId { get; set; }

    public int? MarcaId { get; set; }

    public string? Notas { get; set; }

    public virtual Bodega? Bodega { get; set; }

    public virtual Marca? Marca { get; set; }
}

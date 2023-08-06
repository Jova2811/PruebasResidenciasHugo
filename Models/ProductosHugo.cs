using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebasResidenciasHugo.Models;

public partial class ProductosHugo
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    
    public decimal? Precio { get; set; }

    
    public int? Stock { get; set; }
}

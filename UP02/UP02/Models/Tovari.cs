using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class Tovari
{
    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int EdIzmId { get; set; }

    public decimal Price { get; set; }

    public int PostavshikId { get; set; }

    public int ProizvoditelId { get; set; }

    public int CategoryId { get; set; }

    public int? Skidka { get; set; }

    public int Count { get; set; }

    public string? Description { get; set; }

    public string Photo { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual EdIzm EdIzm { get; set; } = null!;

    public virtual Postavshik Postavshik { get; set; } = null!;

    public virtual Proizvoditel Proizvoditel { get; set; } = null!;

    public virtual ICollection<SostavZakaza> SostavZakazas { get; set; } = new List<SostavZakaza>();
}

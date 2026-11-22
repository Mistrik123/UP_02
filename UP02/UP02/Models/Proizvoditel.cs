using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class Proizvoditel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Tovari> Tovaris { get; set; } = new List<Tovari>();
}

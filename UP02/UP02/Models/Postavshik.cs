using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class Postavshik
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Tovari> Tovaris { get; set; } = new List<Tovari>();
}

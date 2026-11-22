using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Zakazi> Zakazis { get; set; } = new List<Zakazi>();
}

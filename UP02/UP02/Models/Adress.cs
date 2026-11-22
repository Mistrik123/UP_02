using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class Adress
{
    public int Id { get; set; }

    public int Index { get; set; }

    public int StreetId { get; set; }

    public int HouseNumber { get; set; }

    public virtual Street Street { get; set; } = null!;
}

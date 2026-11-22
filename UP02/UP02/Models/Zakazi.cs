using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class Zakazi
{
    public int IdZakaz { get; set; }

    public DateOnly DateZakaza { get; set; }

    public DateOnly? DateDostavka { get; set; }

    public int AdressVidahi { get; set; }

    public int ClientId { get; set; }

    public string? CodeReceipt { get; set; }

    public int StatusId { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual ICollection<SostavZakaza> SostavZakazas { get; set; } = new List<SostavZakaza>();

    public virtual Status Status { get; set; } = null!;
}

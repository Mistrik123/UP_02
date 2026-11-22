using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class User
{
    public int Id { get; set; }

    public string Famil { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Otchestvo { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Zakazi> Zakazis { get; set; } = new List<Zakazi>();
}

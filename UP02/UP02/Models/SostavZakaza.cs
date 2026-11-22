using System;
using System.Collections.Generic;

namespace UP02.Models;

public partial class SostavZakaza
{
    public int Id { get; set; }

    public int ZakazId { get; set; }

    public string Article { get; set; } = null!;

    public int Count { get; set; }

    public virtual Tovari ArticleNavigation { get; set; } = null!;

    public virtual Zakazi Zakaz { get; set; } = null!;
}

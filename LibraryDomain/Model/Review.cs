using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class Review : Entity
{

    public long BookId { get; set; }

    public long UserId { get; set; }

    public string Text { get; set; } = null!;

    public double? Rate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

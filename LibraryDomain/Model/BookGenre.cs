using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;

public partial class BookGenre : Entity
{

    public long bookid { get; set; }

    public long genreid { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}

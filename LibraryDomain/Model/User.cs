using System;
using System.Collections.Generic;

namespace LibraryDomain.Model;
public enum UserStatus
{
    User = 0,  
    Admin = 1
}
public partial class User : Entity
{

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public byte Age { get; set; }
    public UserStatus Status { get; set; } = UserStatus.User;
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();

    public virtual ICollection<UserLibrary> UserLibraries { get; set; } = new List<UserLibrary>();
}

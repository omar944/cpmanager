﻿namespace Entities.App;

public class Participation : BaseEntity
{
    public int Rank { get; set; }
    public string? Year { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    
    public string? TeamName { get; set; }
    public User User { get; set; }=null!;
}
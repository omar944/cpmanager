﻿namespace CodeforcesTool.Models;

public class ProblemFromApiDto
{
    public int ContestId { get; set; }
    public string? Index { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }
    public List<string>? Tags { get; set; }
}
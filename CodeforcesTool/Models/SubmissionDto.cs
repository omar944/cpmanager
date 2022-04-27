﻿namespace CodeforcesTool.Models;

public class SubmissionDto
{
    public ProblemDto? Problem { get; set; }
    public Author? Author { get; set; }
    public string? ProgrammingLanguage { get; set; }
    public string? Verdict { get; set; }
}

public class Author
{
    public List<Member>? Members { get; set; }
}

public class Member
{
    public string? Handle { get; set; }
}


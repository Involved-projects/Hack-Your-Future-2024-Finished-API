namespace API.DTOs;

public class TodoDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Assignee { get; set; }

    public string? Description { get; set; }
}
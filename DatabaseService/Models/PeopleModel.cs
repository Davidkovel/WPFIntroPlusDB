using DatabaseService.Abstractions;

namespace DatabaseService.Models;

public class PeopleModel : IModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }

    public PeopleModel()
    {
        ;
    }

    public PeopleModel(int id, string name, string surname, string course)
    {
        Id = id;
        Name = name;
        Surname = surname;
    }

    public PeopleModel(int id, string name, string surname)
    {
        Id = id;
        Name = name;
        Surname = surname;
    }
}
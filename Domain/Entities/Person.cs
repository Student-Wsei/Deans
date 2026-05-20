using Domain.ValueObjects;

namespace Domain.Entities;

public abstract class Person: EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public EmailAddress Email { get; set; } = new EmailAddress("placeholder@example.com");
}

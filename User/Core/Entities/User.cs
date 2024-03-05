using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Entity;

namespace Core.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }

    private User(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public static (User user, ValidationResult? erros) Create(string name, string email)
    {
        var user = new User(name, email);
        var resultValidation = new UserValidator().Validate(user);
        return resultValidation is null ? (user, null) : (user, resultValidation);
    }

    public ValidationResult? Update(string name, string email)
    {
        Name = name;
        Email = email;
        UpdatedAt = DateTime.Now;
        return new UserValidator().Validate(this);
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().MinimumLength(3).WithMessage("Nome invalido");
        RuleFor(user => user.Email).EmailAddress().WithMessage("Email invalido");
    }
}
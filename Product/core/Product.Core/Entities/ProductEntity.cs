using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Shared.Core.Entity;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Product.Core.Entities;

public class ProductEntity : BaseEntity
{
    [MaxLength(150)]
    public string Description { get; private set; }
    [MaxLength(50)]
    public string Category { get; private set; }
    
    public int ValueUnit { get; private set; }
    public int Stock { get; private set; }

    private ProductEntity(){}
    private ProductEntity(string description, string category, int value,  int stock)
    {
        Description = description;
        Category = category;
        Stock = stock;
        ValueUnit = value;
    }
    
    public static (ProductEntity product, ValidationResult? erros) Create(string description, string category, int value, int stock)
    {
        var product = new ProductEntity(description, category, value, stock);
        var resultValidation = new ProductValidator(true).Validate(product);
        return resultValidation is null ? (product, null) : (product, resultValidation);
    }

    public ValidationResult? Update(string description, string category)
    {
        Description = description;
        Category = category;

        return new ProductValidator().Validate(this);
    }

    public bool AddStock(int quantity)
    {
        if (quantity <= 0) return false;

        Stock += quantity;
        return true;
    }
    
    public bool RemoveStock(int quantity)
    {
        if (quantity <= 0) return false;
        Stock -= quantity;

        return Stock >= 0;
    }
}

public class ProductValidator : AbstractValidator<ProductEntity>
{
    public ProductValidator(bool validateStock = false)
    {
        RuleFor(product => product.Description).NotEmpty().MinimumLength(3).WithMessage("Nome invalido");
        RuleFor(product => product.Category).NotEmpty().MinimumLength(2).WithMessage("Categoria invalida");
        RuleFor(product => product.ValueUnit).GreaterThanOrEqualTo(100).WithMessage("Valor invalido");
        if(validateStock)
            RuleFor(product => product.Stock).GreaterThanOrEqualTo(0).WithMessage("Quantidate de items invalida");
    }
}
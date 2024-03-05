using Microsoft.AspNetCore.Mvc;
using Product.Core.Services.Interfaces;
using Product.web.Controllers.Product.Dto;
using Shared.Core.Notification;

namespace Product.web.Controllers.Product;

public static class ProductController
{
    public static RouteGroupBuilder ConfigureProductController(this RouteGroupBuilder app)
    {
        var group = app.MapGroup("products").WithDescription("Product").WithTags("Product");
        group.MapGet("/", GetProducts);
        group.MapGet("/{id:guid}", GetProduct);
        group.MapGet("/custom", GetProductsCustom);
        group.MapPost("/", CreateProduct);
        group.MapPut("/{id:guid}", UpdateProduct);
        group.MapPut("/{id:guid}/add/{quantity:int}", AddInStock);
        group.MapPut("/{id:guid}/remove/{quantity:int}", RemoveInStock);
        group.MapDelete("/{id:guid}", DeleteProduct);
        return group;
    }

    private static async Task<IResult> GetProducts(IProductServices productService)
    {
        var users = await productService.GetAllAsync();

        return TypedResults.Ok(users);
    }

    private static async Task<IResult> GetProduct(Guid id, IProductServices productService,
        NotificationContext notificationContext)
    {
        if (Guid.Empty == id)
        {
            notificationContext.AddNotification("User", $"User {id} invalido");
            return TypedResults.BadRequest();
        }

        var user = await productService.GetByIdAsync(id);

        if (user is not null) return TypedResults.Ok(user);

        notificationContext.AddNotification("User", $"User {id} invalido");
        return TypedResults.BadRequest();
    }
    private static async Task<IResult> GetProductsCustom([FromQuery]string ids, IProductServices productService,
        NotificationContext notificationContext)
    {
        var listIds = ids.Split(',').Select(x =>
        {
            Guid id;
            return Guid.TryParse(x, out id) ? id : Guid.Empty;
        }).Where(x => x != Guid.Empty);

        var products = await Task.WhenAll(listIds.Select(productService.GetByIdAsync));

        return TypedResults.Ok(products.Where(x=> x != null));
    }

    private static async Task<IResult> CreateProduct(CreateProductDto dto, IProductServices productService)
    {
        var product = await productService.CreateAsync(dto.Description, dto.Category, dto.Value, dto.InitStock);
        return TypedResults.Created($"/api/products/{product?.Id}", product);
    }

    private static async Task<IResult> AddInStock(Guid id, int quantity, IProductServices productService)
    {
        var result = await productService.AddInStock(id, quantity);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> RemoveInStock(Guid id, int quantity, IProductServices productService)
    {
        var result = await productService.RemoveInStock(id, quantity);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> UpdateProduct(Guid id, CreateProductDto dto, IProductServices productServices)
    {
        var product = await productServices.UpdateAsync(id, dto.Description, dto.Category);
        return TypedResults.Ok(product);
    }

    private static async Task<IResult> DeleteProduct(Guid id, IProductServices productServices)
    {
        var result = await productServices.DeleteAsync(id);
        return TypedResults.Ok(result);
    }
}
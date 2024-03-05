namespace Product.web.Controllers.Product.Dto;

public class CreateProductDto
{
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int InitStock { get; set; }
    public int Value { get; set; }
}
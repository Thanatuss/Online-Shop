namespace Application.Command.DTO.ProductDTO;

public class UpdateDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public long Price { get; set; }
    public bool IsActive { get; set; } = true;
}
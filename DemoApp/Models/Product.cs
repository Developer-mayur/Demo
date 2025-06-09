using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public byte[]? ImageData { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public DateTime? CreatedAt { get; set; }
}

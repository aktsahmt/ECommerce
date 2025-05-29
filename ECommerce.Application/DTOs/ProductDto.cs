﻿namespace ECommerce.Application.DTOs
{
    public class ProductDto
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public required string Currency { get; set; }
        public string? Category { get; set; }
        public int Stock { get; set; }
    }
}

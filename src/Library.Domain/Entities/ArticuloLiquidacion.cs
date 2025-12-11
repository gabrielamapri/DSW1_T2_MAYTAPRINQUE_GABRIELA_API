using System;

namespace Library.Domain.Entities
{
    public class ArticuloLiquidacion
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

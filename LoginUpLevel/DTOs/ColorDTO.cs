using LoginUpLevel.Models;

namespace LoginUpLevel.DTOs
{
    public class ColorDTO
    {
        public int Id { get; set; }
        public string NameColor { get; set; } = null!;
        public ICollection<ProductColorDTO> productColorDTOs { get; } = new List<ProductColorDTO>();
    }
}

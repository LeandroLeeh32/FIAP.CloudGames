using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
        public ICollection<PromotionGame> PromotionGames { get; set; } = new List<PromotionGame>();
    }
}

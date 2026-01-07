using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Domain.Entities
{
    public class Game
    {
        public string Name { get; }

        public Game(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Nome do jogo é obrigatío", nameof(name)); 

            Name = name;
            
        }

    }
}

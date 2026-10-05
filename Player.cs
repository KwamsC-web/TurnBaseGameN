using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace TurnBaseGameN
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Defense { get; set; }

        public int Mana { get; set; }

        protected static Random random = new Random();

        public Player(string name, int health, int attackPower, int defense, int mana)
        {
            this.Name = name;
            this.Health = health;
            this.AttackPower = attackPower;
            this.Defense = defense;
            this.Mana = mana;
        }

        public bool AttackPlayer(Player opponent)
        {
            if (random.NextDouble() > 0.2)
            {
                int damage = Math.Max(0, AttackPower - opponent.Defense);
                opponent.Health = Math.Max(0, opponent.Health - damage);
                this.Mana += 5;
                return true;
            }
            return false;
        }

        public int UseSkill(Player opponent)
        {
            if (this.Mana >= 20)
            {
                this.Mana -= 20;
                if (random.NextDouble() > 0.2)
                {
                    int skillDamage = this.AttackPower * 2;
                    opponent.Health = Math.Max(0, opponent.Health - skillDamage);
                    return 1;
                }
                else
                {
                    return 2;
                }

            }
            return 0;
        }
    }
}

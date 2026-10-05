using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnBaseGameN
{
    public class EnemyPlayer:Player
    {
        public EnemyPlayer(string name, int health, int attackPower, int defensePower, int mana) : base(name, health, attackPower, defensePower, mana)
        {
        }

        public bool TakeTurn(Player opponent)
        {
            if (this.Mana >= 10 && random.NextDouble() > 0.5)
            {
                UseSkill(opponent);
                return false;
            }
            else
            {
                AttackPlayer(opponent);
                return true;
            }
        }
    }
}

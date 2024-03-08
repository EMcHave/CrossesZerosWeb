using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    internal class StupidBotPlayer : Player
    {
        private Random random;
        public StupidBotPlayer(Role role)
            : base(role)
        {
            random = new Random();
        }

        public override void MakeMove(GameField field, int x = -1, int y = -1)
        {
            Role tempRole = Role.Crosses;
            while(tempRole != Role.None)
            {
                x = random.Next(0, field.FieldSize);
                y = random.Next(0, field.FieldSize);
                tempRole = field.Cells[x, y].CellRole;
            }
            field.Cells[x, y].CellRole = this.PlayerRole;
            if (CheckWin(field, PlayerRole, x, y))
                RaiseGameOverEvent();
            field.latestOccupiedCell = new Move(x, y);
        }
        public override string ToString()
        {
            return "Бот";
        }
    }
}

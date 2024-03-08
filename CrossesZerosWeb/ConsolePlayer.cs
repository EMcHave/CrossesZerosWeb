using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    internal class ConsolePlayer : Player
    {
        public ConsolePlayer(Role role)
            : base(role) { }

        public override void MakeMove(GameField field, int x, int y)
        {
            field.Cells[x, y].CellRole = PlayerRole;
            if (CheckWin(field, PlayerRole, x, y))
                RaiseGameOverEvent();
            field.latestOccupiedCell = new Move(x, y);
        }
        public override string ToString() { return "Игрок в консоли"; }

    }
}

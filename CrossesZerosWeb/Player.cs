using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    internal abstract class Player
    {
        public Role PlayerRole { get; private set; }
        public bool PlayerWon { get; private set; }

        public delegate void WinHandler(object sender);
        public event WinHandler PlayerWonEvent;
        public Player(Role role) 
        { 
            PlayerRole = role;
            PlayerWon = false;
        }

        public abstract void MakeMove(GameField field, int x = -1, int y = -1);
        public virtual void RaiseGameOverEvent() { PlayerWonEvent?.Invoke(this); } 
        public static bool CheckWin(GameField field, Role role, int x, int y)
        {
            for (int i = 0; i < field.FieldSize; i++) 
            {
                if (field.Cells[x, i].CellRole != role)
                    break;
                if (i == field.FieldSize - 1)
                    return true;
            }
            for (int i = 0; i < field.FieldSize; i++)
            {
                if (field.Cells[i, y].CellRole != role)
                    break;
                if (i == field.FieldSize - 1)
                    return true;
            }
            if(x == y)
            {
                for (int i = 0; i < field.FieldSize; i++)
                {
                    if (field.Cells[i, i].CellRole != role)
                        break;
                    if (i == field.FieldSize - 1)
                        return true;
                }
            }
            if(x + y == field.FieldSize - 1)
            {
                for (int i = 0; i < field.FieldSize; i++)
                {
                    if (field.Cells[i, (field.FieldSize - 1) - i].CellRole != role)
                        break;
                    if (i == field.FieldSize - 1)
                        return true;
                }
            }
            return false;
        }
    }
}

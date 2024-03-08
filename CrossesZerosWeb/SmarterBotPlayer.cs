using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    internal class SmarterBotPlayer : Player
    {
        private Random random;
        private Role oppositeRole;
        public SmarterBotPlayer(Role role) : base(role)
        {
            random = new Random();
            if (role == Role.Crosses)
                oppositeRole = Role.Zeros;
            else if (role == Role.Zeros)
                oppositeRole = Role.Crosses;
        }

        public override void MakeMove(GameField field, int x = -1, int y = -1)
        {
            var playerLastMove = field.latestOccupiedCell;
            int rowCounter = 0;
            int colCounter = 0;
            int diagCounter = 0;
            int antiDiagCounter = 0;

            Move move = new Move(-1,-1);

            for (int j = 0; j < field.FieldSize; j++)
                if (field.Cells[playerLastMove.X, j].CellRole == oppositeRole)
                    rowCounter++;

            for (int i = 0; i < field.FieldSize; i++)
                if (field.Cells[i, playerLastMove.Y].CellRole == oppositeRole)
                    colCounter++;

            if (playerLastMove.X == playerLastMove.Y)
                for (int i = 0; i < field.FieldSize; i++)
                    if (field.Cells[i, i].CellRole == oppositeRole)
                        diagCounter++;

            if (playerLastMove.X + playerLastMove.Y == field.FieldSize - 1)
                for (int i = 0; i < field.FieldSize; i++)
                    if (field.Cells[i, (field.FieldSize - 1) - i].CellRole == oppositeRole)
                        antiDiagCounter++;

            Dictionary<string, int> counters = new Dictionary<string, int>();
            counters["rowCounter"] = rowCounter;
            counters["colCounter"] = colCounter;
            counters["diagCounter"] = diagCounter;
            counters["antiDiagCounter"] = antiDiagCounter;
            bool movePossible = false;

            foreach (KeyValuePair<string, int> counter in counters.OrderByDescending(key => key.Value))
            {
                movePossible = MakeBestMove(counter.Key, field, playerLastMove, ref move);
                if (movePossible)
                    break;
            }


            field.Cells[move.X, move.Y].CellRole = PlayerRole;
            if (CheckWin(field, PlayerRole, move.X, move.Y))
                RaiseGameOverEvent();
            field.latestOccupiedCell = new Move(move.X, move.Y);
        }

        private bool MakeBestMove(string counter, GameField field, Move playerLastMove, ref Move move)
        {
            switch (counter)
            {
                case "rowCounter":
                    {
                        for (int j = 0; j < field.FieldSize; j++)
                            if (field.Cells[playerLastMove.X, j].CellRole == Role.None)
                            {
                                move = new Move(playerLastMove.X, j);
                                return true;
                            }
                        break;
                    }
                case "colCounter":
                    {
                        for (int i = 0; i < field.FieldSize; i++)
                            if (field.Cells[i, playerLastMove.Y].CellRole == Role.None)
                            {
                                move = new Move(i, playerLastMove.Y);
                                return true;
                            }
                        break;
                    }
                case "diagCounter":
                    {
                        for (int i = 0; i < field.FieldSize; i++)
                            if (field.Cells[i, i].CellRole == Role.None)
                            {
                                move = new Move(i, i);
                                return true;
                            }
                        break;
                    }
                case "antiDiagCounter":
                    {
                        for (int i = 0; i < field.FieldSize; i++)
                            if (field.Cells[i, (field.FieldSize - 1) - i].CellRole == Role.None)
                            {
                                move = new Move(i, (field.FieldSize - 1) - i);
                                return true;
                            }
                        break;
                    }
            }
            return false;
        }
    }
}

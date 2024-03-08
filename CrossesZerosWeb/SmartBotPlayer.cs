using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    internal class SmartBotPlayer : Player
    {
        private Role oppositeRole;
        public SmartBotPlayer(Role role) : base(role)
        {
            if (role == Role.Crosses)
                oppositeRole = Role.Zeros;
            else if (role == Role.Zeros)
                oppositeRole = Role.Crosses;
        }

        public override void MakeMove(GameField field, int x = -1, int y = -1)
        {
            //var bestMove = MiniMax(field, true).Item2;
            var bestMove = MiniMaxAB(field, true, int.MinValue, int.MaxValue).Item2;
            field.Cells[bestMove.X, bestMove.Y].CellRole = PlayerRole;
            if (CheckWin(field, PlayerRole, bestMove.X, bestMove.Y))
                RaiseGameOverEvent();
            field.latestOccupiedCell = new Move(bestMove.X, bestMove.Y);
        }

        private Tuple<int, Cell> MiniMax(GameField field, bool isMaximizing)
        {
            List<Cell> availiableMoves = field.EmptyCells();

            if (GameOver(field))
                return new Tuple<int, Cell>(EvaluateBoard(field), new Cell());


            
            int bestValue = 0;
            Cell bestMove = new Cell();
            Role role = Role.None;
            if (isMaximizing)
            {
                bestValue = int.MinValue;
                role = PlayerRole;
            } 
            else if(!isMaximizing)
            {
                bestValue = int.MaxValue;
                role = oppositeRole;
            }

            
            foreach(Cell cell in availiableMoves)
            {
                GameField newField = field.Clone();
                newField.Cells[cell.X, cell.Y].CellRole = role;
                int hypoteticalValue = MiniMax(newField, !isMaximizing).Item1;

                if(isMaximizing && hypoteticalValue > bestValue)
                {
                    bestValue = hypoteticalValue;
                    bestMove = cell;
                }
                if(!isMaximizing && hypoteticalValue < bestValue)
                {
                    bestValue = hypoteticalValue;
                    bestMove = cell;
                }
            }
            
            
            return new Tuple<int, Cell>(bestValue, bestMove);
        }

        private Tuple<int, Cell> MiniMaxAB(
            GameField field,
            bool isMaximizing,
            int alpha,
            int beta)
        {
            List<Cell> availiableMoves = field.EmptyCells();

            if (GameOver(field))
                return new Tuple<int, Cell>(EvaluateBoard(field), new Cell());


            int bestValue = 0;
            Cell bestMove = new Cell();
            Role role;
            if (isMaximizing)
            {
                bestValue = int.MinValue;
                role = PlayerRole;
                foreach (Cell cell in availiableMoves)
                {
                    GameField newField = field.Clone();
                    newField.Cells[cell.X, cell.Y].CellRole = role;
                    int hypoteticalValue = MiniMaxAB(newField, !isMaximizing, alpha, beta).Item1;
                    if (hypoteticalValue > bestValue)
                    {
                        bestValue = hypoteticalValue;
                        bestMove = cell;
                    }
                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                        break;
                }
                return new Tuple<int, Cell>(bestValue, bestMove);
            }
            else
            {
                bestValue = int.MaxValue;
                role = oppositeRole;
                foreach (Cell cell in availiableMoves)
                {
                    GameField newField = field.Clone();
                    newField.Cells[cell.X, cell.Y].CellRole = role;
                    int hypoteticalValue = MiniMaxAB(newField, !isMaximizing, alpha, beta).Item1;
                    if (hypoteticalValue < bestValue)
                    {
                        bestValue = hypoteticalValue;
                        bestMove = cell;
                    }
                    beta = Math.Min(beta, bestValue);

                    if(beta <= alpha)
                        break;
                }
                return new Tuple<int, Cell>(bestValue, bestMove);
            }
        }

        private bool GameOver(GameField field)
        {
            if (HasWon(field, Role.Zeros) || HasWon(field, Role.Crosses) || !field.EmptyCellsExist())
                return true;
            return false;
        }

        private static bool HasWon(GameField field, Role role)
        {
            int counter = 0;
            for (int i = 0; i < field.FieldSize; i++)
            {
                for (int j = 0; j < field.FieldSize; j++)
                    if (field.Cells[i, j].CellRole == role)
                        counter++;
                if (counter == field.FieldSize) return true;
                counter = 0;
            }
                
            
            for (int j = 0; j < field.FieldSize; j++)
            {
                for (int i = 0; i < field.FieldSize; i++)
                    if (field.Cells[i, j].CellRole == role)
                        counter++;
                if (counter == field.FieldSize) return true;
                counter = 0;
            }
                
            
            for(int i = 0; i < field.FieldSize; i++)
                if (field.Cells[i, i].CellRole == role)
                    counter++;
            if (counter == field.FieldSize) return true;
            counter = 0;


            for (int i = 0; i < field.FieldSize; i++)
                if (field.Cells[(field.FieldSize - 1) - i, i].CellRole == role)
                    counter++;
            if (counter == field.FieldSize) return true;

            return false;
        }

        private int EvaluateBoard(GameField field)
        {
            if (HasWon(field, PlayerRole))
                return 10;
            if (HasWon(field, oppositeRole))
                return -10;
            else
                return 0;
        }
    }
}

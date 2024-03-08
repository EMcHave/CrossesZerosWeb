using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    public enum Role
    {
        Crosses,
        Zeros,
        None
    }

    struct Move
    {
        public Move(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }

    internal class Game
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public GameField Field { get; private set; }
        public bool GameCompleted { get; set; }
        public Role Winner { get; private set; }

        public Player CurrentPlayer { get; private set; }
        public delegate void GameCompletedHandler(object sender);
        public event GameCompletedHandler GameCompletedEvent;
        public Game(int n, Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = Player1;

            Player1.PlayerWonEvent += FinishGame;
            Player2.PlayerWonEvent += FinishGame;
            Field = new GameField(n);
        }

        public bool MakeGameStep(int i, int j)
        {
            if(CurrentPlayer is ConsolePlayer)
                if (Field.Cells[i, j].CellRole != Role.None)
                    return false;
           
            CurrentPlayer.MakeMove(Field, i, j);
            if (!GameCompleted)
                CheckDraw();

            SwapPlayers();
            return true;
        }

        private void SwapPlayers()
        {
            if (CurrentPlayer == Player1)
                CurrentPlayer = Player2;
            else if (CurrentPlayer == Player2)
                CurrentPlayer = Player1;
        }

        void FinishGame(object sender)
        {
            GameCompleted = true;
            Winner = (sender as Player).PlayerRole;
            GameCompletedEvent.Invoke(this);
        }

        void CheckDraw()
        {
            if (!Field.EmptyCellsExist())
            {
                GameCompleted = true;
                Winner = Role.None;
                GameCompletedEvent.Invoke(this);
            }
        }
    }
}

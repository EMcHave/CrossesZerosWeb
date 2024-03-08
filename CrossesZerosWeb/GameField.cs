using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesZeros
{
    internal struct Cell : ICloneable
    {
        public Role CellRole { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int i, int j)
        {
            X = i;
            Y = j;
            CellRole = Role.None;
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    internal class GameField
    {
        private int fieldSize;
        public Move latestOccupiedCell {  get; set; }
        public int FieldSize
        {
            get { return fieldSize; }
            private set { fieldSize = value; }
        }

        public Cell[,] Cells { get; private set; }
        public GameField(int size)
        {
            fieldSize = size;
            Cells = new Cell[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    Cells[i, j] = new Cell(i, j);
        }

        public GameField(int size, Cell[,] cells)
        {
            fieldSize = size;
            Cells = cells;
        }

        public List<Cell> EmptyCells()
        {
            List<Cell> emptyCells = new List<Cell>();
            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                    if (Cells[i, j].CellRole == Role.None)
                        emptyCells.Add(Cells[i, j]);
            return emptyCells;
        }
        public bool EmptyCellsExist()
        {
            return EmptyCells().Count != 0;
        }

        public bool CellExists(int i, int j)
        {
            if (!Enumerable.Range(0, FieldSize).Contains(i))
                return false;
            else if (!Enumerable.Range(0, FieldSize).Contains(j))
                return false;
            return true;
        }

        public GameField Clone()
        {
            int size = fieldSize;
            Cell[,] newCells = new Cell[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    newCells[i, j] = (Cell)Cells[i, j].Clone();
            return new GameField(size, newCells);
        }
    }
}

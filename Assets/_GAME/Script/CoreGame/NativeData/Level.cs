using System;
using System.Collections.Generic;

namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// Level GridMap, represented by a 2D Array filled with cells classes.
    ///</summary>
    public class Level
    {
        #region variables

        private Cell[,] _cellsArray = null;

        private int _width = 3;

        private int _height = 3;

        private bool _isGameOver = false;

        private bool _isPaused = false;

        private Snake _snake;

        #endregion


        #region public API

        public int Width => _width;
        public int Height => _height;
        public Cell[,] CellsArray => _cellsArray;

        public bool IsGameOver
        {
            get { return _isGameOver; }
            set { _isGameOver = value; }
        }

        public bool IsPaused
        {
            get { return _isPaused; }
            set { _isPaused = value; }
        }

        public Snake SnakePlayer
        {
            get { return _snake; }
            set { _snake = value; }
        }

        #endregion

        /// <summary>
        /// Constructor for level map
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height)
        {
            _width = width;
            _height = height;

            _cellsArray = new Cell[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isCellOnBorder = x == 0 || x == width - 1 || y == 0 || y == height - 1;

                    if (isCellOnBorder)
                        _cellsArray[x, y] = new Cell(x, y, CellState.Wall);
                    else
                        _cellsArray[x, y] = new Cell(x, y, CellState.Empty);
                }
            }
        }

        public void PauseSwitch()
        {
            _isPaused = !_isPaused;
        }

        public void BuildFruit()
        {
            List<Cell> _availableCells = new List<Cell>();

            foreach (Cell cell in _cellsArray)
            {
                if (cell.State == CellState.Empty)
                    _availableCells.Add(cell);
            }

            if (_availableCells.Count > 0)
                _availableCells[new Random().Next(0, _availableCells.Count - 1)].CellUpdate(CellState.Fruit);
        }

        public void BuildSnake(Snake snakeToBuild)
        {
            _snake = snakeToBuild;

            _snake.SnakeCellsList = new List<Cell>();
            _snake.SnakeCellsList.Add(_cellsArray[_snake.Position.x, _snake.Position.y]);
        }
    }

    public delegate void RendererUpdate();
}
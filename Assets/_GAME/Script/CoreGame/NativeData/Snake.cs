using System.Collections.Generic;

namespace SuperSnake.ClassicSnake
{
    public class Snake
    {
        private Coords2D _position;

        private Direction _forwardDirection;

        private int _length = 1;

        private int _score = 0;

        private int _growUpdate;

        private Level _level;

        private List<Cell> _snakeCellsList;

        private List<Direction> _inputQueue = new List<Direction>();

        private int _fruitPower;

        #region Public API

        public Coords2D Position => _position;
        public Direction ForwardDirection
        {
            get => _forwardDirection;
            set => _forwardDirection = value;
        }
        public int Length => _length;
        public int Score => _score;
        public int GrowUpdate
        {
            get => _growUpdate;
            set => _growUpdate = value;
        }
        public List<Cell> SnakeCellsList
        {
            get => _snakeCellsList;
            set => _snakeCellsList = value;
        }

        public List<Direction> InputQueue
        {
            get => _inputQueue;
            set => _inputQueue = value;
        }

        public int FruitPower
        {
            get => _fruitPower;
            set => _fruitPower = value;
        }

        public event RendererUpdate ScoreRendererUpdate;

        #endregion

        public Snake(int x, int y, Direction forwardDirection, int growUpdate, int fruitPower, Level level)
        {
            _position = new Coords2D(x, y);
            _forwardDirection = forwardDirection;
            _growUpdate = growUpdate;
            _level = level;
            _fruitPower = fruitPower;

            _level.CellsArray[_position.x, _position.y].CellUpdate(CellState.Snake);
        }

        public Snake(int fruitPower, Level level)
        {
            _position = new Coords2D(level.Width / 2, 1);
            _forwardDirection = Direction.Up;
            _growUpdate = 4;
            _level = level;
            _fruitPower = fruitPower;

            _level.CellsArray[_position.x, _position.y].CellUpdate(CellState.Snake);
        }

        public void Update()
        {
            if (_inputQueue.Count > 0)
            {
                _forwardDirection = _inputQueue[0];
                _inputQueue.RemoveAt(0); 
            }

            if (_forwardDirection == Direction.Up)
                _position.y++;
            else if (_forwardDirection == Direction.Down)
                _position.y--;
            else if (_forwardDirection == Direction.Left)
                _position.x--;
            else if (_forwardDirection == Direction.Right)
                _position.x++;

            bool isCurrentCellWallOrSnake = _level.CellsArray[_position.x, _position.y].State == CellState.Wall || _level.CellsArray[_position.x, _position.y].State == CellState.Snake;

            if (isCurrentCellWallOrSnake)
                _level.IsGameOver = true;
            else
            {
                if(_level.CellsArray[_position.x, _position.y].State == CellState.Fruit)
                {
                    _growUpdate += _fruitPower;
                    _level.BuildFruit();

                    _score += _fruitPower;
                    ScoreRendererUpdate.Invoke();
                }

                _level.CellsArray[_position.x, _position.y].CellUpdate(CellState.Snake);
                _snakeCellsList.Add(_level.CellsArray[_position.x, _position.y]);

                if (_growUpdate == 0)
                {
                    _snakeCellsList[0].CellUpdate(CellState.Empty);
                    _snakeCellsList.RemoveAt(0);
                }
                else
                {
                    _growUpdate--;
                }
            }

            _length = _snakeCellsList.Count;
        }
    }

    public enum Direction
    {
        Right,
        Left,
        Up,
        Down,
    }
}

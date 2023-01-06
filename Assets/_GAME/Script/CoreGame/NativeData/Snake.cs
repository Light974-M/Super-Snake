using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace SuperSnake.ClassicSnake
{
    public class Snake
    {
        private int _playerIndex = 0;

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

        public Coords2D Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Direction ForwardDirection
        {
            get => _forwardDirection;
            set => _forwardDirection = value;
        }
        public int Length => _length;
        public int Score
        {
            get => _score;
            set => _score = value;
        }
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

        public event SnakeRendererUpdate ScoreRendererUpdate;

        public Level Level
        {
            get => _level;
            set => _level = value;
        }

        #endregion

        public Snake(int x, int y, Direction forwardDirection, int growUpdate, int fruitPower, int playerIndex, Level level)
        {
            _playerIndex = playerIndex;
            _position = new Coords2D(x, y);
            _forwardDirection = forwardDirection;
            _growUpdate = growUpdate;
            _level = level;
            _fruitPower = fruitPower;

            _snakeCellsList = new List<Cell>() { _level.CellsArray[_position.x, _position.y] };

            _level.CellsArray[_position.x, _position.y].CellUpdate(CellState.Snake);
        }

        public Snake(int fruitPower, int playerIndex, Level level)
        {
            _playerIndex = playerIndex;
            _position = new Coords2D((level.Width / 2) + playerIndex, 1 + playerIndex);
            _forwardDirection = Direction.Up;
            _growUpdate = 4;
            _level = level;
            _fruitPower = fruitPower;

            _snakeCellsList = new List<Cell>() { _level.CellsArray[_position.x, _position.y] };

            _level.CellsArray[_position.x, _position.y].CellUpdate(CellState.Snake);
        }

        public void Update()
        {
            DirectionUpdate();

            Move();

            if(IsCurrentCellCrossable)
            {
                if(IsCurrentCellTypeOf(CellState.Fruit))
                    HitFruit();

                UpdateCells();
            }
            else
                _level.IsGameOver = true;

            _length = _snakeCellsList.Count;
        }

        public void DirectionQueueUpdate(Direction toGo, Direction ToNotGo)
        {
            bool availableFirstSlot = _inputQueue.Count == 0 && _forwardDirection != ToNotGo;
            bool availableSecondSlot = _inputQueue.Count == 1 && _inputQueue[0] != ToNotGo;
            bool availableChangeSecondSlot = _inputQueue.Count == 2 && _inputQueue[0] != ToNotGo;

            if (availableFirstSlot || availableSecondSlot)
                _inputQueue.Add(toGo);
            else if (availableChangeSecondSlot)
                _inputQueue[1] = toGo;
        }

        private void DirectionUpdate()
        {
            if (_inputQueue.Count > 0)
            {
                _forwardDirection = _inputQueue[0];
                _inputQueue.RemoveAt(0);
            }
        }

        private void Move()
        {
            if (_forwardDirection == Direction.Up)
                _position.y++;
            else if (_forwardDirection == Direction.Down)
                _position.y--;
            else if (_forwardDirection == Direction.Left)
                _position.x--;
            else if (_forwardDirection == Direction.Right)
                _position.x++;
        }

        private void HitFruit()
        {
            _growUpdate += _fruitPower;
            _level.BuildFruit();

            _score += _fruitPower;
            ScoreRendererUpdate.Invoke(_playerIndex);
        }

        private void UpdateCells()
        {
            _level.CellsArray[_position.x, _position.y].CellUpdate(CellState.Snake);
            _snakeCellsList.Add(_level.CellsArray[_position.x, _position.y]);

            if (NoGrowInQueue)
            {
                _snakeCellsList[0].CellUpdate(CellState.Empty);
                _snakeCellsList.RemoveAt(0);
            }
        }

        private bool IsCurrentCellTypeOf(CellState stateToTest)
        {
            return _level.CellsArray[_position.x, _position.y].State == stateToTest;
        }

        private bool IsCurrentCellCrossable
        {
            get => _level.CellsArray[_position.x, _position.y].State == CellState.Empty || _level.CellsArray[_position.x, _position.y].State == CellState.Fruit;
        }

        private bool NoGrowInQueue
        {
            get
            {
                bool isGrowUpdateNull = _growUpdate == 0;

                if (!isGrowUpdateNull)
                    _growUpdate--;

                return isGrowUpdateNull;
            }
        }
    }

    public enum Direction
    {
        Right,
        Left,
        Up,
        Down,
    }

    public delegate void SnakeRendererUpdate(int playerIndex);
}

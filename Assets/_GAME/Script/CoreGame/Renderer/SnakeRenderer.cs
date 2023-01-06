using SuperSnake.ClassicSnake;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace SuperSnake.ClassicSnake
{
    /// <summary>
    /// renderer of snake player, manage, update, and link it with level
    /// </summary>
    [AddComponentMenu("SuperSnake/ClassicSnake/Snake")]
    public class SnakeRenderer : MonoBehaviour
    {
        private LevelRenderer _linkedLevelRenderer;

        private Snake _snake;

        [SerializeField, Tooltip("parameters of this snake player")]
        private SnakeParameters _snakeParameters;

        private float _updateTimer = 0;

        private Dictionary<string, KeyCode> _controls = new Dictionary<string, KeyCode>()
        {
            { "Up", KeyCode.UpArrow },
            { "Down", KeyCode.DownArrow },
            { "Left", KeyCode.LeftArrow },
            { "Right", KeyCode.RightArrow }
        };


        #region public API

        public Snake LinkedSnake
        {
            get { return _snake; }
            set { _snake = value; }
        }

        public SnakeParameters SnakeInstanceParameters
        {
            get { return _snakeParameters; }
            set { _snakeParameters = value; }
        }

        public LevelRenderer LinkedLevelRenderer
        {
            get { return _linkedLevelRenderer; }
            set { _linkedLevelRenderer = value; }
        }

        #endregion

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (!_snake.Level.IsPaused && !_snake.Level.IsGameOver)
            {
                InputManager();
                SnakeUpdate();
                _updateTimer += Time.deltaTime;
            }
        }

        private void OnUpdateControls()
        {
            for (int i = 0; i < _snakeParameters.ValuesControls.Count; i++)
            {
                _controls[_controls.Keys.ToList()[i]] = _snakeParameters.ValuesControls[i];
            }

            _snakeParameters.ValuesControls = new List<KeyCode>(_controls.Values);
        }

        public void BuildSnake(int playerIndex)
        {
            BuildDefaultControls(playerIndex);

            _snakeParameters = new SnakeParameters();
            _snakeParameters.OverrideSnakeParameters(_linkedLevelRenderer.SnakeGlobalParameters);

            _snakeParameters.ValuesControls = new List<KeyCode>(_controls.Values);
            _snakeParameters.UpdateControls += OnUpdateControls;

            _snakeParameters.PositionX += playerIndex;
            _snakeParameters.PositionY += playerIndex;

            if (_snakeParameters == null || _snakeParameters.IsSnakeDefaultPos)
                _snake = new Snake(_snakeParameters.FruitsPower, playerIndex, _linkedLevelRenderer.Level);
            else
                _snake = new Snake(_snakeParameters.PositionX, _snakeParameters.PositionY, _snakeParameters.StartDirection, _snakeParameters.StartGrowUpdate, _snakeParameters.FruitsPower, playerIndex, _linkedLevelRenderer.Level);
        }

        private void BuildDefaultControls(int playerIndex)
        {
            if (playerIndex == 0)
            {
                _controls = new Dictionary<string, KeyCode>()
                {
                    { "Up", KeyCode.UpArrow },
                    { "Down", KeyCode.DownArrow },
                    { "Left", KeyCode.LeftArrow },
                    { "Right", KeyCode.RightArrow }
                };
            }
            else if (playerIndex == 1)
            {
                _controls = new Dictionary<string, KeyCode>()
                {
                    { "Up", KeyCode.Z },
                    { "Down", KeyCode.S },
                    { "Left", KeyCode.Q },
                    { "Right", KeyCode.D }
                };
            }
            else if (playerIndex == 2)
            {
                _controls = new Dictionary<string, KeyCode>()
                {
                    { "Up", KeyCode.I },
                    { "Down", KeyCode.K },
                    { "Left", KeyCode.J },
                    { "Right", KeyCode.L }
                };
            }
        }

        private void SnakeUpdate()
        {
            if (_updateTimer >= 1f / _snakeParameters.SnakeSpeed)
            {
                for (int i = 0; i < _snakeParameters.UpdateScale; i++)
                {
                    _snake.Update();
                    _linkedLevelRenderer.UpdateLengthUIInvoke();
                }

                _updateTimer = 0;
            }
        }

        private void InputManager()
        {
            if (Input.GetKeyDown(_controls["Up"]))
                _snake.DirectionQueueUpdate(Direction.Up, Direction.Down);
            else if (Input.GetKeyDown(_controls["Down"]))
                _snake.DirectionQueueUpdate(Direction.Down, Direction.Up);
            else if (Input.GetKeyDown(_controls["Right"]))
                _snake.DirectionQueueUpdate(Direction.Right, Direction.Left);
            else if (Input.GetKeyDown(_controls["Left"]))
                _snake.DirectionQueueUpdate(Direction.Left, Direction.Right);
        }
    }
}

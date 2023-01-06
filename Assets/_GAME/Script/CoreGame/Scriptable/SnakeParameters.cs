using System.Collections.Generic;
using UnityEngine;

namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// parameter of snake that will be used between scene loads
    ///</summary>
    [CreateAssetMenu(fileName = "NewSnakeParameters", menuName = "ScriptableObjects/ClassicSnake/SnakeParameters")]
    public class SnakeParameters : ScriptableObject
    {
        [SerializeField, Tooltip("is snake setup by default intern program ?")]
        private bool _isSnakeDefaultPos = false;

        [SerializeField, Tooltip("start position in x axis")]
        private int _positionX = 1;

        [SerializeField, Tooltip("start position in y axis")]
        private int _positionY = 1;

        [SerializeField, Tooltip("start direction of snake")]
        private Direction _startDirection = Direction.Up;

        [SerializeField, Tooltip("start growing of snake")]
        private int _startGrowUpdate = 4;

        [SerializeField, Tooltip("speed of snake update(in update/sec)")]
        private float _snakeSpeed = 1;

        [SerializeField, Tooltip("number of update per update, usefull to make ultra speed snakes")]
        private int _updateScale = 1;

        [SerializeField, Tooltip("number of tiles to grow with one fruit")]
        private int _fruitsPower = 1;

        [SerializeField, Tooltip("set the controls used by this player")]
        private List<KeyCode> _valuesControls;


        #region public API

        public bool IsSnakeDefaultPos
        {
            get { return _isSnakeDefaultPos; }
            set { _isSnakeDefaultPos = value; }
        }

        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        public Direction StartDirection
        {
            get { return _startDirection; }
            set { _startDirection = value; }
        }

        public int StartGrowUpdate
        {
            get { return _startGrowUpdate; }
            set { _startGrowUpdate = value; }
        }

        public float SnakeSpeed
        {
            get { return _snakeSpeed; }
            set { _snakeSpeed = value; }
        }

        public int FruitsPower
        {
            get { return _fruitsPower; }
            set { _fruitsPower = value; }
        }

        public int UpdateScale => _updateScale;

        public List<KeyCode> ValuesControls
        {
            get { return _valuesControls; }
            set { _valuesControls = value; }
        }

        public event UpdateSnakeParameters UpdateControls;

        #endregion

        public void OverrideSnakeParameters(SnakeParameters newSnakeParameters)
        {
            _isSnakeDefaultPos = newSnakeParameters.IsSnakeDefaultPos;
            _positionX = newSnakeParameters.PositionX;
            _positionY = newSnakeParameters.PositionY;
            _startDirection = newSnakeParameters.StartDirection;
            _startGrowUpdate = newSnakeParameters.StartGrowUpdate;
            _snakeSpeed = newSnakeParameters.SnakeSpeed;
            _updateScale = newSnakeParameters.UpdateScale;
            _fruitsPower = newSnakeParameters.FruitsPower;
        }

        private void OnValidate()
        {
            UpdateControls.Invoke();
        }
    }

    public delegate void UpdateSnakeParameters();
}

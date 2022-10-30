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

        public float SnakeSpeed => _snakeSpeed;

        public int FruitsPower => _fruitsPower;

        public int UpdateScale => _updateScale;

        #endregion
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// parameter of level that will be used between scene loads
    ///</summary>
    [CreateAssetMenu(fileName = "NewLevelParameters", menuName = "ScriptableObjects/ClassicSnake/LevelParameters")]
    public class LevelParameters : ScriptableObject
    {
        [SerializeField, Tooltip("width of level in x")]
        private int _width = 3;

        [SerializeField, Tooltip("height of level in y")]
        private int _height = 3;

        [SerializeField, Tooltip("number of player in scene")]
        private int _playerNumber = 1;


        #region public API

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int PlayerNumber
        {
            get => _playerNumber;
            set => _playerNumber = value;
        }

        #endregion
    }
}
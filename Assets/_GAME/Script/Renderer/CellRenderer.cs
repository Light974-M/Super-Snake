using UnityEngine;

namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// renderer for every game cells, making textures, and collision detections
    ///</summary>
    [AddComponentMenu("SuperSnake/ClassicSnake/CellRenderer")]
    public class CellRenderer : MonoBehaviour
    {
        [Header("INPUT TEXTURES\n")]

        [SerializeField]
        private SpriteRenderer _cellTile;

        [SerializeField]
        private Sprite _defaultSprite;

        [SerializeField]
        private Sprite _wallSprite;

        [SerializeField]
        private Sprite _snakeSprite;

        [SerializeField]
        private Sprite _fruitSprite;


        private Cell _linkedCell = null;


        #region Public API

        public Cell LinkedCell
        {
            get { return _linkedCell; }
            set { _linkedCell = value; }
        }

        #endregion

        public void RendererUpdateEventSetup()
        {
            _linkedCell.CellRendererUpdate += GraphicUpdate;
        }

        public void GraphicUpdate()
        {
            switch (_linkedCell.State)
            {
                case CellState.Empty:
                    _cellTile.sprite = _defaultSprite;
                    break;
                case CellState.Snake:
                    _cellTile.sprite = _snakeSprite;
                    break;
                case CellState.Wall:
                    _cellTile.sprite = _wallSprite;
                    break;
                case CellState.Fruit:
                    _cellTile.sprite = _fruitSprite;
                    break;

                default:
                    break;
            }
        }
    }
}

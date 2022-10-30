namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// represent cells epxloitable into a 2D grid
    ///</summary>
    public class Cell
    {
        #region Variables

        private Coords2D _position;

        private CellState _state;

        #endregion

        #region public API

        public Coords2D Position => _position;

        public CellState State
        {
            get => _state;
            set => _state = value;
        }

        public event RendererUpdate CellRendererUpdate;

        #endregion

        public Cell(int x, int y, CellState state)
        {
            _position = new Coords2D(x, y);
            _state = state;
        }

        public void CellUpdate(CellState newState)
        {
            _state = newState;
            CellRendererUpdate.Invoke();
        }
    } 

    public enum CellState
    {
        Empty,
        Snake,
        Wall,
        Fruit,
    }
}

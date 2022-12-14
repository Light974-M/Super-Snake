using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// renderer of levelMap for unityEngine
    ///</summary>
    [AddComponentMenu("SuperSnake/ClassicSnake/Level")]
    public class LevelRenderer : MonoBehaviour
    {
        #region variables

        private const int _intInfinity = 2147483647;


        [Header("USER PARAMETERS")]
        [SerializeField, Tooltip("parameters for level of current scene to be loaded")]
        private LevelParameters _levelParameters;

        [SerializeField, Tooltip("parameters for snake of current scene to be loaded")]
        private SnakeParameters _snakeParameters;


        [Header("LEVEL PARAMETERS")]
        [SerializeField]
        private GameObject _cellObjectPrefab;

        [SerializeField, Tooltip("GameObject that contain every cells of the grid")]
        private GameObject _cellsParentObject;


        [Header("CAMERA PARAMETERS")]
        [SerializeField, Tooltip("camera used to setup view of grid")]
        private Camera _gameCamera;


        [Header("UI EVENTS")]
        [SerializeField, Tooltip("event used to update timer value in UI")]
        private UnityEvent _updateTimerUI;

        [SerializeField, Tooltip("event used to update score value in UI")]
        private UnityEvent _updateScoreUI;

        [SerializeField, Tooltip("event used to update length of snake in UI")]
        private UnityEvent _updateLengthUI;


        /**************************PRIVATE VARIABLES*************************/

        private Level _level = null;

        private float _timer = 0;
        private float _updateTimer = 0;

        #endregion

        #region public API

        //make a get of Level, and make at the same time sure that _level is not null, if it is, it will make a new level
        public Level Level
        {
            get
            {
                if (_level == null)
                    _level = new Level(_levelParameters.Width + 2, _levelParameters.Height + 2);

                return _level;
            }
        }

        public float Timer => _timer;

        #endregion

        private void Awake()
        {
            _gameCamera = FindObjectOfType<Camera>();

            _gameCamera.transform.position = new Vector3((Level.Width / 2f - 0.5f) * transform.localScale.x, (Level.Height / 2f - 0.5f) * transform.localScale.y, -100) + transform.position;
            _gameCamera.orthographicSize = (Level.Width + Level.Height) / 3.4f;

            LevelBuild();

            RendererEventSetup();
        }

        private void Start()
        {
            BuildLD();
        }

        private void Update()
        {
            if (!Level.IsPaused && !Level.IsGameOver)
            {
                InputManager();
                SnakeUpdate();
                TimerUpdate();
            }

            if (Input.GetKeyDown(KeyCode.R))
                Respawn();

            PauseInputManager();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (_gameCamera == null)
                    _gameCamera = FindObjectOfType<Camera>();

                if (_cellsParentObject == null)
                {
                    if (transform.Find("Cells") == null)
                    {
                        _cellsParentObject = new GameObject("Cells");
                        _cellsParentObject.transform.SetParent(transform);
                    }
                    else
                    {
                        _cellsParentObject = transform.Find("Cells").gameObject;
                    }
                }

                _levelParameters.Width = Mathf.Clamp(_levelParameters.Width, 1, _intInfinity);
                _levelParameters.Height = Mathf.Clamp(_levelParameters.Height, 1, _intInfinity);

                _gameCamera.transform.position = new Vector3((Level.Width / 2f - 0.5f) * transform.localScale.x, (Level.Height / 2f - 0.5f) * transform.localScale.y, -100) + transform.position;
                _gameCamera.orthographicSize = (Level.Width + Level.Height) / 4f;

                for (int y = 0; y < Level.Height; y++)
                {
                    for (int x = 0; x < Level.Width; x++)
                    {
                        Gizmos.color = Color.blue;

                        if (Level.CellsArray[x, y].State == CellState.Wall)
                            Gizmos.color = Color.red;

                        Gizmos.DrawWireCube((new Vector3(x * transform.localScale.x, y * transform.localScale.y) + transform.position), Vector2.one * transform.localScale);
                    }
                }

                if ((_levelParameters.Width != Level.Width || _levelParameters.Height != Level.Height))
                {
                    _level = null;
                }
            }
        }

        private void LevelBuild()
        {
            _level = new Level(_levelParameters.Width + 2, _levelParameters.Height + 2);

            for (int y = 0; y < Level.Height; y++)
            {
                for (int x = 0; x < Level.Width; x++)
                {
                    GameObject cellPrefab = Instantiate(_cellObjectPrefab, new Vector3(x * transform.localScale.x, y * transform.localScale.y) + transform.position, Quaternion.identity);
                    if (!cellPrefab.TryGetComponent(out CellRenderer cellScript))
                        cellScript = cellPrefab.AddComponent<CellRenderer>();

                    cellPrefab.transform.SetParent(_cellsParentObject.transform);
                    cellScript.LinkedCell = Level.CellsArray[x, y];

                    if (cellScript.LinkedCell.State == CellState.Wall)
                        cellScript.GraphicUpdate();
                }
            }
        }

        private void BuildLD()
        {
            if (_snakeParameters == null || _snakeParameters.IsSnakeDefaultPos)
                _level.BuildSnake(new Snake(_snakeParameters.FruitsPower, Level));
            else
                _level.BuildSnake(new Snake(_snakeParameters.PositionX, _snakeParameters.PositionY, _snakeParameters.StartDirection, _snakeParameters.StartGrowUpdate, _snakeParameters.FruitsPower, Level));

            Level.BuildFruit();

            _level.SnakePlayer.ScoreRendererUpdate += ScoreUpdate;
            ScoreUpdate();
        }

        private void TimerUpdate()
        {
            _updateTimer += Time.deltaTime;
            _timer += Time.deltaTime;
            _updateTimerUI.Invoke();
        }

        private void ScoreUpdate()
        {
            _updateScoreUI.Invoke();
        }

        private void PauseInputManager()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _level.PauseSwitch();
        }

        private void DirectionQueueUpdate(Direction toGo, Direction ToNotGo)
        {
            bool availableFirstSlot = _level.SnakePlayer.InputQueue.Count == 0 && _level.SnakePlayer.ForwardDirection != ToNotGo;
            bool availableSecondSlot = _level.SnakePlayer.InputQueue.Count == 1 && _level.SnakePlayer.InputQueue[0] != ToNotGo;
            bool availableChangeSecondSlot = _level.SnakePlayer.InputQueue.Count == 2 && _level.SnakePlayer.InputQueue[0] != ToNotGo;

            if (availableFirstSlot || availableSecondSlot)
                _level.SnakePlayer.InputQueue.Add(toGo);
            else if (availableChangeSecondSlot)
                _level.SnakePlayer.InputQueue[1] = toGo;
        }

        private void RendererEventSetup()
        {
            CellRenderer[] cellsRendererArray = FindObjectsOfType<CellRenderer>();

            foreach (CellRenderer cellRenderer in cellsRendererArray)
                cellRenderer.RendererUpdateEventSetup();
        }

        private void InputManager()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                DirectionQueueUpdate(Direction.Up, Direction.Down);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                DirectionQueueUpdate(Direction.Down, Direction.Up);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                DirectionQueueUpdate(Direction.Right, Direction.Left);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                DirectionQueueUpdate(Direction.Left, Direction.Right);
        }

        private void SnakeUpdate()
        {
            if (_updateTimer >= 1f / _snakeParameters.SnakeSpeed)
            {
                for (int i = 0; i < _snakeParameters.UpdateScale; i++)
                {
                    _level.SnakePlayer.Update();
                    _updateLengthUI.Invoke();
                }

                _updateTimer = 0;
            }
        }

        public void Respawn()
        {
            Level.IsGameOver = false;
            Level.IsPaused = false;
            _timer = 0;
            _updateTimer = 0;

            foreach (Cell cell in _level.CellsArray)
            {
                if (cell.State != CellState.Empty && cell.State != CellState.Wall)
                    cell.CellUpdate(CellState.Empty);
            }

            BuildLD();
        }
    }
}

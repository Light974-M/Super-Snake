using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperSnake.ClassicSnake
{
    ///<summary>
    /// controller of every UI and buttons of menu Scene
    ///</summary>
    [AddComponentMenu("SuperSnake/ClassicSnake/MenuController")]
    public class MenuController : MonoBehaviour
    {
        #region variables

        [SerializeField, Tooltip("list of all panel used for every part of the menu")]
        private GameObject[] _panelList;

        [SerializeField, Tooltip("give the current loaded panel")]
        private Panel _panelDrawed;

        [SerializeField, Tooltip("scriptable preset parameters used to build level")]
        private LevelParameters _levelParameters;

        [SerializeField, Tooltip("scriptable preset parameters used to build snake")]
        private SnakeParameters _snakeParameters;

        [SerializeField, Tooltip("slider used to set width of level preset")]
        private Slider _widthSlider;

        [SerializeField, Tooltip("slider used to set height of level preset")]
        private Slider _heightSlider;

        [SerializeField, Tooltip("slider used to set snake speed of snake preset")]
        private Slider _speedSlider;

        [SerializeField, Tooltip("slider used to set fruit power of snake preset")]
        private Slider _fruitPowerSlider;

        [SerializeField, Tooltip("slider used to set fruit power of snake preset")]
        private Slider _playerNumberSlider;

        #endregion

        #region Public API

        /// <inheritdoc cref="_panelDrawed"/>
        public enum Panel
        {
            Main,
            LevelChoose,
            ClassicMinesweeper,
        }

        #endregion

        //called when the script instance is being loaded
        private void Awake()
        {
            //set slider value to scriptable corresponding values
            _widthSlider.value = _levelParameters.Width;
            _heightSlider.value = _levelParameters.Height;
            _speedSlider.value = _snakeParameters.SnakeSpeed;
            _fruitPowerSlider.value = _snakeParameters.FruitsPower;
            _playerNumberSlider.value = _levelParameters.PlayerNumber;
        }

        //called every frame
        private void Update()
        {
            //set slider value to scriptable corresponding values, clamped, bomb dynamically clamped to max tiles number
            _levelParameters.Width = (int)Mathf.Round(_widthSlider.value);
            _levelParameters.Height = (int)Mathf.Round(_heightSlider.value);
            _snakeParameters.SnakeSpeed = (int)Mathf.Round(_speedSlider.value);
            _snakeParameters.FruitsPower = (int)Mathf.Round(_fruitPowerSlider.value);
            _levelParameters.PlayerNumber = (int)Mathf.Round(_playerNumberSlider.value);
        }

        /// <summary>
        /// called when user click on play button
        /// </summary>
        public void OnPlayButton()
        {
            SceneManager.LoadScene("main");
        }

        /// <summary>
        /// called when user click on return button
        /// </summary>
        public void OnReturnButton()
        {
            //set panel value and update panel, depending on current panel value
            if ((int)_panelDrawed == 1 || (int)_panelDrawed == 2)
            {
                _panelDrawed--;
            }

            PanelUpdate();
        }

        /// <summary>
        /// called when user click on start button
        /// </summary>
        public void OnSettingsButton()
        {
            _panelDrawed++;

            PanelUpdate();
        }

        /// <summary>
        /// called when user click on quit button
        /// </summary>
        public void OnQuitButton()
        {
            //quit application
            Application.Quit();
        }

        /// <summary>
        /// called every time user change menu
        /// </summary>
        private void PanelUpdate()
        {
            // for each panel in panelList, activate only panel refering to current panel value
            for (int i = 0; i < _panelList.Length; i++)
            {
                if (i == (int)_panelDrawed)
                    _panelList[i].SetActive(true);
                else
                    _panelList[i].SetActive(false);
            }
        }
    } 
}

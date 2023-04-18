using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KID
{
    /// <summary>
    /// 場景控制
    /// </summary>
    public class SceneControl : MonoBehaviour
    {
        [SerializeField, Header("按鈕遊玩")]
        private Button btnPlay;
        [SerializeField, Header("按鈕離開")]
        private Button btnQuit;
        [SerializeField, Header("要遊玩的場景名稱")]
        private string nameToPlay = "遊戲場景";

        private void Awake()
        {
            btnPlay.onClick.AddListener(Play);
            btnQuit.onClick.AddListener(Quit);
        }

        /// <summary>
        /// 遊玩指定場景
        /// </summary>
        private void Play()
        {
            SceneManager.LoadScene(nameToPlay);
        }

        /// <summary>
        /// 離開
        /// </summary>
        private void Quit()
        {
            Application.Quit();
        }
    }
}

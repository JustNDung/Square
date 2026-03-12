using UnityEngine;

namespace GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        public void OnClickHomeButton()
        {
            SceneLoader.Instance.LoadHome();
        }
    }
}
using UnityEngine;

namespace Workshop
{
    public class WorkshopUI : MonoBehaviour
    {
        public void OnClickHomeButton()
        {
            SceneLoader.Instance.LoadHome();
        }
    }
}
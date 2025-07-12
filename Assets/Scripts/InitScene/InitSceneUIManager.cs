using UnityEngine;

public class InitSceneUIManager : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        SceneLoader.Instance.LoadScene("GamePlay");
    }
}

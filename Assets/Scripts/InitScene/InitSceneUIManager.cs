using UnityEngine;

public class InitSceneUIManager : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        SceneLoader.Instance.LoadScene("GamePlay");
    }
    
    public void OnClickWorkshopButton()
    {
        SceneLoader.Instance.LoadScene("LevelEditor");
    }
    
    public void OnClickChaptersButton()
    {
        SceneLoader.Instance.LoadScene("LevelList");
    }   
}

using UnityEngine;
public class CameraController : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float padding = 10f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    
    public void FitCameraToMap()
    {
        MapState tempMState = MapManager.Instance.MapState;
        float mapWidth = tempMState.Width * 2;
        float mapLength = tempMState.Length * 2;
        
        float aspect = cam.aspect;
        
        float sizeByLength = mapLength / 2;
        float sizeByWidth = mapWidth / (2 * aspect);

        float finalSize = Mathf.Max(sizeByLength, sizeByWidth);
        cam.orthographicSize = finalSize + padding;
        cam.transform.position = new Vector3(mapWidth / 2 - 1, cam.transform.position.y , mapLength / 2 - 1);
    }
}
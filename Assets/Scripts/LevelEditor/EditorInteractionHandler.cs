using UnityEngine;
public class EditorInteractionHandler : MonoBehaviour
{
    private void Update()
    {
        if (GameModeManager.Instance.CurrentMode != GameMode.LevelEditor) return;

        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider);
                var interactable = hit.collider.GetComponent<IEditorInteractable>();
                if (interactable != null)
                {
                    interactable.OnEditorRightClick();
                }
            }
        }
    }
}
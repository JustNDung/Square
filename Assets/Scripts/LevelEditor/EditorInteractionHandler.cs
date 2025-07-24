using UnityEngine;
public class EditorInteractionHandler : MonoBehaviour
{
    private IEditorInteractable _currentInteractable;
    private Outline _currentOutline;
    private void Update()
    {
        if (GameModeManager.Instance.CurrentMode != GameMode.LevelEditor) return;

        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            OnRightClick();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) // Left-click
        {
            OnESCDown();
        }
    }

    private void OnRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<IEditorInteractable>();
            if (interactable != null)
            {
                if (_currentInteractable != interactable)
                {
                    ResetOutline();
                    _currentInteractable = interactable;
                    interactable.OnEditorRightClick();
                    
                    _currentOutline = hit.collider.GetComponent<Outline>();
                    if (_currentOutline != null)
                    {
                        _currentOutline.enabled = true;
                        _currentOutline.OutlineColor = Color.red;
                    }
                }

                return;
            }
        }
    }
    
    private void OnESCDown()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.OnESCDown();
        }
        
        ResetOutline();
    }

    private void ResetOutline()
    {
        if (_currentOutline != null)
        {
            _currentOutline.enabled = false;
            _currentOutline = null;
        }
        
        _currentInteractable = null;
    }
}
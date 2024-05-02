using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIScreenChanger: MonoBehaviour
{
    [SerializeField] private UIScreen _actualScreen;
    [SerializeField] private UnityEvent _onUIClose; 


    private void OnEnable()
    {

        var onEscape =  SingletonActionPlayer.Instance.inputActions.UICustom.SenseESCBuild;

        onEscape.started += ToggleScreen;
    }


    private void OnDisable()
    {        
        var onEscape =  SingletonActionPlayer.Instance.inputActions.UICustom.SenseESCBuild;

        onEscape.started -= ToggleScreen;
    }

    
    public void ChangeToScreen(UIScreen screen)
    {
        _actualScreen.Close();
        _actualScreen = screen;
        _actualScreen.Open();
    }


    public void CloseScreen(UIScreen screen)
    {
        if(screen.GetLast() == null)
        {
            _onUIClose?.Invoke();
        }
        _actualScreen.Close();
    }


    private void ToggleScreen(InputAction.CallbackContext context)
    {
        var lastScreen = _actualScreen.GetLast();

        if(_actualScreen.IsActive)
        {
            if(lastScreen != null)
                ChangeToScreen(_actualScreen.GetLast());
            else if(_actualScreen.DeactivateByEscape)
                _actualScreen.Close();
        }
        else
            _actualScreen.Open();
    }


    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void TakeOffScreenSelections()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

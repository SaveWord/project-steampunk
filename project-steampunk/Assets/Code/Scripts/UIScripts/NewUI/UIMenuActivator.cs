using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIMenuActivator : MonoBehaviour
{
    [SerializeField] private UIMenu _mainMenu;    
    [SerializeField] private UnityEvent _onUIMenuActivation;
    [SerializeField] private UnityEvent _onUIMenuDeactivation;
    [SerializeField] private UIMenuChanger _menuChanger;


    private void Start()
    {
        _menuChanger.Initialize(_mainMenu);    
    }


    private void OnEnable()
    {
        var onEscape = SingletonActionPlayer.Instance.inputActions.UICustom.SenseESCBuild;

        onEscape.started += HandleClickOnEscape;
    }


    private void OnDisable()
    {
        var onEscape = SingletonActionPlayer.Instance.inputActions.UICustom.SenseESCBuild;

        onEscape.started -= HandleClickOnEscape;
    }


    private void ActivateUIMenu()
    {
        ActivateUIMenuFrom(_mainMenu);
    }


    public void ActivateUIMenuFrom(UIMenu menu)
    {
        _menuChanger.OpenMenu(menu);
        _onUIMenuActivation?.Invoke();
        ShowCursor();
    }


    public void DeactivateUIMenu()
    {
        _menuChanger.CloseCurrent();
        _onUIMenuDeactivation?.Invoke();
        HideCursor();
    }


    private void HandleClickOnEscape(InputAction.CallbackContext conext)
    {
        if(!_menuChanger.IsCurrentActive)
            ActivateUIMenu();
        else if(_menuChanger.CanMoveToParent && _menuChanger.CanBeClosedByEscape)
            _menuChanger.MoveToParent();
        else if(_menuChanger.CanBeClosedByEscape)
            DeactivateUIMenu();      
    }


    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

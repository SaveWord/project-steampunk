using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIMenu : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;
    public bool CanBeClosedByEscape => _canBeClosedByEscape;

    [SerializeField] private UnityEvent _onOpen;
    [SerializeField] private UnityEvent _onClose;
    [SerializeField] private UIMenu _parentMenu;
    [SerializeField] private bool _canBeClosedByEscape = true;



    public void Open()
    {
        gameObject.SetActive(true);
        _onOpen?.Invoke();
    }


    public void Close()
    {
        gameObject.SetActive(false);
        _onClose?.Invoke();
    }


    public UIMenu GetParent()
    {
        return _parentMenu;
    }


    public void TakeOffSelections()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

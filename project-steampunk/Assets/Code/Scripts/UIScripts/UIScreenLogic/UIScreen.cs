using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIScreen : MonoBehaviour
{
    public bool IsActive { get {return gameObject.activeSelf;}}
    [SerializeField] private UnityEvent _onOpen;
    [SerializeField] private UnityEvent _onClose;
    [SerializeField] private UIScreen _lastScreen;
    [SerializeField] private bool _deactivateByEscape = true;


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


    public UIScreen GetLast()
    {
        return _lastScreen;
    }
}

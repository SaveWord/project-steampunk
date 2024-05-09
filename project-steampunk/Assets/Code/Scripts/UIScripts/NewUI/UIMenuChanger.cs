using UnityEngine;
public class UIMenuChanger : MonoBehaviour
{
    public bool CanBeClosedByEscape => _currentMenu.CanBeClosedByEscape;
    public bool IsCurrentActive => _currentMenu.IsActive;
    public bool CanMoveToParent => _currentMenu.GetParent() != null;

    private UIMenu _currentMenu;



    public void Initialize(UIMenu menu)
    {
        _currentMenu = menu;
    }


    public void MoveToMenu(UIMenu menu)
    {
        CloseCurrent();
        OpenMenu(menu);
    }


    public void CloseCurrent()
    {
        _currentMenu.Close();
    }


    public void OpenMenu(UIMenu menu)
    {
        _currentMenu = menu;
        _currentMenu.Open();
    }


    public void MoveToParent()
    {
        MoveToMenu(_currentMenu.GetParent());
    }
}

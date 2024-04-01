using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public int selectNumberWeapon;
    private ActionPrototypePlayer inputActions;
    private Animator animator;

    private void OnEnable()
    {
        inputActions = SingletonActionPlayer.Instance.inputActions;
        //inputActions = new ActionPrototypePlayer();
        //inputActions.Enable();
        inputActions.Player.Weapon1.started += context => Weapon1();
        inputActions.Player.Weapon2.started += context => Weapon2();
        inputActions.Player.Weapon3.started += context => Weapon3();
        animator = transform.root.GetComponentInChildren<Animator>();
    }
    IEnumerator ISwitch()
    {
        animator.SetBool("switch", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("switch", false);
    } 
    private void OnDisable()
    {
        //inputActions.Disable();
        inputActions.Player.Weapon1.started -= context => Weapon1();
        inputActions.Player.Weapon2.started -= context => Weapon2();
        inputActions.Player.Weapon3.started -= context => Weapon3();
    }

    void Update()
    {
        Vector2 mouseScroll = inputActions.Player.SwitchWeaponMouse.ReadValue<Vector2>();
        MouseScroll(mouseScroll);
        animator.SetFloat("weaponType", selectNumberWeapon);
    }
    private void MouseScroll(Vector2 _input)
    {
        int previouseSelect = selectNumberWeapon;
        if(_input.y > 0)
        {
            if (selectNumberWeapon >= transform.childCount - 1)
            {
                selectNumberWeapon = 0;
            }
            else selectNumberWeapon++;
        }
        if(_input.y < 0)
        {
            if(selectNumberWeapon <= 0)
            {
                selectNumberWeapon = transform.childCount - 1;
            }
            else selectNumberWeapon--;
        }
        if(previouseSelect != selectNumberWeapon) { SelectedWeapon(); }
    }
    private void SelectedWeapon()
    {
        int i =0;
        foreach (Transform weapon in transform)
        {
            if(i == selectNumberWeapon)
            {
                weapon.gameObject.SetActive(true);
                StopCoroutine(ISwitch());
                StartCoroutine(ISwitch());
            }
            else weapon.gameObject.SetActive(false);
            i++;
        }
    }
    private void Weapon1()
    {
        selectNumberWeapon = 0;
        SelectedWeapon();
    }
    private void Weapon2()
    {
        selectNumberWeapon = 1;
        SelectedWeapon();
    }
    private void Weapon3()
    {
        selectNumberWeapon = 2;
        SelectedWeapon();
    }
}

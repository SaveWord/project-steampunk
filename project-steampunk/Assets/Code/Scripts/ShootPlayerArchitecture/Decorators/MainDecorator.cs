using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public abstract class MainDecorator : MonoBehaviour,IWeapon
{
    protected IWeapon weapon;
    protected float maxPatrons;


    public MainDecorator(IWeapon MainDecorator)
    {
        MainDecorator = weapon;
    }
    public virtual float Damage
    {
        get { return weapon.Damage; }
        set { }
    }
    public virtual float Range
    {
        get { return weapon.Range; }
        set { }
    }
    public virtual float ReloadSpeed
    {
        get { return weapon.ReloadSpeed; }
        set { }
    }
    public virtual float Patrons
    {
        get { return weapon.Patrons; }
        set { }
    }
    public virtual WeaponTypeDamage AttackType
    {
        get { return weapon.AttackType; }
        set { }
    }
    public virtual LayerMask enemyLayer
    {
        get { return weapon.enemyLayer; }
        set { }
    }

    public virtual bool Switch { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public virtual RaycastHit hitLine { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public virtual void Shoot(InputAction.CallbackContext context)
    {
        weapon.Shoot(context);
    }
    public async  virtual void Reload(InputAction.CallbackContext context)
    {
        weapon.Reload(context);
    }
    public async virtual Task CancelToken() { }
    public void ShowDamage(string message, Color color)
    {
        var _floatingMessage = (GameObject)Resources.Load("FloatingMessage", typeof(GameObject));
        Debug.Log("show damage");
        if (_floatingMessage)
        {
            var notice = Instantiate(_floatingMessage, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), UnityEngine.Quaternion.Euler(new Vector3(0, 0, 0)));
            notice.GetComponent<TextMeshPro>().text = message;
            notice.GetComponent<TextMeshPro>().color = color;
            notice.transform.parent = GameObject.Find("Main Camera").transform;
            notice.transform.localRotation = UnityEngine.Quaternion.Euler(new Vector3(0, 0, 0));
            float randomisedPositionx = (Random.Range(0.0f, 0.9f) * 2 - 1) / 3;
            float randomisedPositiony = (Random.Range(0.0f, 0.9f) * 2 - 1) / 3;
            notice.transform.localPosition = new Vector3(randomisedPositionx, randomisedPositiony, 3);

        }
    }
}

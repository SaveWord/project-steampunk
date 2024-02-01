using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static IWeapon;
using UnityEngine.UI;

public class ParametrsUpdateDecorator : MainDecorator
{
    private IWeapon _weapon;
    //parametrs weapon
    private float _updateDamage;
    private float _updateRange;
    private float _updateReload;
    private float _updatePatrons;
    private WeaponTypeDamage _updateWeaponType;
    private LayerMask _updateEnemyLayer;
    //effects and ui value
    private GameObject _vfxShootPrefab;
    private TextMeshProUGUI _patronsText;
    private bool isReload;


    //constructor
    public ParametrsUpdateDecorator(IWeapon weapon, float updateDamage,
        float updateRange, float updateReload, float updatePatrons,
        IWeapon.WeaponTypeDamage updateWeaponType, LayerMask mask,
        GameObject vfxShootPrefab, TextMeshProUGUI patronsText) : base(weapon)
    {
        _weapon = weapon;
        _updateDamage = updateDamage;
        _updateRange = updateRange;
        _updateReload = updateReload;
        _updatePatrons = updatePatrons;
        _updateWeaponType = updateWeaponType;
        _updateEnemyLayer = mask;
        maxPatrons = updatePatrons;
        
        //effect parametrs
        _vfxShootPrefab = vfxShootPrefab;
        _patronsText = patronsText;
    }

    //properties
    public override float Damage
    {
        get { return _updateDamage; }
        set { }
    }

    public override float Range
    {
        get { return _updateRange; }
        set { }
    }

    public override float ReloadSpeed
    {
        get { return _updateReload; }
        set { }
    }

    public override float Patrons
    {
        get { return _updatePatrons; }
        set { _updatePatrons = value; }
    }

    public override WeaponTypeDamage AttackType
    {
        get { return _updateWeaponType; }
        set { }
    }
    public override LayerMask enemyLayer
    {
        get {return _updateEnemyLayer;}
        set { }
    }

    //methods decorator shoot and reload logic
    public override void Shoot(InputAction.CallbackContext context)
    {
        if ((context.started || context.performed) && Patrons > 0)
        {
            Patrons--;
            _patronsText.text = Patrons.ToString();
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                out RaycastHit hit, Range, enemyLayer, QueryTriggerInteraction.Ignore))
            {
                
                hit.collider.TryGetComponent(out IDamageableProps damageableProps);
                damageableProps?.GetDamage(Damage);

                hit.collider.TryGetComponent(out IHealth damageable);
                damageable?.TakeDamage(Damage);

                ShowDamage(Damage + "");
            }
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
               out RaycastHit hitVfx, Range, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                var newVfxShoot = Instantiate(_vfxShootPrefab, hitVfx.point, Quaternion.identity);
            }
        }
        if (Patrons == 0)
        {
            Reload(context);
        }
    }

    private void ShowDamage(string message)
    {
        var _floatingMessage = (GameObject)Resources.Load("FloatingMessage", typeof(GameObject));
        Debug.Log("show damage");
        if (_floatingMessage)
        {
            var notice = Instantiate(_floatingMessage, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), UnityEngine.Quaternion.Euler(new Vector3(0, 0, 0)));
            notice.GetComponent<TextMeshPro>().text = message;
            notice.transform.parent = GameObject.Find("Main Camera").transform;
            notice.transform.localRotation = UnityEngine.Quaternion.Euler(new Vector3(0, 0, 0));
            float randomisedPosition = (Random.Range(0.0f, 0.9f) * 2 - 1) / 3;
            Debug.Log("pos" + randomisedPosition);
            notice.transform.localPosition = new Vector3(randomisedPosition, 0, 3);

        }
    }

    public async override void Reload(InputAction.CallbackContext context)
    {
        if (context.started && Patrons < maxPatrons && isReload == false)
        {
            Debug.Log("Activate");
            isReload = true;
           await Task.Delay((int)ReloadSpeed * 1000);
            isReload = false;
            Debug.Log("Deactivate");
            Patrons = maxPatrons;
            _patronsText.text = Patrons.ToString();
        }
    }
}

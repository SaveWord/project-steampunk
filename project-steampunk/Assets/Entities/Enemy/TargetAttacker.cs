using Enemies;
using System.Collections;
using UnityEngine;

public class TargetAttacker : MonoBehaviour, ITargetAttacker
{
    private bool _isReady;

    private void Awake()
    {
        _isReady = true;
    }

    public void Attack(ITarget target)
    {
        if (_isReady)
        {
            Debug.Log("Attack");
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _isReady = false;
        yield return new WaitForSeconds(2f);
        _isReady = true;
    }

    

}

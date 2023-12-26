using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyGetDamage : MonoBehaviour, IDamageable
{
    [SerializeField] protected float enemyHP;
    private NavMeshAgent agent;
    public static event Action weaponLevelUp;
    private EnemyShoot enemyShoot;
    private EnemyAttack enemyAttack;

    private float damageTimer = 0f;
    private float damageInterval = 2f;
    private float damageAmount = 0f;
    private Vector3 initialPosition;
    private bool frozeen;


    public float jumpHeight = 1.0f; 
    public float jumpSpeed = 5.0f; 

    private bool isJumping = false;

    private bool wasAgentEnabled;
    private bool isDescending = false;

    //ui
    public Slider sliderHP;
    protected Canvas canvas;
    private void Awake()
    {
        enemyShoot = GetComponent<EnemyShoot>();
        enemyAttack = GetComponent<EnemyAttack>();
        agent = GetComponent<NavMeshAgent>();
        wasAgentEnabled = agent.enabled;
    }
    protected void Update()
    {
        //ui
        sliderHP.transform.LookAt(Camera.main.transform, Vector3.up);

        if (isJumping)
        {
            float jumpStep = jumpSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * jumpStep);

            if (transform.position.y >= initialPosition.y + jumpHeight)
            {
                isJumping = false;
                isDescending = true;
            }
        }

        if (isDescending)
        {
            float descentStep = jumpSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * descentStep);

            if (transform.position.y <= initialPosition.y)
            {
                isDescending = false;
                transform.position = initialPosition;
                // Re-enable the NavMeshAgent after descending
                agent.enabled = wasAgentEnabled;
            }
        }
    }
    protected virtual void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        sliderHP.value = enemyHP;
        sliderHP.gameObject.SetActive(false);
    }
    protected IEnumerator HPView()
    {
        sliderHP.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        sliderHP.gameObject.SetActive(false);
    }
    public virtual void GetDamage(float damage)
    {
        enemyHP -= damage;
        ShowDamage(damage * 10 + "");
        sliderHP.value = enemyHP;
        StartCoroutine(HPView());
        if (enemyHP <= 0)
        {
            weaponLevelUp?.Invoke();
            Destroy(gameObject);
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
            float randomisedPosition = (UnityEngine.Random.Range(0.0f, 0.9f) * 2 - 1) / 3;
            notice.transform.localPosition = new Vector3(randomisedPosition, 0, 3);
        }
    }

    public void Status(string state)
    {
        switch (state)
        {
            case "stun":
                StartCoroutine(Stun());
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter!");
        if (other.CompareTag("killzone"))
        {
            BlowArea killZone = other.GetComponent<BlowArea>();
            if (killZone.isfrozen())
            {
                initialPosition = transform.position;
                frozeen = true;
            }
            else
            {
                frozeen = false;
            }
            if (killZone.liftUp()>0)
            {
                wasAgentEnabled = agent.enabled;
                agent.enabled = false;
                isJumping = true;
                initialPosition = transform.position;
                Debug.Log("lify!");
                jumpSpeed = killZone.liftUp();
                jumpHeight = jumpSpeed/2;

            }
            if (killZone != null)
            {
                damageAmount = killZone.getDamdge();
            }
            Debug.Log(damageAmount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Continuously interacting with t!");
        //Debug.Log(other.tag);
        if (other.CompareTag("killzone"))
        {
            if (frozeen)
            {
                transform.position = initialPosition;
            }
            damageTimer += Time.deltaTime;
            //Debug.Log("Continuously interacting with the killzone!");
            if (damageTimer >= damageInterval)
            {
                Debug.Log("takeDamgae");
                GetDamage(damageAmount);
                damageTimer = 0f;
            }
        }
    }

    
    IEnumerator Stun()
    {
        agent.enabled = false;
        if (enemyShoot != null) enemyShoot.enabled = false;
        if (enemyAttack != null) enemyAttack.enabled = false;
        yield return new WaitForSeconds(2f);
        agent.enabled = true;
        if (enemyShoot != null) enemyShoot.enabled = true;
        if (enemyAttack != null) enemyAttack.enabled = true;
    }

}

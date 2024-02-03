using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlarrow : MonoBehaviour
{
    [SerializeField] private GameObject imageArrow; 
    private GameObject _arrow;
    private GameObject player;
    private Camera mainCamera;
    bool seen;
    [SerializeField] private float radarRadius = -200f;

    private void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        InstantiateImage();
    }
    private bool IsVisible()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return (screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1 && screenPoint.z > 0);
    }
    private void UpdateImagePosition()
    {
        if (_arrow != null && player != null)
        {
            Vector3 directionToTarget = player.transform.position - transform.position;

            float angleToTarget = Vector3.SignedAngle(player.transform.forward, directionToTarget, Vector3.up);
            Debug.Log(angleToTarget);
            angleToTarget = angleToTarget + 180f;

            _arrow.transform.rotation = Quaternion.Euler(0f, 0f, - angleToTarget);

            float radians = Mathf.Deg2Rad * (angleToTarget);
            //Debug.Log(radians);

            float x = Mathf.Sin(radians) * radarRadius;
            float y = Mathf.Cos(radians) * radarRadius;

            _arrow.transform.localPosition = new Vector3(-x, -y, 0f);

        }
    }

    private void InstantiateImage()
    {
        Vector3 positionOffset = new Vector3(0f, 0f, 0f);
        _arrow = Instantiate(imageArrow, positionOffset, Quaternion.identity);
        _arrow.transform.parent = FindObjectsWithTag("arrows").transform;
        _arrow.transform.localPosition = positionOffset;
    }
    public void Hide()
    {
        if (_arrow != null)
        {
            _arrow.SetActive(false);
        }
    }
    public void Show()
    {
        if (_arrow != null&&!seen)
        {
            _arrow.SetActive(true);
        }
    }

    public void ChangeColorToGray()
    {
        if (_arrow != null)
        {
            _arrow.GetComponent<Image>().color = Color.gray;
        }
    }

    public void ChangeColorToRed()
    {
        if (_arrow != null)
        {
            _arrow.GetComponent<Image>().color = Color.red;
        }
    }

    private GameObject FindObjectsWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        if (objectsWithTag.Length > 0)
        {
            return objectsWithTag[0];
        }

        return null;
    }
    private void Update()
    {
        if (IsVisible())
        {
            seen = true;
            //Debug.Log(seen);
            Hide();
        }
        else
        {
            seen = false;
        }
        UpdateImagePosition();


    }
    public void Die()
    {
        Destroy(_arrow);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    
    [SerializeField] private float _speed;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _powerupDuration;

    private Rigidbody _rigidBody;
    private Coroutine _powerupCoroutine;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal = a or left (-) & d or right (+)
        // Vertical = s or down (-) & w or up (+)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 horizontalDirection = horizontal * _camera.right;
        Vector3 verticalDirection = vertical * _camera.forward;
        horizontalDirection.y = 0;
        verticalDirection.y = 0;

        Vector3 movementDirection = horizontalDirection + verticalDirection;
        
        _rigidBody.velocity = movementDirection * _speed * Time.fixedDeltaTime;

        //Debug.Log("Horizontal: " + horizontal);
        //Debug.Log("Vertical: " + vertical);


    }

    public void PickPowerUp()
    {
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }

        //Debug.Log("Pick Power Up");
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }

    private IEnumerator StartPowerUp()
    {
        if (OnPowerUpStart != null)
        {
            OnPowerUpStart();
        }

        yield return new WaitForSeconds(_powerupDuration);

        if (OnPowerUpStop != null)
        {
            OnPowerUpStop();
        }
        
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    
    [SerializeField] private float _speed;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _powerupDuration;

    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private int _health;
    [SerializeField] private TMP_Text _healthText;

    private Rigidbody _rigidBody;
    private Coroutine _powerupCoroutine;
    private bool _isPowerUpActive;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        UpdateUI();

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
        Debug.Log(OnPowerUpStart);
        //Debug.Log("Pick Power Up");
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }
    private void UpdateUI()
    {
        _healthText.text = "Health: " + _health;
    }
    public void Dead()
    {
        _health -= 1;
        
        if (_health > 0)
        {
            transform.position = _respawnPoint.position;
        }
        else
        {
            _health = 0;
            SceneManager.LoadScene("LoseScreen");
        }
        UpdateUI();

    }
    private IEnumerator StartPowerUp()
    {
        _isPowerUpActive = true;

        if (OnPowerUpStart != null)
        {
            OnPowerUpStart();
        }

        yield return new WaitForSeconds(_powerupDuration);
        _isPowerUpActive = false;

        if (OnPowerUpStop != null)
        {
            OnPowerUpStop();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isPowerUpActive)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().Dead();
            }
        }
    }

}

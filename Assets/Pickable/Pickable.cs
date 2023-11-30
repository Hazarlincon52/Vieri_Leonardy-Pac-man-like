using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PickableType;

public class Pickable : MonoBehaviour
{
    [SerializeField] 
    public PickableType _pickableType;
    public Action<Pickable> OnPicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Trigger");
            //Debug.Log("Pickup: " + _pickableType);
            if (OnPicked != null)
            {
                OnPicked(this);
            }
        }
            
    }
}

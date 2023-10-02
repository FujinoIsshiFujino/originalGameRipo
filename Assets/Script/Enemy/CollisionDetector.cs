using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{

    [SerializeField] private TriggerEvent onTriggerStayEvent = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerEnterEvent = new TriggerEvent();
    private void OnTriggerStay(Collider other)
    {

        onTriggerStayEvent.Invoke(other);
        Debug.Log("other" + other);

    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent.Invoke(other);
    }

    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {

    }
}

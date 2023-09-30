using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{

    [SerializeField] private TriggerEvent onTriggerStayEvent = new TriggerEvent();
    private void OnTriggerStay(Collider other)
    {
        onTriggerStayEvent.Invoke(other);
    }

    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {

    }
}

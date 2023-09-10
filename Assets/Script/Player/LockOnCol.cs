using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCol : MonoBehaviour
{
     public bool isLockOn;

    // Start is called before the first frame update
    void Start()
    {
        isLockOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Enemy" && !isLockOn)
        {
            isLockOn = true;
        }
        else if (other.gameObject.tag != "Enemy" && isLockOn)
        {
            isLockOn = false;
        }
    }
}

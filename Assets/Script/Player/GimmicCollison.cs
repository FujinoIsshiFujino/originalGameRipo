using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicCollison : MonoBehaviour
{
    [SerializeField] GameObject Player;
    Vector3 playerForward;
    Vector3 objForward;
    public float angleInDegrees;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

   private void OnTriggerStay(Collider other) {
    
        

    //　とりあえず正面のみOK
    {
        if(other.gameObject.tag == "Treasure")
        {

            objForward = other.gameObject.transform.forward.normalized;
            playerForward = Player.transform.forward.normalized;

            float dotProduct = Vector3.Dot(objForward, playerForward);
            float angleInRadians = Mathf.Acos(dotProduct);
            angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            if(angleInDegrees >= 120 && angleInDegrees<=180)
            {
                Debug.Log("Treasure");
            }

        }
    }
   }
}

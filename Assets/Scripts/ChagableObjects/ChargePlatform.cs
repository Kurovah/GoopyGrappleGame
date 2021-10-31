using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlatform : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var chargeObj = other.gameObject.GetComponent<IChargeSource>();
        if (chargeObj != null && chargeObj.isCharged)
        {
            EventManager.current.OnPlatformCharged(id);
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
            
    }
}

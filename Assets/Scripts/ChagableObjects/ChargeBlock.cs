using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChargeBlock : BaseChargeObject
{
    // Update is called once per frame
    protected override void OnPlatformCharged(int _id)
    {
        base.OnPlatformCharged(_id);

        if (!charged) 
        {
            charged = true;
            Debug.Log("Charged!");
            transform.DOMoveY(transform.position.y + transform.localScale.y / 2, 0.1f);
        }
        
    }
}

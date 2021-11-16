using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChargeObject : MonoBehaviour
{
    public int id;
    public bool charged;
    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    protected virtual void Initialise()
    {
        EventManager.current.onPlatformCharged += OnPlatformCharged;
    }

    // Update is called once per frame
    protected virtual void OnPlatformCharged(int _id)
    {
        //if the id doesn't match skip call
        if (_id != id)
            return;
    }
}

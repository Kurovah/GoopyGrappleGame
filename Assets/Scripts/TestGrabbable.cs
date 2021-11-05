using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrabbable : BasePhysics, IGrabbable, IChargeSource, IPhysicsUser
{
    bool hasBeenThrown;
    public bool charged;
    public bool isCharged
    {
        get { return charged; }
        set { charged = value; }
    }

    public bool canBeGrabbed { get; set; }

    public void OnGrab()
    {
        
    }

    public void OnThrow(Vector3 _direction)
    {
        velocity = _direction;
        hasBeenThrown = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterRidgidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //put physics related updates in the fixed updates
    void Update()
    {
        if (hasBeenThrown)
        {
            var hits = CheckForCol(velocity, 0.2f);
            if (hits.Count > 0)
            {
                foreach (var item in hits)
                {
                    var hitInterface = item.transform.gameObject.GetComponent<IHurtable>();
                    if(hitInterface != null)
                    {
                        hitInterface.OnHit();
                    }
                }
            }
        }
    }

    public void SetVelocity(Vector3 _newVel)
    {
        Debug.Log("velocity set");
        velocity = _newVel;
    }
}

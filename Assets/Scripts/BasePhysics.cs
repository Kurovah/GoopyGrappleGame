using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePhysics : MonoBehaviour
{
    [Header("Base Physics")]
    public Vector3 velocity;
    public float gravityScale,friction,terminalVelocity;
    public bool grounded;
    public LayerMask groundedIgnoreMask;
    public Rigidbody2D characterRidgidBody;
    public bool physicsPaused;    


    public virtual void UpdatePhysics()
    {
        Physics2D.queriesHitTriggers = false;
        grounded = CheckForCol(Vector2.down, 0.1f).Count > 0;
        
        //side collision
        if(velocity.x != 0)
        {
            if (CheckForCol(Vector2.right * Mathf.Sign(velocity.x), 0.1f).Count > 0)
            {
                velocity.x = 0;
            }
        }

        //bumping the ceiling
        if (velocity.y > 0)
        {
            if (CheckForCol(Vector2.up, 0.1f).Count > 0)
            {
                velocity.y = 0;
            }
        }

        if (!grounded)
        {
            //apply gravity
            if(velocity.y > terminalVelocity)
            {
                velocity.y -= gravityScale;
            }
        } else
        {
            //stopping horizontally
            if(velocity.x != 0)
            {
                velocity.x -= Mathf.Sign(velocity.x) * friction;
            }
            //stopping vertically
            if(velocity.y < 0)
                velocity.y = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!physicsPaused)
        {
            UpdatePhysics();
            characterRidgidBody.velocity = velocity;
        }
    }

    protected List<RaycastHit2D> CheckForCol(Vector2 _dir, float _dis)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        characterRidgidBody.Cast(_dir, hits, _dis);
        return hits;
    }
}

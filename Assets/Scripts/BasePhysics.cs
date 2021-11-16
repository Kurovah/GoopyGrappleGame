using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePhysics : MonoBehaviour
{
    [Header("Base Physics")]
    public Vector3 velocity, secondaryVel;
    public float gravityScale,friction,terminalVelocity;
    public bool grounded;
    public LayerMask groundedIgnoreMask;
    public Rigidbody2D characterRidgidBody;
    public bool physicsPaused;    


    public virtual void UpdatePhysics()
    {
        Physics2D.queriesHitTriggers = false;

        List<RaycastHit2D> hits;
        GroundCheck(out hits);
        var groundedres = CheckForCol(Vector2.down, 0.01f);
        grounded = groundedres.Count > 0;

        //side collision
        if (velocity.x != 0)
        {
            if (CheckForCol(Vector2.right * Mathf.Sign(velocity.x), 0.1f).Count > 0)
            {
                velocity.x = 0;
            }
        }

        //bumping the ceiling
        if (velocity.y > 0)
        {
            if (CheckForCol(Vector2.up, 0.01f).Count > 0)
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
            if (velocity.x != 0)
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
            characterRidgidBody.velocity = velocity + secondaryVel;
        }
    }

    protected List<RaycastHit2D> CheckForCol(Vector2 _dir, float _dis)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        characterRidgidBody.Cast(_dir, hits, _dis);
        return hits;
    }

    protected void GroundCheck(out List<RaycastHit2D> _hits)
    {
        _hits = new List<RaycastHit2D>();
        _hits.Add(Physics2D.Raycast(transform.position, Vector2.down, 0.1f, ~groundedIgnoreMask));
        _hits.Add(Physics2D.Raycast(transform.position + Vector3.right * 0.25f, Vector2.down, 0.1f, ~groundedIgnoreMask));
        _hits.Add(Physics2D.Raycast(transform.position + Vector3.left * 0.25f, Vector2.down, 0.1f, ~groundedIgnoreMask));


        

        grounded = _hits.Count > 0;
    }
}

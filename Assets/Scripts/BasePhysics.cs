using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePhysics : MonoBehaviour
{
    [Header("Base Physics")]
    public Vector3 velocity;
    public float gravityScale, friction, terminalVelocity;
    //collision variables
    public Vector2 wallCollisionSize, floorCollisionSize;
    public Vector2 rightOffset, leftOffset;
    public bool grounded;
    public LayerMask groundMask;
    public Rigidbody2D characterRidgidBody;
    public bool physicsPaused;    


    public virtual void UpdatePhysics()
    {
        Physics2D.queriesHitTriggers = false;
        GroundCheck();
        WallCheck();
    }

    private void FixedUpdate()
    {
        if (!physicsPaused)
        {
            UpdatePhysics();
            characterRidgidBody.velocity = new Vector2(velocity.x, characterRidgidBody.velocity.y);
        }
    }

    protected List<RaycastHit2D> CheckForCol(Vector2 _dir, float _dis)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        characterRidgidBody.Cast(_dir, hits, _dis);
        return hits;
    }

    protected void WallCheck()
    {
        if ((Physics2D.OverlapBox((Vector2)transform.position + leftOffset, wallCollisionSize, 0, groundMask) && velocity.x < 0) || 
            (Physics2D.OverlapBox((Vector2)transform.position + rightOffset, wallCollisionSize, 0, groundMask) && velocity.x > 0)) {
            velocity.x = 0;
        }
    }

    protected void GroundCheck()
    {
        grounded = Physics2D.OverlapBox(transform.position, floorCollisionSize, 0, groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position, floorCollisionSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftOffset, wallCollisionSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightOffset, wallCollisionSize);
    }
}

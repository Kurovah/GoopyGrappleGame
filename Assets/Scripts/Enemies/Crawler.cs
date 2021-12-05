using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crawler : BaseEnemy,IGrabbable
{
    Sequence currentSequence;
    public LayerMask ignoreLayer;
    public int facing = 1;
    float stunTime = 0;
    Coroutine stun;
    enum CrawlerStates
    {
        normal,
        alerted,
        attacking,
        stunned,
    }

    public float crawlVel;
    public bool canBeGrabbed { get; set; }

    protected override void InitStateFunctions()
    {
        NormalStateAction = NormalState;
        AlertStateAction = AlertState;
        ReactStateAction = ReactState;
        StunnedStateAction = StunnedState;
    }

    #region state functions
    void NormalState()
    {
        velocity.x = facing * crawlVel;

        var groundDeectect = Physics2D.Raycast(transform.position + Vector3.right * .5f * facing, Vector2.down, 0.5f, ~ignoreLayer);
        //turn around at a wall or at a ledge
        if (!groundDeectect.collider || CheckForCol(Vector2.right * facing, 0.2f).Count > 0)
        {
            facing *= -1;
        }


        //check for player
        if(Physics2D.Raycast(transform.position + Vector3.up * 0.5f,Vector2.right * facing, 5, LayerMask.GetMask("Player")))
        {
            //if player found charge
            if(!GameManager.current.player.invulnerable)
                currentState = EnemyStates.reacting;
        }
    }

    void AlertState()
    {
        currentState = EnemyStates.reacting;
    }

    void ReactState()
    {
        velocity.x = facing * crawlVel * 2;
        var groundDeectect = Physics2D.Raycast(transform.position + Vector3.right * .75f * facing, Vector2.down, 0.5f, ~ignoreLayer);
        //turn around at a wall or at a ledge
        if (!groundDeectect.collider || CheckForCol(Vector2.right * facing, 0.2f).Count > 0)
        {
            currentState = EnemyStates.normal;
        }


        var hit = Physics2D.OverlapCircle(transform.position + Vector3.up / 2, 1, LayerMask.GetMask("Player"));
        if(hit != null && !GameManager.current.player.invulnerable)
        {
            GameManager.current.player.GetHurt(_knockBack:new Vector2(facing * 10,10));
        }
    }

    void StunnedState()
    {
        if(stun == null)
            stun = StartCoroutine(ResetStun());
    }
    #endregion


    private void OnDrawGizmos()
    {
        Ray r = new Ray(transform.position + Vector3.right * .5f * Mathf.Sign(velocity.x), Vector2.down);
        Gizmos.DrawRay(r);
    }

    public void OnGrab()
    {
        throw new System.NotImplementedException();
    }

    public void OnThrow(Vector3 _direction)
    {
       
        velocity = _direction;
        currentState = EnemyStates.stunned;
    }

    public void SetCurrentSequence(Sequence _newSequence)
    {
        currentSequence.Kill();
        currentSequence = _newSequence;
    }

    public IEnumerator ResetStun()
    {
        Debug.Log("Bleh");
        yield return new WaitForSeconds(5);       
        currentState = EnemyStates.normal;
    }
}

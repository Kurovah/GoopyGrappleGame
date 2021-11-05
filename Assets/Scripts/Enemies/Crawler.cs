using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crawler : BaseEnemy,IGrabbable
{
    Sequence currentSequence;
    public LayerMask ignoreLayer;
    public int facing = 1;
    float stunTime;
    enum CrawlerStates
    {
        normal,
        alerted,
        attacking,
        stunned,
    }

    CrawlerStates currentState = CrawlerStates.normal;
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
            currentState = CrawlerStates.alerted;
        }
    }

    void AlertState()
    {
        if(!Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector2.right * facing, 5, LayerMask.GetMask("Player")))
        {
            currentState = CrawlerStates.normal;
        }
    }

    void ReactState()
    {
        var groundDeectect = Physics2D.Raycast(transform.position + Vector3.right * .5f * facing, Vector2.down, 0.5f, ~ignoreLayer);
        //turn around at a wall or at a ledge
        if (!groundDeectect.collider || CheckForCol(Vector2.right * facing, 0.2f).Count > 0)
        {
            currentState = CrawlerStates.normal;
        }
    }

    void StunnedState()
    {

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
        currentState = CrawlerStates.stunned;
        stunTime = 5;
    }

    public void SetCurrentSequence(Sequence _newSequence)
    {
        currentSequence.Kill();
        currentSequence = _newSequence;
    }
}

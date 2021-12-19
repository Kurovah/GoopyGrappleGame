using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBehaviour : BasePhysics, IChargeSource
{
    [Header("Player Exclusive")]
    public bool canGrab;
    public bool invulnerable;
    bool charged, usingSwingVel;
    public float moveSpeed;
    public float facing = 1;
    public float jumpPow;
    public int maxHp, hp;

    public Vector3 shotDir;
    public GameObject grappleHand;
    public GameObject grabbedItem;
    public Sequence currentSequence;
    public IEnumerator currentCoroutine;
    public LineRenderer line;
    public SpriteRenderer bodySprite;
    public bool isCharged { get { return charged; } set { charged = value; } }
    public enum PlayerStates
    {
        normal,
        grabbing,
        swinging,
        throwing,
        stunned,
        dead
    }

    public PlayerStates currentState = PlayerStates.normal;

    

    // Start is called before the first frame update
    void Awake()
    {
        maxHp = 5;
        hp = 5;
        canGrab = true;
        characterRidgidBody = GetComponent<Rigidbody2D>();

        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position + Vector3.up / 2); line.SetPosition(1, grappleHand.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        bodySprite.flipX = facing > 0;
        line.SetPosition(0, transform.position + Vector3.up/2); line.SetPosition(1, grappleHand.transform.position);
        line.enabled = grappleHand.activeSelf;

        if (Input.GetKeyDown(KeyCode.Q)) { GetHurt(); }

        switch (currentState)
        {
            case PlayerStates.normal:
                NormalState();
                break;
            case PlayerStates.grabbing:
                GrabState();
                break;
            case PlayerStates.swinging:
                break;
            case PlayerStates.throwing:
                break;
            case PlayerStates.stunned:
                break;
            case PlayerStates.dead:
                break;
        }

       
        
    }

    

    #region state functions
    void NormalState()
    {
        //setting the shot vector
        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            //if no directional input shot vector is forwards by default
            shotDir = Vector3.right * facing;
        } else
        {
            shotDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        velocity.x = Mathf.Lerp(velocity.x,Input.GetAxisRaw("Horizontal") * moveSpeed, 0.1f);
        
        

        if (Input.GetAxisRaw("Horizontal") != 0)
            facing = Input.GetAxisRaw("Horizontal");

        
        //jumping
        if (grounded)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                characterRidgidBody.velocity = new Vector2(characterRidgidBody.velocity.x, 0);
                characterRidgidBody.velocity += Vector2.up * jumpPow;
            }

        } else
        {
            //double jump with grabbed object
            if (grabbedItem != null && Input.GetKeyDown(KeyCode.J))
            {
                grabbedItem.SetActive(true);
                grabbedItem.transform.position = transform.position + Vector3.up / 2 + Vector3.down * 1.2f;
                grabbedItem.gameObject.GetComponent<IGrabbable>().OnThrow(Vector3.down);
                characterRidgidBody.velocity = new Vector2(characterRidgidBody.velocity.x, 0);
                characterRidgidBody.velocity += Vector2.up * jumpPow;
                grabbedItem = null;
                charged = false;
            }
        }

        //grabbing attempt (also do a little hop)
        if (Input.GetKeyDown(KeyCode.K) && canGrab)
        {
            if (grabbedItem == null)
            {
                currentState = PlayerStates.grabbing;
                SetCurrentSequence(GrabSequence());
            } else
            {
                if (shotDir.normalized == Vector3.down)
                {
                    characterRidgidBody.velocity = new Vector2(characterRidgidBody.velocity.x, 0);
                    characterRidgidBody.velocity += Vector2.up * jumpPow;
                }

                grabbedItem.GetComponent<IGrabbable>().OnThrow(shotDir * 10);
                grabbedItem.transform.position = transform.position + Vector3.up / 2 + shotDir * 1.2f;
                grabbedItem.SetActive(true);
                grabbedItem = null;
                charged = false;


            }
            
        }
    }
    #endregion

    void GrabState()
    {
        Physics2D.queriesHitTriggers = true;
        Collider2D[] hits = Physics2D.OverlapCircleAll(grappleHand.transform.position, .8f);
        if(hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                var grabInter = hit.gameObject.GetComponent<IGrabbable>();
                var swingInter = hit.gameObject.GetComponent<ISwingable>();
                //make sure no other items can be grabbed if one is found
                //for a grabbable
                if (grabInter != null)
                {
                    
                    grabbedItem = hit.gameObject;
                    grabbedItem.SetActive(false);

                    //becoming charged
                    var c = grabbedItem.GetComponent<IChargeSource>();
                    if (c != null && c.isCharged)
                    {
                        isCharged = true;
                    }
                    break;
                }
                    
                //for a swingable
                if(swingInter != null)
                {
                    SetCurrentSequence(SwingSequence(hit.gameObject.transform.position));
                    break;
                }
            }
        }
        Physics2D.queriesHitTriggers = false;
    }

    Sequence GrabSequence()
    {
        //enable hand send it out send it in
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => {grappleHand.SetActive(true); if (!grounded) { velocity.y = 2; } canGrab = false; });
        s.Append(grappleHand.transform.DOLocalMove(shotDir*1.5f, 0.1f));
        s.Append(grappleHand.transform.DOLocalMove(Vector3.zero, 0.1f));
        s.AppendCallback(() => {grappleHand.SetActive(false); currentState = PlayerStates.normal; canGrab = true; });
        return s;
    }

    Vector3 grapplePoint;
    Sequence SwingSequence(Vector3 _grapplePoint)
    {
        grapplePoint = _grapplePoint;
        float side = Mathf.Sign(transform.position.x - grapplePoint.x);
        if(side == 0) { side = facing; }
        float startingAngle = GetStartAngle(grapplePoint, transform.position) * Mathf.Rad2Deg;
        if(startingAngle < 0)
        {
            startingAngle += 360;
        }
        physicsPaused = true;
        characterRidgidBody.Sleep();
        facing = side;
        placeChar(startingAngle);
        Debug.Log("Started at:" + startingAngle);
        //move to correct position
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => { DOVirtual.Float(startingAngle, 180 + side * 50, 0.2f, placeChar).SetEase(Ease.Linear); });
        s.AppendCallback(() => { physicsPaused = false; characterRidgidBody.velocity = new Vector2(facing * 15, jumpPow); characterRidgidBody.WakeUp(); currentState = PlayerStates.normal;});
        s.Append(grappleHand.transform.DOLocalMove(Vector3.zero, 0.1f));
        s.AppendCallback(() => { grappleHand.SetActive(false); canGrab = true;});
        return s;
    }
    
    float GetStartAngle(Vector3 from, Vector3 to)
    {
        return Mathf.Atan2(
            to.x - from.x,
            to.y - from.y
            );
    }

    void placeChar(float _angle)
    {
        float dis = 2;
        grappleHand.transform.position = grapplePoint;
        transform.position = new Vector3(grapplePoint.x + Mathf.Sin(_angle * Mathf.Deg2Rad) * dis, grapplePoint.y + Mathf.Cos(_angle * Mathf.Deg2Rad) * dis);
    }

    public void GetHurt(int _damage = 1, Vector2 _knockBack = default)
    {
       hp -= _damage;
       velocity.x = _knockBack.x;
       velocity.y = _knockBack.y;
       SetCurrentCoroutine(InvTime());
       EventManager.current.OnPlayerHurt();
    }

    public void SetCurrentSequence(Sequence _newSequence)
    {
        currentSequence.Kill();
        currentSequence = _newSequence;
    }

    public void SetCurrentCoroutine(IEnumerator _newCoroutine)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = _newCoroutine;
    }

    IEnumerator InvTime(float Time = 1.5f)
    {
        invulnerable = true;
        yield return new WaitForSeconds(Time);
        invulnerable = false;
    }
}

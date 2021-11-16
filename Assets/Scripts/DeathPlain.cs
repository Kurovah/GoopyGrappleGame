using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeathPlain : MonoBehaviour
{
    public Transform p1, p2;
    public Rect colRect = new Rect();
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

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Fallen");
            FallSequence();
        }
    }

    Sequence FallSequence()
    {
        Vector3 p = FindClosest();
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => { GameManager.current.player.GetHurt(); });
        s.AppendInterval(1);
        s.AppendCallback(() => { GameManager.current.player.transform.position = p; });
        return s;
    }

    Vector3 FindClosest()
    {
        if (Vector3.Distance(GameManager.current.player.transform.position, p1.position) < Vector3.Distance(GameManager.current.player.transform.position, p2.position))
            return p1.position;

        return p2.position;
    }
    
}

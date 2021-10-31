using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ConfinedCam : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public PolygonCollider2D roomCamBounds;
    // Start is called before the first frame update
    void Start()
    {
        cam.Follow = FindObjectOfType<PlayerBehaviour>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            cam.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cam.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerBehaviour player;
    public static GameManager current;
    // Start is called before the first frame update
    void Awake()
    {
        current = this;
        player = FindObjectOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

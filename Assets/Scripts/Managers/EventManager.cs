using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;
    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public event Action<int> onPlatformCharged;
    public void OnPlatformCharged(int _platformId)
    {
        onPlatformCharged?.Invoke(_platformId);
    }

    public event Action<int> onPlatformUnharged;
    public void OnPlatformUnharged(int _platformId)
    {
        onPlatformUnharged?.Invoke(_platformId);
    }

    public event Action onPlayerHurt;
    public void OnPlayerHurt()
    {
        onPlayerHurt?.Invoke();
    }

    public event Action onGamePaused;
    public void OnGamePaused()
    {
        onGamePaused?.Invoke();
    }
}

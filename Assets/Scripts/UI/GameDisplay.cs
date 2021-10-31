using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameDisplay : MonoBehaviour
{
    public GameObject hpPipPrefab;
    public Transform hpBar;
    public Transform hpBarArea;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.current.player.maxHp; i++)
        {
            Instantiate(hpPipPrefab, hpBarArea);
        }
        UpdateHealth();
        EventManager.current.onPlayerHurt += ShakeBar;
        EventManager.current.onPlayerHurt += UpdateHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShakeBar()
    {
        hpBar.DOShakePosition(0.5f, 5f, 10);
    }

    void UpdateHealth()
    {
        for (int i = 0; i < GameManager.current.player.maxHp; i++)
        {
            if(i < GameManager.current.player.hp)
            {
                Debug.Log(i);
                hpBarArea.GetChild(i).GetComponent<Image>().color = Color.red;
            } else
            {
                hpBarArea.GetChild(i).GetComponent<Image>().color = Color.grey;
            }
        }
    }
}

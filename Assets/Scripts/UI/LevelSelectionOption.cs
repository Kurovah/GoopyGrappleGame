using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelSelectionOption : MonoBehaviour
{
    bool highlighted;
    RectTransform r;
    public string levelName;
    public Transform mapPoint;
    AreaDataVeiwer a;
    public LevelData levelData = new LevelData();
    // Start is called before the first frame update
    void Awake()
    {
        a = FindObjectOfType<AreaDataVeiwer>();
        r = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHighlighted(bool _highlighted)
    {
        if (_highlighted)
        {
            a.SetData(levelData);
            DOTween.To(() => r.sizeDelta, x => r.sizeDelta = x, new Vector2(199,55), 0.2f);

        } else
        {
            DOTween.To(() => r.sizeDelta, x => r.sizeDelta = x, new Vector2(188,50), 0.2f);
        }
    }
}

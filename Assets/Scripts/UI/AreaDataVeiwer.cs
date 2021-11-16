using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AreaDataVeiwer : MonoBehaviour
{
    public Image image;
    public Text timeText;
    public void SetData(LevelData _data)
    {
        SlideIn();
        if (_data.levelImage != null)
            image.sprite = _data.levelImage;
    }

    Sequence SlideIn()
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => { GetComponent<RectTransform>().anchoredPosition = new Vector2(250, 0); });
        s.Append(GetComponent<RectTransform>().DOAnchorPosX(0,0.2f).SetEase(Ease.OutBack));
        return s;
    }
}

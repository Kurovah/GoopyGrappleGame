using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelSelectorBehaviour : MonoBehaviour
{
    LevelSelectionOption currentOption;
    public float pauseFrames;
    bool canScroll = true;
    float scrollBuffer = 60;
    // Start is called before the first frame update
    void Start()
    {
        SetOption(0);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S) && currentOption.transform.GetSiblingIndex() < transform.childCount-1)
        //    SetOption(currentOption.transform.GetSiblingIndex() + 1);

        //if (Input.GetKeyDown(KeyCode.W) && currentOption.transform.GetSiblingIndex() > 0)
        //    SetOption(currentOption.transform.GetSiblingIndex() - 1);

        int scroll = (int)Input.GetAxisRaw("Vertical");
        Debug.Log("Scroll:" + scroll);

        if(scroll == 0)
        {
            if(scrollBuffer != 0)
            {
                scrollBuffer = pauseFrames;
            }
        } else
        {
            //this is so  that you scroll on first press
            if (scrollBuffer == pauseFrames)
            {
                int newIndex = currentOption.transform.GetSiblingIndex() - scroll;
                newIndex = Mathf.Clamp(newIndex, 0, transform.childCount-1);
                SetOption(newIndex);
                
            }
                
            if (scrollBuffer > 0)
            {
                scrollBuffer -= 1;
            } else
            {
                int newIndex = currentOption.transform.GetSiblingIndex() - scroll;
                newIndex = Mathf.Clamp(newIndex, 0, transform.childCount-1);
                SetOption(newIndex);
                scrollBuffer = pauseFrames/4;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene(currentOption.levelName);
        }
    }

    void SetOption(int index)
    {
        foreach(Transform t in transform)
        {
            t.gameObject.GetComponent<LevelSelectionOption>().SetHighlighted(false);
        }
        currentOption = transform.GetChild(index).GetComponent<LevelSelectionOption>();
        currentOption.SetHighlighted(true);
    }



    IEnumerable PauseScroll()
    {
        Debug.Log("Paused");
        canScroll = false;
        yield return new WaitForSeconds(pauseFrames);
        canScroll = true;
        Debug.Log("unPaused");
    }
}

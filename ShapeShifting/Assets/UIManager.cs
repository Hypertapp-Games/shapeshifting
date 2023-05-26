using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ButtonPnl;
    public GameObject RePlayPnl;
    public List<AutoScrolling> autoScrollings = new List<AutoScrolling>();
    public float autoScrollSpeed = 3000;
    public float autoScrollTime = 1;
    public float augmentTime = 0.5f;
    public float autoScrollEndSmooth;
    void Start()
    {
        ButtonPnl.gameObject.SetActive(true);
        RePlayPnl.gameObject.SetActive(false);
        
    }

    private void OnEnable()
    {
        autoScrollings = ButtonPnl.GetComponentsInChildren<AutoScrolling>().ToList();
    }

    public void AutoScroll(string vehicleName, int index)
    {
        var ats = autoScrollings[index];
        ats.speed = autoScrollSpeed;
        ats.timeToRoll = autoScrollTime;
        autoScrollTime += augmentTime;
        ats.endSmooth = autoScrollEndSmooth;
        for (int i = 0; i < ats.gameObject.transform.childCount; i++)
        {
            if (ats.gameObject.transform.GetChild(i).name == vehicleName)
            {
                ats.valueSelected = ats.gameObject.transform.GetChild(i).gameObject;
                ats.isRoll = true;
                StartCoroutine(ats.Scroll());
            }
        }
        // for (int i = 0; i < sprites.Count; i++)
        // {
        //     Debug.Log(sprites[i].name);
        //     if (sprites[i].name == vehicleName)
        //     {
        //         Debug.Log(1);
        //         VehicleIcon[index].sprite = sprites[i];
        //     }
        // }
    }

    public void EndGame()
    {
        ButtonPnl.gameObject.SetActive(false);
        RePlayPnl.gameObject.SetActive(true);
    }

    public void RePlay()
    {
        SceneManager.LoadScene(0);
    }
}

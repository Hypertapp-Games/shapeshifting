using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ButtonPnl;
    public GameObject RePlayPnl;
    public GameObject SliderPnl;
    
    
    List<AutoScrolling> autoScrollings = new List<AutoScrolling>();
    [Header("ScrollVehicle")]
    public float autoScrollSpeed = 3000;
    public float autoScrollTime = 1;
    public float augmentTime = 0.5f;
    public float autoScrollEndSmooth;
    
    [Header("Traveled")]
    public List<Slider> sliders = new List<Slider>();
    GameManager gameManager;
    public TMP_Text currentLevel;
    public TMP_Text nextLevel;
    
    [Header("FPS")]
    public TMP_Text fps;

    [Header("BonusCoin")]
    public TMP_Text ordinal;
    public TMP_Text coin;
    public RectTransform buttons;
    float Value;
    bool valueg = true;
    bool valuel = false;
    int xcoin = 1;
    public TMP_Text cointext;
    public GameObject GetBonusCoinBtn;
    public GameObject NoGetBonusCoinBtn;
    void Start()
    {
        ButtonPnl.gameObject.SetActive(true);
        SliderPnl.gameObject.SetActive(true);
        RePlayPnl.gameObject.SetActive(false);
        gameManager = gameObject.GetComponent<GameManager>();
        currentLevel.text = gameManager.level.ToString();
        nextLevel.text = (gameManager.level + 1).ToString();
        StartCoroutine(ShowFPS());
    }

    private void OnEnable()
    {
        autoScrollings = ButtonPnl.GetComponentsInChildren<AutoScrolling>().ToList();
    }
    public IEnumerator ShowFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            var fr = (int)(1f / Time.unscaledDeltaTime);
            fps.text = fr.ToString();

        }
    }

    private void Update()
    {
        for (int i = 0; i < gameManager.allPlayer.Count; i++)
        {
            sliders[i].value = gameManager.allPlayer[i].distanceTraveled / gameManager.allPlayer[i].distanceX;
        }
    }

    public void SicleSlider()
    {
        if (Value >= 34)
        {
            valueg = false;
            valuel = true;
        }
        
        if (Value <= 0)
        {
            valueg = true;
            valuel = false;
        }
        float amount = Value / 100.0f * 180.0f / 360;
        float angle = amount * 360;
        buttons.localEulerAngles = new Vector3(0, 0, -angle);
        if (valueg && !valuel)
        {
            Value += Time.deltaTime * 40;
        }
        else if(!valueg && valuel)
        {
            Value -= Time.deltaTime * 40;
        }

        if (Value <= 1)
        {
            xcoin = 2;
        }
        else if(Value <=6)
        {
            xcoin = 3;
        }
        else if(Value <= 12)
        {
            xcoin = 4;
        }
        else if(Value <= 21.5f)
        {
            xcoin = 5;
        }
        else if(Value <=27.5f)
        {
            xcoin = 4;
        }
        else if(Value <= 31.5f)
        {
            xcoin = 3;
        }
        else
        {
            xcoin = 2;
        }

        cointext.text = (gameManager.coinGetInThisLevel * xcoin).ToString();
        //0-1:x2 ; 1-5:x3  ;
    }

    public IEnumerator _SicleSlider()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            SicleSlider();
        }
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
    }

    public void EndGame(int ord)
    {
        ButtonPnl.gameObject.SetActive(false);
        SliderPnl.gameObject.SetActive(false);
        RePlayPnl.gameObject.SetActive(true);
        ordinal.text = AddOrdinal(ord);
    }
    public string AddOrdinal(int num)
    {
        if( num <= 0 ) return num.ToString();

        switch(num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }
    
        switch(num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }

    public void GetBonusCoin()
    {
        Debug.Log("GetBonusCoin");
        PlayerPrefs.SetInt("Coin", gameManager.coin + gameManager.coinGetInThisLevel*xcoin);
        StartCoroutine(RePlay());
    }

    public void NoGetBonusCoin()
    {
        Debug.Log("NoGetBonusCoin");
        PlayerPrefs.SetInt("Coin", gameManager.coin + gameManager.coinGetInThisLevel);
        StartCoroutine(RePlay());
    }

    public IEnumerator RePlay()
    {
        valueg = false;
        valuel = false;
        GetBonusCoinBtn.gameObject.SetActive(false);
        NoGetBonusCoinBtn.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    public void Upgrade()
    {
        SceneManager.LoadScene(1);
    }
}

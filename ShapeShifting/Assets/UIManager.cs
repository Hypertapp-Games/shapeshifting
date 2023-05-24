using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ButtonPnl;
    public GameObject RePlayPnl;
    void Start()
    {
        ButtonPnl.gameObject.SetActive(true);
        RePlayPnl.gameObject.SetActive(false);
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarryCoins : MonoBehaviour
{
    public float currentY = 1;
    private bool getCoin = false;
    private bool upgradeCoin;
    public GameObject coin1;
    public GameObject ListCoin;
    public List<GameObject> coint = new List<GameObject>();
    public List<GameObject> cointemp = new List<GameObject>();
    public int coin;
    public TMP_Text coinText;

    private void Start()
    {
        coin = PlayerPrefs.GetInt("Coin");
        coinText.text = coin.ToString();
    }

    public void SetUp()
    {
        for (int i = 0; i < ListCoin.transform.childCount; i++)
        {
            cointemp.Add(ListCoin.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        for (int i = 0; i < coint.Count; i++)
        {
            coint[i].transform.position = new Vector3(coin1.transform.localToWorldMatrix.GetPosition().x,coint[i].transform.localToWorldMatrix.GetPosition().y,coin1.transform.localToWorldMatrix.GetPosition().z);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "ListCoin")
        {
            getCoin = true;
            StartCoroutine(GetCoin());
        }
        if (other.gameObject.name == "UpgradePoint" && other.gameObject.GetComponent<UpgradePoint>().CanUpgrade)
        {
            upgradeCoin = true;
            StartCoroutine(UpgradeCoin(other.gameObject));
        }
        //
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "ListCoin")
        {
            getCoin = false;
        }
        if (other.gameObject.name == "UpgradePoint")
        {
            upgradeCoin = false;
        }
    }
    IEnumerator GetCoin()
    {
        int i = cointemp.Count-1;
        while (getCoin && i >= 0 )
        {
            var co = cointemp[i];
            cointemp.Remove(co);
            StartCoroutine(0.2f.Tweeng((p) => co.gameObject.transform.position = p , co.transform.position,  coin1.transform.localToWorldMatrix.GetPosition()));
            yield return new WaitForSeconds(0.2f);
            coint.Add(co);
            co.transform.position = new Vector3(coin1.transform.localToWorldMatrix.GetPosition().x,currentY,coin1.transform.localToWorldMatrix.GetPosition().z);
            currentY += 0.1f;
            i--;
        }
    }
    IEnumerator UpgradeCoin(GameObject obj)
    {
        int i = coint.Count-1;
        while (upgradeCoin && i >= 0 && obj.GetComponent<UpgradePoint>().CanUpgrade)
        {
            var co = coint[i];
            coint.Remove(co);
            co.transform.position = new Vector3(coin1.transform.localToWorldMatrix.GetPosition().x,coin1.transform.localToWorldMatrix.GetPosition().y,coin1.transform.localToWorldMatrix.GetPosition().z);
            StartCoroutine(0.2f.Tweeng((p) => co.gameObject.transform.position = p , co.transform.position,  obj.transform.localToWorldMatrix.GetPosition()));
            yield return new WaitForSeconds(0.2f);
            coin -= 100;
            obj.GetComponent<UpgradePoint>().Upgrade();
            PlayerPrefs.SetInt("Coin",coin);
            coinText.text = coin.ToString();
            Destroy(co);
            currentY -= 0.1f;
            i--;
        }
    }
    public void MainScene()
    {
        SceneManager.LoadScene(0);
    }
}

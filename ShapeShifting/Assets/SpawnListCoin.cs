using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnListCoin : MonoBehaviour
{
    public int coin;
    public GameObject coinObject;
    public Transform ListCoin;
    public float currentY = 0.3f;

    public int temp;

    public CarryCoins carryCoins;
    // Start is called before the first frame update
    void Start()
    {
        coin = PlayerPrefs.GetInt("Coin");
        InstanceCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstanceCoin()
    {
        temp = coin / 100;
        while (temp > 0 )
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var co = Instantiate(coinObject);
                var pos = transform.GetChild(i).transform.localToWorldMatrix.GetPosition();
                co.transform.position = new Vector3(pos.x, currentY, pos.z);
                co.transform.parent = ListCoin;
                temp--;
            }

            currentY += 0.1f;
        }
        carryCoins.SetUp();
        
    }
}

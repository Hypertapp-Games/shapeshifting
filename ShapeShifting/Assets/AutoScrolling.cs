using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrolling : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public bool isRoll = false;
    private float distance;
    private float maxDistance;
    private List<GameObject> CheckList = new List<GameObject>();
    private float valueSelectedPos;
    private float time;
    public GameObject valueSelected;
    public float timeToRoll;
    public float endSmooth = 0.1f;
    void Start()
    {
        SetUp();
        StartCoroutine(Scroll());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Scroll()
    {
        while (isRoll == true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
            for (int i = 0; i < CheckList.Count; i++)
            {
                CheckList[i].transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
            }

            if (CheckList[0].transform.position.y <= maxDistance)
            {
                CheckList[0].transform.position = new Vector3(CheckList[0].transform.position.x, CheckList[CheckList.Count-1].transform.position.y+distance,
                    CheckList[0].transform.position.z);
                var temp = CheckList[0].gameObject;
                CheckList.Remove(CheckList[0]);
                CheckList.Add(temp);
            }

            if (time >= timeToRoll)
            {
                if (valueSelected.transform.position.y >= valueSelectedPos)
                {
                    EndRoll();
                    isRoll = false;
                }
            }
        }
    }

    public void EndRoll()
    {
        var blap = valueSelected.transform.position.y - valueSelectedPos;
        StartCoroutine(endSmooth.Tweeng((p) => transform.position = p , transform.position,  new Vector3(transform.position.x, transform.position.y-blap, transform.position.z)));
    }

    public void SetUp()
    {
        // distance = gameObject.transform.GetChild(0).transform.position.y -
        //            gameObject.transform.GetChild(1).transform.position.y;
        // maxDistance = gameObject.transform.GetChild(0).transform.position.y + distance;
        for (int i = 0; i < transform.childCount; i++)
        {
            CheckList.Add(gameObject.transform.GetChild(i).gameObject);
        }
        
        valueSelectedPos = CheckList[1].gameObject.transform.position.y;
        CheckList.Reverse();
        distance = CheckList[1].transform.position.y - CheckList[0].transform.position.y;
        maxDistance = CheckList[0].transform.position.y - distance;
    }
}

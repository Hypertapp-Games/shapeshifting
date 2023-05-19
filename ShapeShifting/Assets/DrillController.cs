using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            other.gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
            //StartCoroutine(0.1f.Tweeng((p) => other.gameObject.transform.localScale = p , other.gameObject.transform.localScale,  new Vector3(3f, 3f, 3f)));
            //StartCoroutine(2f.Tweeng((p) => other.gameObject.transform.position = p , other.gameObject.transform.position, new Vector3(1000, 1000, 1000)));
            other.gameObject.transform.position = new Vector3(1000, 1000, 1000);
        }
    }
}

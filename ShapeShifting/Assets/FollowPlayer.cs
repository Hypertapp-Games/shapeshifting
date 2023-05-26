using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public GameObject player;
    private Vector3 offset = new Vector3();

    void Start()
    {
        offset = gameObject.transform.position - player.transform.position;
    }
    private void Update()
    {
        if(player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}

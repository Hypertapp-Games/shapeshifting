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
        float dist = Vector3.Distance(player.transform.localToWorldMatrix.GetPosition() , gameObject.transform.position );
        Vector3 forward = player.transform.localToWorldMatrix.GetPosition() - gameObject.transform.position ;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, forward, out hit, dist-2 ))
        {
            Debug.Log(hit.transform.name);
            Destroy(hit.transform.parent.gameObject);
        }
    }
    private void OnDrawGizmosSelected()
    {
        float dist = Vector3.Distance(player.transform.localToWorldMatrix.GetPosition() , gameObject.transform.position );
        Vector3 forward = player.transform.localToWorldMatrix.GetPosition() - gameObject.transform.position ;
        Gizmos.color = Color.red;
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + forward );
    }
}

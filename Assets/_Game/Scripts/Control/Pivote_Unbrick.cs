using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivote_Unbrick : MonoBehaviour
{
    private bool isCollect = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollect)
        {
            isCollect = true;
            GetComponent<MeshRenderer>().enabled = true;
            other.GetComponent<Player>().RemoveBrick();
        }
    }
}

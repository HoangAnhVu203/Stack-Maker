using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pivote_Brick : MonoBehaviour
{
    private bool isCollect = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollect)
        {
            isCollect = true;
            gameObject.SetActive(false);
            other.GetComponent<Player>().AddBrick();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivote_Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Clear();
                LevelManager.Instance.OnFinish();
            }

        }
    }
}

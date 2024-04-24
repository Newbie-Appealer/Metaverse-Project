using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance._players.GetChild(0).GetComponent<PlayerController>()._isCrashed = true;
        GameManager.Instance._players.GetChild(0).GetComponent<PlayerController>().F_GetRB().velocity = Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance._players.GetChild(0).GetComponent<PlayerController>()._isCrashed = false;
    }
}

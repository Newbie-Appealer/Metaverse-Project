using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance._players.GetChild(0).GetComponent<PlayerController>()._isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance._players.GetChild(0).GetComponent<PlayerController>()._isGrounded = false;
    }
}

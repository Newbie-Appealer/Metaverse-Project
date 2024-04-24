using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        GameManager.Instance._player.GetComponent<PlayerController>()._isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance._player.GetComponent<PlayerController>()._isGrounded = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("야");
        GameManager.Instance._player.GetComponent<PlayerController>()._isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("충돌 해제");
        GameManager.Instance._player.GetComponent<PlayerController>()._isGrounded = false;
    }
}

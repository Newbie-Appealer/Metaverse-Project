using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("��");
        GameManager.Instance._player.GetComponent<PlayerController>()._isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�浹 ����");
        GameManager.Instance._player.GetComponent<PlayerController>()._isGrounded = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGround : MonoBehaviour
{
    public PlayerController _playerController;

    private void OnTriggerStay(Collider other)
    {
        if(_playerController != null)
        {
            _playerController._isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_playerController != null)
        {
            _playerController._isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerController != null)
        {
            _playerController._isGrounded = true;
        }
    }
}

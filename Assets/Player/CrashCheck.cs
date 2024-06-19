using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCheck : MonoBehaviour
{
    [SerializeField] private PlayerController _playerCtr;
    private void OnTriggerEnter(Collider other)
    {
        if (_playerCtr != null && !_playerCtr._isCrashed)
        {
            _playerCtr._isCrashed = true;
        }

        if(other.CompareTag("Player") && _playerCtr != null)
        {
            if(Input.GetMouseButtonDown(0))
            {

                other.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playerCtr != null && _playerCtr._isCrashed)
        {
            _playerCtr._isCrashed = false;
        }
    }
}

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playerCtr != null && _playerCtr._isCrashed)
        {
            _playerCtr._isCrashed = false;
        }
    }
}

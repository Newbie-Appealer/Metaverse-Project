using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeObject : MonoBehaviour
{
    [Header("fake object Colliders")]
    [SerializeField] private List<Collider> _colliders;

    private void Start()
    {
        if (_colliders == null)
            return;

        for(int i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = false;
        }
    }
}

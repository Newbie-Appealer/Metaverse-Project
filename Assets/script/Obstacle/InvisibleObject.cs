using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [Header("invisible object MeshRenderers")]
    [SerializeField] private List<MeshRenderer> _mesh;
    private void Start()
    {
        if (_mesh == null)
            return; 

        for(int i = 0; i < _mesh.Count; i++)
        {
            _mesh[i].enabled = false;
        }
    }
}

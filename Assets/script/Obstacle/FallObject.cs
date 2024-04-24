using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    Vector3 currentPosition;
    Quaternion currentRotation;
    Rigidbody rb;
    private void Start()
    {
        currentPosition = transform.position;
        currentRotation = transform.rotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        StartCoroutine(F_ReturnObject());
    }
    IEnumerator F_ReturnObject()
    {
        yield return new WaitForSeconds(3f);   // 3초 대기후
        transform.position = currentPosition;
        transform.rotation = currentRotation;
        Destroy(rb);
    }
}

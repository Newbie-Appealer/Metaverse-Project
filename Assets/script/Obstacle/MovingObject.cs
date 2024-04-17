using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPosition;
    private Vector3 _beforePosition;

    private void Start()
    {
        _beforePosition = transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(F_MoveObject());
        }
    }

    IEnumerator F_MoveObject()
    {
        yield return new WaitForSeconds(0.5f);
        while (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, 0.1f);
        }

        StartCoroutine(F_ReturnObject());       // 돌아감
    }

    IEnumerator F_ReturnObject()
    {
        yield return new WaitForSeconds(5f);    // 5초 대기후
        while (Vector3.Distance(transform.position, _beforePosition) > 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _beforePosition, 0.1f);
        }
    }
}

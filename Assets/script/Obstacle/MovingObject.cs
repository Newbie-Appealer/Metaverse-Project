using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("local Position")]
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _beforePosition;

    private bool _isMoving;
    private void Start()
    {
        _beforePosition = transform.localPosition;
        _isMoving = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (_isMoving)
            return;

        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(F_MoveObject());
        }
    }

    IEnumerator F_MoveObject()
    {
        _isMoving = true;
        yield return new WaitForSeconds(0.5f);
        while (Vector3.Distance(transform.localPosition, _targetPosition) > 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _targetPosition, 0.04f);
        }

        StartCoroutine(F_ReturnObject());       // 돌아감
    }

    IEnumerator F_ReturnObject()
    {
        yield return new WaitForSeconds(3f);    // 3초 대기후
        while (Vector3.Distance(transform.localPosition, _beforePosition) > 0.1f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _beforePosition, 0.04f);
        }

        _isMoving = false;
    }
}

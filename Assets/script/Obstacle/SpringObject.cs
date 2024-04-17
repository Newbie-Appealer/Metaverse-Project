using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    UP,
    RANDOM,
}

public class SpringObject : MonoBehaviour
{

    [SerializeField] private Direction _direction;
    [SerializeField] private float _springForce;

    private void Start()
    {
        if(_springForce <= 0.5f)
        {
            Debug.LogError("_springForce 가 너무 작거나 없음!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();

            switch (_direction)
            {
                case Direction.UP:
                    F_UpForceObject(playerRB);
                    break;
                case Direction.RANDOM:
                    F_RandomForceObject(playerRB);
                    break;
            }
        }
    }

    private void F_UpForceObject(Rigidbody v_rb)
    {
        v_rb.AddForce(Vector3.up * _springForce, ForceMode.Impulse);
    }

    private void F_RandomForceObject(Rigidbody v_rb)
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(0.0f, 1.0f);
        float z = Random.Range(-1.0f, 1.0f);

        Vector3 direction = new Vector3(x, y, z);

        v_rb.AddForce(direction * _springForce, ForceMode.Impulse);
    }
}

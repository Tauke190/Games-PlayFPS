using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{


    public bool open;


    private Vector3 intialposition;
    private Vector3 endposition;


    private void Start()
    {
        intialposition = transform.position;
        endposition = transform.position + new Vector3(0, 5, 0);
    }

    private void Update()
    {
        if (open)
        {
            transform.position = Vector3.Lerp(transform.position, endposition, 0.01f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, intialposition, 0.01f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            open = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Closedoor());
        }
    }

    private IEnumerator Closedoor()
    {
        yield return new WaitForSeconds(3);
        open = false;
    }



}

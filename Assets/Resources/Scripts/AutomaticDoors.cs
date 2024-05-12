using UnityEngine;
using System.Collections;

public class AutomaticDoors : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public Transform leftClosedLocation;
    public Transform rightClosedLocation;
    public Transform leftOpenLocation;
    public Transform rightOpenLocation;

    public float speed = 1.0f;

    bool isOpening = false;
    bool isClosing = false;
    Vector3 distance;

    void Update ()
    {
        if (isOpening)
        {
            distance = leftDoor.localPosition - leftOpenLocation.localPosition;
            if (distance.magnitude < 0.001f)
            {
                isOpening = false;
                leftDoor.localPosition = leftOpenLocation.localPosition;
                rightDoor.localPosition = rightOpenLocation.localPosition;
            }
            else
            {
                leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, 
                                                      leftOpenLocation.localPosition, 
                                                      Time.deltaTime * speed);
                rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, 
                                                       rightOpenLocation.localPosition, 
                                                       Time.deltaTime * speed);
            }
        }
        else if (isClosing)
        {
            distance = leftDoor.localPosition - leftClosedLocation.localPosition;
            if (distance.magnitude < 0.001f)
            {
                isClosing = false;
                leftDoor.localPosition = leftClosedLocation.localPosition;
                rightDoor.localPosition = rightClosedLocation.localPosition;
            }
            else
            {
                leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, 
                                                      leftClosedLocation.localPosition, 
                                                      Time.deltaTime * speed);
                rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, 
                                                       rightClosedLocation.localPosition, 
                                                       Time.deltaTime * speed);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        isOpening = true;
        isClosing = false;
    }

    void OnTriggerStay(Collider col)
    {
        isOpening = true;
        isClosing = false;
    }

    void OnTriggerExit(Collider col)
    {
        isClosing = true;
        isOpening = false;
    }
}

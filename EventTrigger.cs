//using ElephantSDK;
//using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerStayEvent;
    public UnityEvent OnTriggerEnterEvent;
    public static float giveTime = 0.1f;


    float time;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnTriggerEnterEvent?.Invoke();
        }
    }
    private void OnTriggerStay(Collider other)
    {

        //doStuff
        if (other.gameObject.CompareTag("Player"))
        {
            time += Time.deltaTime;
            if (time >= giveTime)
            {
                time = 0;
                OnTriggerStayEvent?.Invoke();
            }
        }

    }
}

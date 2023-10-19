using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject objectToGive;
    public void GiveArrow()
    {
        GameObject arrowObj = GameManager.Instance.arrowPool.Pull(objectToGive.transform.position, objectToGive.transform.rotation).gameObject;
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrow.Initialize();
        ArrowStackManager.Instance.AddMeToStackAndEveluate(arrow);

        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class FinalPartBase : MonoBehaviour
{
    public float myScore;
    public TextMeshProUGUI scoreText;


    private void Awake()
    {
        scoreText.text = myScore.ToString();
    }

    public void ChangeMyScore(float score)
    {
        myScore = score;
        scoreText.text = score.ToString("0.0") + "x";
    }


}

public class FinalPart : FinalPartBase
{
    //public Agent myAgent;

    //bool agentActivated;

    //private void Start()
    //{
    //    Invoke(nameof(CantFire), 1f);
    //    myAgent.gameObject.SetActive(false);
    //}
    //private void Update()
    //{
    //    if (agentActivated)
    //        return;
    //    if (Vector3.Distance(transform.position, LevelManager.Instance.levelSettings.finalManager.cam1.transform.position) > GameManager.Instance.distanceToRunCommands + 30)
    //        return;
    //    myAgent.gameObject.SetActive(true);
    //    agentActivated = true;
    //    Invoke(nameof(CantFire), 1f);
    //}

    //private void CantFire()
    //{
    //    myAgent.SetCanFire(false);
    //}

}

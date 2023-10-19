using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public abstract class FinalManagerBase : MonoBehaviour
{
    public float scoreMultiplier;
    public int finalPartCount;
    public GameObject singleFinalPart;
    private Transform finalParent;

    public List<FinalPartBase> finalPartList = new List<FinalPartBase>();

    public bool useManualOffset;
    public float manualOffset;


    public float saturationMultiplier = 0.2f;
    public float hueMultiplier = 0.2f;
    public float maxSaturation = 1;
    public float minSaturation = 0.3f;
    public float m_Hue = 0.5f;
    public float m_Saturation = 0f;
    public float m_Vibrance = 0.5f;


    float oldSaturation;
    float oldHue;
    int direction = +1;


    public virtual void Awake()
    {
        finalParent = this.transform;

    }



    [Button]
    public void FinalPartsCreator()
    {
        //finalParent = this.transform;
        //oldSaturation = minSaturation;
        //oldHue = minSaturation;

        //for (int i = 0; i < finalPartCount; i++)
        //{
        //    //FinalPartBase finalPart = Instantiate(singleFinalPart).GetComponent<FinalPartBase>();
        //    GameObject currentTile = PrefabUtility.InstantiatePrefab(singleFinalPart.gameObject as GameObject) as GameObject;
        //    FinalPartBase finalPart = currentTile.GetComponent<FinalPartBase>();
        //    finalPartList.Add(finalPart);

        //    Color color = GetColorGradient();

        //    finalPart.GetComponent<Renderer>().material.color = color;

        //    Transform part = finalPart.transform;
        //    part.SetParent(finalParent);
        //    Vector3 newPos = Vector3.zero;
        //    newPos.z += useManualOffset ? i * manualOffset : i * part.localScale.z;
        //    part.localPosition = newPos;

        //    finalPart.ChangeMyScore(1 + i * scoreMultiplier);
        //}
    }

    private Color GetColorGradient()
    {
        Color color = new Color();
        //color = Random.ColorHSV();
        //Color.RGBToHSV(color, out m_Hue, out m_Saturation, out m_Vibrance);


        if (direction > 0)
        {
            //m_Saturation = oldSaturation + saturationMultiplier;
            m_Hue = oldHue + hueMultiplier;
        }

        if (direction < 0)
        {
            //m_Saturation = oldSaturation - saturationMultiplier;
            m_Hue = oldHue - hueMultiplier;
        }

        if (oldHue < minSaturation)
        {
            direction = +1;
            //m_Saturation = minSaturation;
            m_Hue = minSaturation + hueMultiplier;

        }
        if (oldHue > maxSaturation)
        {
            direction = -1;
            //m_Saturation = maxSaturation;
            m_Hue = maxSaturation - hueMultiplier;

        }

        color = Color.HSVToRGB(m_Hue, m_Saturation, m_Vibrance);

        oldSaturation = m_Saturation;
        oldHue = m_Hue;
        return color;
    }

    [Button]
    public void DeleteParts()
    {
        foreach (var item in finalPartList)
        {
            DestroyImmediate(item.gameObject);
        }
        finalPartList.Clear();
    }


    public abstract void OnFinalTriggered();
}

public class FinalManager : FinalManagerBase
{

    //public static FinalManager Instance;

    public bool isDoingFinal;

    public CinemachineVirtualCamera cam1;
    public bool followLastArrow;
    public float moveTimeForOneAgent = 0.5f;
    public float finalArrowSpeed;


    public override void Awake()
    {
        base.Awake();
        //Instance = this;

    }

    public override void OnFinalTriggered()
    {
        //StartCoroutine(DoFinal());
    }

    //public IEnumerator DoFinal()
    //{
    //    if (isDoingFinal)
    //        yield break;
    //    isDoingFinal = true;
    //    float delay = 0.2f;
    //    PlayerController.Instance.SetCanMove(false);
    //    PlayerController.Instance.SetCanFire(false);
    //    if(ArrowSpawner.Instance)
    //        ArrowSpawner.Instance.SetCanSpawn(false);
    //    UIManager.Instance.SetActiveButtons(false);

    //    int arrowCount = ArrowStackManager.Instance.stack.Count;
    //    int finalPartCount = this.finalPartCount;
    //    int count = Mathf.Min(arrowCount, finalPartCount);

    //    LevelManager.Instance.LevelCompleted();

    //    if (count == 0)
    //    {
    //        UIManager.Instance.SetTotalMoneyOnFinal(GameManager.Instance.collectedMoney);
    //        UIManager.Instance.OpenWinPanel();
    //        yield break;
    //    }

    //    cam1.Follow = null;

    //    FinalPart lastPart = (FinalPart)finalPartList[count - 1];
    //    Agent lastAgent = lastPart.myAgent;

    //    if (!followLastArrow)
    //    {
    //        cam1.transform.DOMoveZ(lastAgent.transform.position.z - 10f, count * moveTimeForOneAgent);
    //    }

    //    Arrow lastArrow = null;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (ArrowStackManager.Instance.stack.Count <= 0)
    //            continue;
    //        Arrow arrow = ArrowStackManager.Instance.GetArrow();
    //        Agent agent = ((FinalPart)finalPartList[i]).myAgent;

    //        ArrowStackManager.Instance.RemoveMeFromStackAndEveluate(arrow);
    //        //PlayerController.Instance.anims[1].SetTrigger("fire");
    //        arrow.FireMeFinal(agent);
    //        //arrow.SetActiveTrail(true);
    //        arrow.arrowSpeed = finalArrowSpeed;
    //        //arrow.destroyMeCoroutine = StartCoroutine(arrow.gameObject.SetActiveAfterSeconds(5f));
    //        //await Task.Delay(delay.FloatToMilisecond());
    //        //yield return Extensions.GetWait(delay);

    //        lastArrow = arrow;

    //        yield return null;
    //    }
    //    if (followLastArrow)
    //    {
    //        cam1.Follow = lastArrow.transform;

    //        while (!lastArrow.finalAgent.amIDead)
    //        {
    //            yield return null;
    //        }
    //        cam1.Follow = null;
    //        cam1.transform.DOMoveZ(lastAgent.transform.position.z - 10f, 0.5f);

    //        float totalWaitToPanelOpen = 1f;
    //        yield return Extensions.GetWait(totalWaitToPanelOpen / 2);

    //        int newMoney = (int)(GameManager.Instance.levelMoney * (lastPart.myScore - 1));
    //        GameManager.Instance.MoneyAdd(newMoney);

    //        yield return Extensions.GetWait(totalWaitToPanelOpen / 2);
    //        //LevelManager.Instance.LevelCompleted();
    //        UIManager.Instance.SetTotalMoneyOnFinal(GameManager.Instance.collectedMoney);
    //        UIManager.Instance.OpenWinPanel();

    //    }


    //}


    
}

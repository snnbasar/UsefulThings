using DG.Tweening;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum perdeType
{
    Calculation,
    SpreadShot,
    ShieldTime,
    FireRate
}
public enum Calculations
{
    Toplama,
    Cikarma,
    Bolme,
    Carpma
}

[System.Serializable]
public class CalculationSingle
{
    [Header("TYPE SETTINGS")]
    public perdeType myType;
    public float myValue;

    public GameObject perdeParent;
    [Header("CALCULATION SETTINGS")]

    //public bool amICalculation;
    public Calculations myCalculation;
    public int myCalcNumber;

    public TextMeshProUGUI myText;
    public TextMeshProUGUI myUpText;

    [Header("EXPLOSION SETTINGS")]
    public Transform perdeOuter;
    public Rigidbody[] perdeInner;
    public Transform expPosition;


    public string CalculationSign(Calculations calculation)
    {
        string sign = "";

        switch (calculation)
        {
            case Calculations.Toplama:
                sign = "+";
                break;
            case Calculations.Cikarma:
                sign = "-";
                break;
            case Calculations.Bolme:
                sign = "/";
                break;
            case Calculations.Carpma:
                sign = "X";
                break;
        }

        return sign;
    }

}
public class Perde : MonoBehaviour
{

    public CalculationSingle[] perdeler;

    private bool perdeTriggered;

    [Header("EXPLOSION SETTINGS")]
    public float power;
    public float range;
    public float upwardForce;
    public ForceMode forceMode;
    public float destroyPartAfterSec;

    private void Start()
    {

        foreach (var perde in perdeler)
        {
            string sign = Mathf.Sign(perde.myValue) == 1 ? "+" : "-";

            switch (perde.myType)
            {
                case perdeType.Calculation:
                    perde.myText.text = perde.CalculationSign(perde.myCalculation) + perde.myCalcNumber.ToString();
                    perde.myUpText.text = "Arrow";
                    break;
                case perdeType.SpreadShot:
                    perde.myText.text = sign + Mathf.Abs(perde.myValue);
                    perde.myUpText.text = "Spread Shot";
                    break;
                case perdeType.ShieldTime:
                    perde.myText.text = sign + Mathf.Abs(perde.myValue);
                    perde.myUpText.text = "Shield Time";
                    break;
                case perdeType.FireRate:
                    perde.myText.text = Mathf.Sign(perde.myValue) == 1 ? "-" : "+" + Mathf.Abs(perde.myValue * 100);
                    perde.myUpText.text = "Fire Rate";
                    break;
                default:
                    break;
            }
        }


    }

    public void OnPerdeTriggered(int id) //Calls on event trigger
    {
        if (perdeTriggered)
            return;
        perdeTriggered = true;

        //CalculationManager.Instance.AddCalculation(perdeler[id]);



    }

    [Button]
    private void PerdeExplosionEffect(int id)
    {

        CalculationSingle perde = perdeler[id];
        perde.myText.gameObject.SetActive(false);
        perde.perdeOuter.gameObject.SetActive(false);
        foreach (var inner in perde.perdeInner)
        {
            inner.gameObject.SetActive(true);
            inner.transform.SetParent(null);
            inner.isKinematic = false;
            Destroy(inner.gameObject, destroyPartAfterSec);

            inner.AddExplosionForce(power,
                perde.expPosition.position, range, upwardForce, forceMode);


            //inner.AddExplosionForce(GameManager.Instance.obstractExplosionForce,
            //    expPosition.position, GameManager.Instance.obstractExplosionForce);


        }
    }



    #region AutomaticCalculation
    public void DoCalculation(int id) //Calls on event trigger
    {
        if (perdeTriggered)
            return;
        perdeTriggered = true;


        switch (perdeler[id].myType)
        {
            case perdeType.Calculation:
                Calculation(id);
                break;
            case perdeType.SpreadShot:
                SpreadShot(id);
                break;
            case perdeType.ShieldTime:
                ShieldTime(id);
                break;
            case perdeType.FireRate:
                FireRate(id);
                break;
            default:
                break;
        }

        ScaleAnim(id);
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);

        //PerdeExplosionEffect(id);

    }

    private void Calculation(int id)
    {
        int currentSoldier = ArrowStackManager.Instance.stack.Count;

        int result = 0;
        switch (perdeler[id].myCalculation)
        {
            case Calculations.Toplama:
                result = currentSoldier + perdeler[id].myCalcNumber;
                break;
            case Calculations.Cikarma:
                result = currentSoldier - perdeler[id].myCalcNumber;
                break;
            case Calculations.Bolme:
                result = currentSoldier / perdeler[id].myCalcNumber;
                break;
            case Calculations.Carpma:
                result = currentSoldier * perdeler[id].myCalcNumber;
                break;
            default:
                break;
        }
        if (result <= 0)
            result = 0;
        ArrowStackManager.Instance.SetArrowCount(result);
    }

    private void SpreadShot(int id)
    {
        int value = (int)perdeler[id].myValue;
        PlayerController.Instance.DoSpreadShot(value);
    }

    private void ShieldTime(int id)
    {
        float value = perdeler[id].myValue;
        Shield.Instance.IncreaseShieldTime(value);
    }

    private void FireRate(int id)
    {
        float value = perdeler[id].myValue;
        PlayerController.Instance.IncreaseFireTime(value);
    }


    private void ScaleAnim(int id)
    {
        Vector3 curScale = transform.localScale;
        Sequence seq = DOTween.Sequence();
        float scaleMultiplier = 1.2f;
        float time = 0.25f;
        seq.Append(perdeler[id].perdeParent.transform.DOScale(curScale * scaleMultiplier, time / 2).SetEase(Ease.Linear));
        seq.Append(perdeler[id].perdeParent.transform.DOScale(curScale, time / 2).SetEase(Ease.Linear));
    }
    #endregion

}

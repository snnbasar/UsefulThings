using DG.Tweening;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;




public class CalculationManager : MonoBehaviour
{
    public static CalculationManager Instance;

    public List<CalculationSingle> calculations = new List<CalculationSingle>();

    public TextMeshProUGUI[] texts;
    public Image[] backgrounds;
    public Image backgroundPanel;
    public Color calcColor;
    public Color calcColor2;
    public float animTime;

    [Header("LEVEL DESIGN")]
    public int[] numbers;
    public Calculations[] calcs;
    public Perde[] perdeler;



    //    private void Awake()
    //    {
    //        Instance = this;
    //        DoScaleAnim(0);
    //    }


    //    public void AddCalculation(CalculationSingle calculation)
    //    {
    //        int numberCount = 0;
    //        int calcCount = 0;
    //        foreach (var calc in calculations)
    //        {
    //            if (calc.amICalculation)
    //                calcCount++;
    //            else
    //                numberCount++;
    //        }

    //        if (calcCount >= 1 && calculation.amICalculation)
    //            return;
    //        if (numberCount >= 2 && !calculation.amICalculation)
    //            return;

    //        calculations.Add(calculation);

    //        if (calculations.Count < 3)
    //            DoScaleAnim(calculations.Count);
    //        else
    //            StopScaleAnim(2);

    //        SetTexts(calculation);


    //        CheckCalculation();
    //    }


    //    private void CheckCalculation()
    //    {
    //        if (calculations.Count <= 2)
    //            return;

    //        int number1 = 0;
    //        Calculations calculation = Calculations.Toplama;
    //        int number2 = 0;
    //        foreach (var calc in calculations)
    //        {
    //            if (calc.amICalculation)
    //                calculation = calc.myCalculation;
    //            else
    //            {
    //                if (number1 == 0)
    //                    number1 = calc.myCalcNumber;
    //                else
    //                    number2 = calc.myCalcNumber;
    //            }
    //        }

    //        int result = DoCalculation(number1, calculation, number2);

    //        Sequence seq = DOTween.Sequence();
    //        seq.SetDelay(animTime);

    //        for (int i = 3; i < backgrounds.Length; i++)
    //        {
    //            if (i == backgrounds.Length - 1)
    //                seq.Append(backgrounds[i].DOColor(calcColor, animTime).OnStart(() =>
    //                {
    //                    texts[4].text = result.ToString();
    //                }));
    //            else if (i == 3)
    //                seq.Append(texts[3].DOColor(calcColor, animTime));

    //            else
    //                seq.Append(backgrounds[i].DOColor(calcColor, animTime));

    //        }
    //        seq.Append(backgroundPanel.DOColor(calcColor2, animTime).SetLoops(4, LoopType.Yoyo));

    //        seq.OnComplete(() =>
    //        {
    //            //PlayerController.Instance.SetSoldierAmount(result);



    //            calculations.Clear();

    //            ClearTexts(2f);


    //        });


    //    }

    //    private static int DoCalculation(int number1, Calculations calculation, int number2)
    //    {
    //        int result = 0;

    //        switch (calculation)
    //        {
    //            case Calculations.Toplama:
    //                result = number1 + number2;
    //                break;
    //            case Calculations.Cikarma:
    //                result = number1 - number2;
    //                break;
    //            case Calculations.Bolme:
    //                result = number1 / number2;
    //                break;
    //            case Calculations.Carpma:
    //                result = number1 * number2;
    //                break;
    //        }

    //        return result;
    //    }

    //    private void SetTexts(CalculationSingle calculation)
    //    {
    //        if (calculations.Count == 1)
    //        {
    //            texts[0].text = calculation.myCalcNumber.ToString();
    //            Sequence seq = DOTween.Sequence();
    //            seq.Append(backgrounds[0].DOColor(calcColor, animTime));
    //            //seq.Append(backgrounds[0].DOColor(Color.white, 0.1f));
    //        }

    //        if (calculations.Count == 2)
    //        {
    //            texts[1].text = CalculationSign(calculation.myCalculation);
    //            Sequence seq = DOTween.Sequence();
    //            seq.Append(texts[1].DOColor(calcColor, animTime));
    //            //seq.Append(backgrounds[1].DOColor(Color.white, 0.1f));
    //            texts[5].text = "";
    //        }
    //        if (calculations.Count == 3)
    //        {
    //            texts[2].text = calculation.myCalcNumber.ToString();

    //            Sequence seq = DOTween.Sequence();
    //            seq.Append(backgrounds[2].DOColor(calcColor, animTime));
    //            //seq.Append(backgrounds[2].DOColor(Color.white, 0.1f));
    //        }
    //    }



    //    public async void ClearTexts(float time = 0)
    //    {
    //        await Task.Delay(time.FloatToMilisecond());

    //        for (int i = 0; i < texts.Length; i++)
    //        {
    //            if (i == 3)
    //            {
    //                texts[3].DOColor(Color.white, 0.1f);
    //                continue;
    //            }
    //            texts[i].text = "";
    //            texts[5].text = "?";

    //        }
    //        for (int i = 0; i < backgrounds.Length; i++)
    //        {
    //            backgrounds[i].color = Color.white;

    //        }

    //        DoScaleAnim(0);

    //    }



    //    private void DoScaleAnim(int line)
    //    {
    //        for (int i = 0; i < 3; i++)
    //        {
    //            StopScaleAnim(i);
    //        }

    //        float scale = 1.2f;
    //        float duration = 0.5f;

    //        backgrounds[line].rectTransform.DOScale(scale, duration).SetLoops(-1, LoopType.Yoyo);

    //    }

    //    private void StopScaleAnim(int line)
    //    {
    //        backgrounds[line].rectTransform.DOKill();
    //        backgrounds[line].rectTransform.DOScale(1, 0.1f);
    //    }

    //    [Button]
    //    public void CalculateVariations()
    //    {
    //        int[] results = new int[8];
    //        results[0] = (DoCalculation(numbers[0], calcs[0], numbers[2]));
    //        results[1] = (DoCalculation(numbers[0], calcs[0], numbers[3]));
    //        results[2] = (DoCalculation(numbers[0], calcs[1], numbers[2]));
    //        results[3] = (DoCalculation(numbers[0], calcs[1], numbers[3]));

    //        results[4] = (DoCalculation(numbers[1], calcs[0], numbers[2]));
    //        results[5] = (DoCalculation(numbers[1], calcs[0], numbers[3]));
    //        results[6] = (DoCalculation(numbers[1], calcs[1], numbers[2]));
    //        results[7] = (DoCalculation(numbers[1], calcs[1], numbers[3]));

    //        Array.Sort(results);


    //        for (int i = 0; i < results.Length; i++)
    //        {
    //            print(results[i]);
    //        }


    //        print("Middles are: " + results[3] + " - " + results[4]);

    //    }



    //    [Button]
    //    public void DoRandomCalc()
    //    {
    //        for (int i = 0; i < numbers.Length; i++)
    //        {
    //            numbers[i] = UnityEngine.Random.Range(0, 101);

    //        }
    //        for (int i = 0; i < calcs.Length; i++)
    //        {
    //            calcs[i] = (Calculations)UnityEngine.Random.Range(0, 4);
    //        }
    //    }

    //    [Button]
    //    public void AddCalculationsToPerdes()
    //    {
    //        for (int i = 0; i < perdeler.Length; i++)
    //        {
    //            if(i == 1)
    //            {
    //                int random = UnityEngine.Random.Range(0, 2);
    //                perdeler[i].perdeler[random].amICalculation = true;
    //                perdeler[i].perdeler[random].myCalculation = calcs[0];

    //                perdeler[i].perdeler[random == 1 ? 0 : 1].amICalculation = true;
    //                perdeler[i].perdeler[random == 1 ? 0 : 1].myCalculation = calcs[1];
    //            }
    //            else
    //            {
    //                int random = UnityEngine.Random.Range(0, 2);
    //                perdeler[i].perdeler[random].amICalculation = false;
    //                perdeler[i].perdeler[random].myCalcNumber = numbers[i + 0];

    //                perdeler[i].perdeler[random == 1 ? 0 : 1].amICalculation = false;
    //                perdeler[i].perdeler[random == 1 ? 0 : 1].myCalcNumber = numbers[i + 1];
    //            }

    //#if UNITY_EDITOR
    //            EditorUtility.SetDirty(perdeler[i]);
    //#endif
    //        }
    //    }



}

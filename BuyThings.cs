using Coffee.UIEffects;
using ElephantSDK;
using MoreMountains.NiceVibrations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyThings : Buyable
{
    public int myId;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI tolevelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI onUpdateText;
    public Button button;
    public CanvasGroup canvasGroup;
    public UIShiny uishiny;

    public float priceMultiplier;



    private void Start()
    {
        //if (GameManager.useRemote)
        //{
        //    startLootMultiplier = RemoteConfig.GetInstance().GetFloat("startLootMultiplier", 1f);
        //    incomeMultiplier = RemoteConfig.GetInstance().GetFloat("incomeMultiplier", 1f);
        //    priceMultiplier = RemoteConfig.GetInstance().GetFloat("priceMultiplier", 25f);

        //    switch (myId)
        //    {
        //        case 0:
        //            maxLevel = RemoteConfig.GetInstance().GetInt("maxLevel_Startloot", 5);
        //            break;
        //        case 1:
        //            maxLevel = RemoteConfig.GetInstance().GetInt("maxLevel_Income", 10);
        //            break;
        //        case 2:
        //            maxLevel = RemoteConfig.GetInstance().GetInt("maxLevel_Damage", 30);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        Initialize();
    }

    public void Initialize()
    {
        //if (GameManager.useRemote)
        //{
        //    if (myId == 0)
        //    {
        //        myPrice = RemoteConfig.GetInstance().GetInt("buyRopePrice", 50);
        //        priceMultiplier = RemoteConfig.GetInstance().GetFloat("buyRopePriceMultiplier", 50);
        //        maxLevel = RemoteConfig.GetInstance().GetInt("buyRopeMaxLevel", 1000);
        //    }
        //    if (myId == 1)
        //    {
        //        myPrice = RemoteConfig.GetInstance().GetInt("buyDigitPrice", 50);
        //        priceMultiplier = RemoteConfig.GetInstance().GetFloat("buyDigitPriceMultiplier", 50);
        //        maxLevel = RemoteConfig.GetInstance().GetInt("buyDigitMaxLevel", 1000);
        //    }
        //}
        //GameManager.Instance.AddMeToBuyThings(this);
        //SetText();
        //if (myId == 0) return;
        //if (myId == 1) return;
        int level = SaveManager.LoadFromDictionary(myId, "buyThingsData", startLevel);
        ManualSetLevel(level);
        UpdateMyPrice();
        SetText();

        //ManualSetLevel(level);
    }


    private void Update()
    {
        bool sts;
        sts = CheckDoIHaveEnoughMoney() && CheckCanUpdateLevel();

        if (button && !canvasGroup)
        {
            button.interactable = sts;
        }
        if (canvasGroup)
        {
            canvasGroup.interactable = sts;
            canvasGroup.alpha = sts ? 1f : 0.65f;
        }

        if (uishiny) uishiny.enabled = sts;

    }

    public override void OnLevelUpgrade()
    {
        if(myId == 0)
        {
            
        }
        if (myId == 1)
        {
            
        }

        SetText();

        //GameManager.Instance.SaveBuyThingsUpgradeData(this);
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        //if (myId == 0) return;
        //if (myId == 1) return;
        SaveManager.SaveToDictionary(myId, currentLevel, "buyThingsData");
        //CheckTutorial();

    }

    //private void CheckTutorial()
    //{
    //    int tutorial = SaveManager.Load("tutorial", 0);
    //    if (tutorial == -1) return;
    //    if (myId == 0 && tutorial == 0)
    //    {
    //        SaveManager.Save(1, "tutorial");
    //        UIManager.Instance.ShowTutorial(1);
    //    }
    //}


    public override void UpdateMyPrice()
    {
        //base.UpdateMyPrice();
        //myPrice = (int)(myPrice * priceMultiplier);
        //myPrice += (int)priceMultiplier;
        //if (myId == 0)
        //{
        //    myPrice = EconomyManager.instance.GetAddMineToolPrice(currentLevel);
        //}
        //if (myId == 1)
        //{
        //    myPrice = EconomyManager.instance.GetAddKolPrice(currentLevel);
        //}
        SetText();
    }

    private void SetText()
    {
        if (CheckCanUpdateLevel())
        {
            //levelText.text = (currentLevel + 1).ToString();
            //if(currentLevel + 2 <= maxLevel + 1)
            //    tolevelText.text = (currentLevel + 2).ToString();
            //else
            //    tolevelText.text = "-";

            costText.text = "" + Extensions.KMBMaker(myPrice);
        }
        else
        {
            //levelText.text = "-";
            costText.text = "-";
        }
        //if (myId == 0)
        //{
        //    if (CheckCanUpdateLevel())
        //    {
        //        levelText.text = (currentLevel + 1).ToString();
        //        tolevelText.text = (currentLevel + 2).ToString();
        //        costText.text = "$" + Extensions.KMBMaker(myPrice);
        //    }
        //    else
        //    {
        //        levelText.text = "-";
        //        costText.text = "-";
        //    }
        //    //onUpdateText.text = PlayerController.Instance.fireTime.ToString() + " sc/arrw";
        //}
        //if(myId == 1)
        //{
        //    if (CheckCanUpdateLevel())
        //    {
        //        levelText.text = (currentLevel + 1).ToString() + "/" + (maxLevel + 1).ToString();
        //        costText.text = "$" + Extensions.KMBMaker(myPrice);
        //    }
        //    else
        //    {
        //        levelText.text = "-";
        //        costText.text = "-";
        //    }
        //    //onUpdateText.text = Shield.Instance.shieldActiveTime.ToString("0.0") + " sn";

        //}


    }

    public void SetActiveMe(bool sts) => this.gameObject.SetActive(sts);

}

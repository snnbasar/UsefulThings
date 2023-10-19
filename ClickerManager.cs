using ElephantSDK;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.NiceVibrations;


public class ClickerManager : MonoBehaviour
{
    public bool enableMe;
    public bool canClick;
    public bool enableCurrentClickMultiplier = true;
    public static ClickerManager Instance;
    public UnityEvent<float> OnClicked;
    public UnityEvent OnClickEnded;
    public float clickMultiplier = 1.25f;
    public float clickEndTime = 1f;
    public float tapToSpeedUpTime = 5f;

    [SerializeField] private GameObject tapToSpeedUp;

    float timer_Click;
    float timer_TapToSpeedUp;
    bool clicked;
    bool onClickInvoked;

    public static float currentMultiplier = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentMultiplier = 1f;
        enableMe = RemoteConfig.GetInstance().GetBool("enableClicker", enableMe); // false
        clickMultiplier = RemoteConfig.GetInstance().GetFloat("clickMultiplier", clickMultiplier); //1.25f
    }

    private void Update()
    {
        if (!enableMe)
            return;
        if (!canClick)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
            timer_Click = 0;
            timer_TapToSpeedUp = 0;
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        if (clicked && !onClickInvoked)
        {
            DoClick();
        }

        timer_Click += Time.deltaTime;
        timer_TapToSpeedUp += Time.deltaTime;
        if(timer_Click >= clickEndTime && clicked)
        {
            EndClick();
        }
        if (timer_TapToSpeedUp >= tapToSpeedUpTime)
        {
            SetActiveTapToSpeedUpText(true);
        }
    }

    public void DoClick()
    {
        OnClicked?.Invoke(clickMultiplier);
        onClickInvoked = true;
        SetActiveTapToSpeedUpText(false);
        if(enableCurrentClickMultiplier) currentMultiplier = clickMultiplier;
    }

    public void EndClick()
    {
        OnClickEnded?.Invoke();
        clicked = false;
        onClickInvoked = false;
        currentMultiplier = 1f;
    }

    public void SetCanClick(bool sts, bool force = false)
    {
        if (force)
        {
            canClick = sts;
            return;
        }
        if (GameManager.Instance._gameState != GameManager.GameState.Started || TutorialManager.tutorialState != -1)
            return;
        canClick = sts;
    }
    public void SetActiveTapToSpeedUpText(bool sts)
    {
        if (!tapToSpeedUp)
            return;
        tapToSpeedUp.gameObject.SetActive(sts);
    }

    public bool IsActive() => canClick && enableMe;
}
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public static int tutorialState;
    public TutorialPart[] tutorialParts;
    private void Awake()
    {
        Instance = this;
        tutorialState = SaveManager.Load("tutorial", 0);

    }
    [Button]
    public void ShowTutorial(int x, bool save = false)
    {
        foreach (var part in tutorialParts)
        {
            Array.ForEach(part.tutorialObjects, x => x.SetActive(false));
        }
        if(x != -1)
            Array.ForEach(tutorialParts[x].tutorialObjects, x => x.SetActive(true));
        tutorialState = x;
        if (save)
            SaveManager.Save(tutorialState, "tutorial");
    }

    public static bool CheckShowTutorial(int tutorialState) => TutorialManager.tutorialState != -1 && TutorialManager.tutorialState == tutorialState;
}

[System.Serializable]
public class TutorialPart
{
    public GameObject[] tutorialObjects;
}
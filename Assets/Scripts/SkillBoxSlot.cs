using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SkillBoxSlot : MonoBehaviour
{
    [Header("Dynamic")]
    public Action component;

    private PlayerController pC;
    private EventBoxText eBT;

    private void Awake()
    {
        pC = FindFirstObjectByType<PlayerController>();
        eBT = FindFirstObjectByType<EventBoxText>();
    }

    public void SetSkillSelected()
    {
        pC.skillSelected = component;
        eBT.AddText("Currently Selected: " + component.displayName);

    }
}

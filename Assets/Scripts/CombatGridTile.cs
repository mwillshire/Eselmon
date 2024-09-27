using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CombatGridTile : MonoBehaviour
{
    [Header("Inscribed")]
    public Color baseColor;
    public Color offsetColor;
    public GameObject highLight;

    [Header("Dynamic")]
    public bool isClicked = false;
    public bool isInNullZone = false;

    private SpriteRenderer sP;

    private void Awake()
    {
        sP = GetComponent<SpriteRenderer>();

        StartCoroutine(CheckCombatModeRoutine());
    }

    private IEnumerator CheckCombatModeRoutine()
    {
        float delay = 1f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            CombatActive();
        }
    }

    private void CombatActive()
    {
        if (StaticVariables.combatMode && !gameObject.activeInHierarchy) gameObject.SetActive(true);
        else if (!StaticVariables.combatMode && gameObject.activeInHierarchy) gameObject.SetActive(false);
    }

    public void Init(bool isOffset)
    {
        sP.color = isOffset ? offsetColor : baseColor;
    }

    private void OnMouseEnter()
    {
        highLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highLight.SetActive(false); 
    }

    private void OnMouseDown()
    {
        if (isInNullZone) return;
        isClicked = true;
    }
}

//Parent to any combat actions the player can take in the game

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Action : MonoBehaviour
{
    [Header("Inscribed")]
    public string displayName;
    public CombatController cC {  get; private set; }
    public PlayerController pC { get; private set; }

    private void Awake()
    {
        cC = FindFirstObjectByType<CombatController>();
        pC = FindFirstObjectByType<PlayerController>();
    }
    public virtual void TriggeringEffect(Vector3 location) { }
}

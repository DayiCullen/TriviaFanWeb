using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void ToggleSoundFX();
    public static event ToggleSoundFX OnToggleSoundFX;

    public static void ToggleSoundFXMethod()
    {
        if (OnToggleSoundFX != null)
        {
            OnToggleSoundFX();
        }
    }

}

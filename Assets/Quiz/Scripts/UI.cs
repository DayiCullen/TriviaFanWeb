using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private VerticalLayoutGroup layoutGroup;

    void Start()
    {
        layoutGroup = GetComponent<VerticalLayoutGroup>();
        AdjustLayout();
    }

    void AdjustLayout()
    {
        // Detectamos Monitor (Horizontal)
        if (Screen.width < Screen.height)
        {
            // Configuración para Monitor:
            layoutGroup.childControlWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childScaleHeight = false;
            layoutGroup.childScaleWidth = false;
            // (Asegúrate de que tus botones tengan un alto fijo aquí)
        }     
    }
}

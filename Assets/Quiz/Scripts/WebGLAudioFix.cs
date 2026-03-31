using UnityEngine;
using UnityEngine.EventSystems;

public class WebGLAudioFix : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Esto fuerza al navegador a habilitar el audio si estaba bloqueado
        if (AudioSettings.dspTime == 0)
        {
            // Solo una operación dummy para activar el motor
            AudioListener.pause = false;
        }
        Debug.Log("Audio Context Activado");
    }
}
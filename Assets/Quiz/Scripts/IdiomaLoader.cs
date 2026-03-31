using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdiomaLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Dictionary<SystemLanguage, string> sceneMap = new Dictionary<SystemLanguage, string>()
    {
        { SystemLanguage.Spanish, "MenuPrincipal" },        
        // Agregá más idiomas y escenas si querés
    };
    void Start()
    {
        SystemLanguage systemLanguage = Application.systemLanguage;
        Debug.Log("Idioma detectado: " + systemLanguage);

        string escenaDestino;

        if (sceneMap.TryGetValue(systemLanguage, out escenaDestino))
        {
            if (SceneManager.GetActiveScene().name != escenaDestino)
            {
                SceneManager.LoadScene(escenaDestino);
            }
        }
        else
        {
            // Si no reconoce el idioma, carga una escena por defecto
            SceneManager.LoadScene("PirncipalMenu");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

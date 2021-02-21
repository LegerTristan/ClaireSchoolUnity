using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Pause : MonoBehaviour
{
    private Image imPause; // Image de pause

    private bool onPause = false; // Contrôleur booléen de la pause

    [SerializeField]
    private AudioClip sndPause, sndUnpause; // Les sons de pause et de sortie de pause


    // On récupère la référence de l'image de la pause.
    void Start()
    {
        imPause = transform.Find("imPause").GetComponent<Image>();
    }

    /* On vérifie si la touche "Escape" est pressée .
       Le cas échéant, on édite le contrôleur et vérifie la valeur du booléen.
       En fonction de sa valeur, on ouvre ou ferme la pause en affichant/cachant l'image etc.*/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPause = !onPause;
            if (onPause)
            {
                GetComponent<AudioSource>().PlayOneShot(sndPause);
                imPause.enabled = true;
                Input.ResetInputAxes();
                Time.timeScale = 0f;
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(sndUnpause);
                imPause.enabled = false;
                Time.timeScale = 1f;
            }
        }
    }
}

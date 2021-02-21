using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    private Image bar; // L'image de la barre
    private Text txt; // Le composant texte contenant la valeur de la barre

    private float progress; // La progression de la barre
    public float Progress
    {
        get { return progress; }
        set { 
            progress = value;
            progress = Mathf.Clamp(progress, 0, 100);
            UpdateValueDisplay();
        }
    }

    [SerializeField]
    private float alert = 25f; // Indicateur du changement de couleur de la barre

    [SerializeField]
    private Color alertColor = Color.red; // Couleur de la barre lorsqu'elle est faible

    private Color startColor; // Couleur par défaut de la barre


    // On initialise la barre
    void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        txt = bar.transform.Find("BarTxt").GetComponent<Text>();
        startColor = bar.color;
        Progress = 100;
    }

    // Met à jour la valeurde la barre et sa couleur si nécessaire.
    private void UpdateValueDisplay()
    {
        txt.text = (int) progress + "%";
        bar.fillAmount = progress / 100;

        if(progress <= alert)
        {
            bar.color = alertColor;
        }
        else
        {
            bar.color = startColor;
        }
    }
}

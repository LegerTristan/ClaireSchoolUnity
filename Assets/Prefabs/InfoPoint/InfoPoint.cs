using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPoint : MonoBehaviour
{
    [TextArea]
    public string Information; // L'information a partagé aux joueurs

    [SerializeField]
    private Text text; // Le composant text contenant l'information

    [SerializeField]
    private GameObject infoPanel; // Le panel de l'information


    // On redéfinit le texte du composant en fonction de l'info a donné.
    void Start()
    {
        text.text = Information;
    }

    /*On vérifie si le collider présent dans le trigger est celui du Player, le cas échant on affiche le panel d'info.*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoPanel.SetActive(true);
        }
    }

    /*On vérifie si le collider présent dans le trigger est celui du Player, le cas échant on cache le panel d'info.*/
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoPanel.SetActive(false);
        }
    }
}

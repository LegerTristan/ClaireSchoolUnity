using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsScript : MonoBehaviour
{
    [SerializeField]
    private ProgressBar bar; // La barre liée au changement de valeur de l'objet

    [SerializeField]
    private int value = 10; // Valeur bonus qui va être appliqué à la barre liée.

    [SerializeField]
    private MeshRenderer meshRenderer; // Le MeshRenderer de l'objet

    /*Vérifie si le joueur est entré en collision avec l'objet
     Le cas échéant, on effectue le changement de progression sur la barre et on fait disparaître le mesh et le collider
     de l'objet avant de le détruire.*/
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            AudioSource audioSrc = GetComponent<AudioSource>();
            bar.Progress += value;
            audioSrc.Play();

            GetComponent<BoxCollider>().enabled = false;
            meshRenderer.enabled = false;
            Destroy(gameObject, audioSrc.clip.length);
            
        }
    }
}

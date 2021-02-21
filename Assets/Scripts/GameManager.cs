using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private ProgressBar healthBar, staminaBar, hungerBar; // Les barres présentes dans le jeu

    // Hunger variables
    [SerializeField]
    private float hungerDecreaseAmount = 1f, hungerDecreaseRate = 2f; // Valeur et fréquence de l'évolution de l'appétit

    // Stamina variables
    [SerializeField]
    private float walkCost = .01f, runCost = .3f; // Coût en énergie de la marche et de la course

    private bool gameOver = false; // Contrôleur booléen de la fin de partie


    // On active l'évolution de l'appétit
    void Start()
    {
        StartCoroutine(DecreaseHunger());
    }

    // Coroutine qui évolue selon une fréquence donnée, et diminue la nourriture en fonction d'une autre valeur donnée.
    IEnumerator DecreaseHunger()
    {
        while (hungerBar.Progress > 0)
        {
            hungerBar.Progress -= hungerDecreaseAmount;
            yield return new WaitForSeconds(hungerDecreaseRate);
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<ClaireController>().ClaireDead();
    }

    /*Contrôle en fonction de l'axe de mouvement et du type de mouvement, les coûts en énerge à appliquer.
      Si la barre d'énergie atteint 0, on déclare à true le booléen "gameOver".*/
    private void Update()
    {
        if(Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.LeftShift) && !gameOver)
        {
            staminaBar.Progress -= runCost;
            if(staminaBar.Progress == 0)
            {
                gameOver = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<ClaireController>().ClaireDead();
            }
        }
        else if (Input.GetAxis("Vertical") != 0 && !gameOver)
        {
            staminaBar.Progress -= walkCost;
            if (staminaBar.Progress == 0)
            {
                gameOver = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<ClaireController>().ClaireDead();
            }
        }
    }
}

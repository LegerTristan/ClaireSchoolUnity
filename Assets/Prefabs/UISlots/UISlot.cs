using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField]
    private Sprite slotWithChild; // Le sprite de l'enfant sorti de la cage

    [SerializeField]
    private Image[] slots; // Les composants images contenant les sprites qu'on va éditer.

    [SerializeField]
    private GameObject particleExitPoint; // La particule du point de sortie, qui contient aussi le script.

    private int x = 0; // Un index pour vérifier le nombres de slots avec un enfant.
    public bool slotsComplete = false; // Contrôleur booléen pour savoir si tout les enfants ont été libéré.

    /* Lorsque déclenché, on met à jouer un nouveau sprite puis on incrémente l'index en lui appliquant
       des limites afin qu'il n'y ait pas d'erreur.
       Si l'index est égale au nombres de slots, aors on déclare le bontrôleur à true et on affiche le point de sortie.*/
    public void AddChildToSlot()
    {
        slots[x].sprite = slotWithChild;
        x++;
        x = Mathf.Clamp(x, 0, slots.Length);

        if(x == slots.Length)
        {
            slotsComplete = true;
            particleExitPoint.SetActive(true);
        }
    }
}

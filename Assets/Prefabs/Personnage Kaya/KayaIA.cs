using UnityEngine;
using UnityEngine.AI;

public class KayaIA : MonoBehaviour
{
    private NavMeshAgent kayaAgent; // NavMeshAgent de l'ennemi
    private Transform target; // Cible de l'ennemi
    private Animator kayaAnimator; // Animator de l'ennemi
    private AudioSource kayaAudioSource; // Source audio de l'ennemi

    [SerializeField]
    private AudioClip sndDeath; // Son de mort (qui est un son de bulle qui explose)
    private GameObject particle; // particule jouée lors de la mort

    [SerializeField]
    private float idleDistance = 10f, walkDistance = 7f, attackDistance = 1f; // Distance à atteindre pour changer d'état : Assis, debout, marche, attaque

    public float kickDamage = 10f; // Dégâts infligés via l'attaque
    public ProgressBar playerHealthBar; // Référence à la barre de vie du joueur

    // Initialisation des paramètres
    void Start()
    {
        kayaAgent = GetComponent<NavMeshAgent>();
        kayaAnimator = GetComponent<Animator>();
        kayaAudioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        particle = transform.Find("Explode").gameObject;
    }

    /* On met à jour la cible de l'IA, et en fonction de la distance entre lui et le joueur,
     * on édite l'animation de l'ennemi et les actions qui vont avec. */
    void Update()
    {
        kayaAgent.SetDestination(target.position);
        if(kayaAgent.remainingDistance >= idleDistance)
        {
            kayaAgent.speed = 0f;
            kayaAnimator.SetBool("Idle?", false);
        }
        else
        {
            if(kayaAgent.remainingDistance >= walkDistance)
            {
                kayaAnimator.SetBool("Idle?", true);
                kayaAnimator.SetBool("IsWalking?", false);
            }
            else
            {
                if (kayaAgent.remainingDistance >= attackDistance)
                {
                    kayaAgent.speed = 1f;
                    kayaAnimator.SetBool("IsWalking?", true);
                    kayaAnimator.SetBool("IsAttacking?", false);
                }
                else
                {
                    kayaAgent.speed = 0f;
                    kayaAnimator.SetBool("IsAttacking?", true);
                }
            }
        }
    }

    /* On applique des dommages au joueur et lorsque celui-ci n'a plus de vie, on appelle la méthode ClaireDead().
     * Puis on désactive tout les ennemis du niveau, en désactivant leur IA et leur déplacement*/

    public void ApplyDamage()
    {
        playerHealthBar.Progress -= kickDamage;
        GameObject.FindGameObjectWithTag("Player").GetComponent<ClaireController>().GetHurt();

        if(playerHealthBar.Progress <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ClaireController>().ClaireDead();
            GameObject[] ennemies = GameObject.FindGameObjectsWithTag("Ennemy");

            foreach(GameObject ennemy in ennemies)
            {
                ennemy.GetComponent<KayaIA>().enabled = false;
                ennemy.GetComponent<Animator>().SetBool("IsWalking?", false);
                ennemy.GetComponent<Animator>().SetBool("IsAttacking?", false);
                
            }
        }
    }

    /* Lorsque le jouer entre en contact avec le trigger du chapeau, on active les particules, désactive
     * le MeshRenderer et le Collider et puis après quelques secondes on détruit l'ennemi. */
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            particle.SetActive(true);
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            kayaAudioSource.PlayOneShot(sndDeath);
            Destroy(gameObject, sndDeath.length);
        }
    }

    /* Lorsque le joueur est en collision avec le collider principal de l'ennemi,
       alors on déclenche l'animation d'attaque et on réduit à 0 la vitesse de déplacement de l'ennemi.*/

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            kayaAgent.speed = 0f;
            kayaAnimator.SetBool("IsAttacking?", true);
            kayaAnimator.SetBool("IsWalking?", false);
        }
    }
}
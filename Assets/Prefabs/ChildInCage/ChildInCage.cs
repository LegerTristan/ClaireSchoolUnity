using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChildInCage : MonoBehaviour
{

    private GameObject particle; // Particule utilisée lors de l'explosion de la cage
    private NavMeshAgent childAgent; // NavMeshAgent de l'objet
    private Animator childAnimator; // Animator de l'objet
    private AudioSource childAudioSource; // AudioSource de l'objet
    private Transform player; // Transform du joueur

    [SerializeField]
    private Transform target; // Transform du point de destination, une fois la cage explosée.

    [SerializeField]
    private AudioClip sndExplosion; // Son d'explosion

    private bool inCage = true; // Contrôleur booléen pour savoir si l'enfant est dans la cage.

    // Initilisation des paramètres
    private void Start()
    {
        particle = transform.Find("Particle_Explode").gameObject;
        childAgent = GetComponentInChildren<NavMeshAgent>();
        childAnimator = GetComponentInChildren<Animator>();
        childAudioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    /*Si le joueur entre en collision avec la cage, celle-ci explose, retirant son BoxCollider et se détruisant.
      Un son et une particule se jouent, et on met à jour les slots.*/
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            inCage = false;
            childAudioSource.PlayOneShot(sndExplosion);
            particle.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("GameManager").GetComponent<UISlot>().AddChildToSlot();
            Destroy(transform.Find("Cage").gameObject);
        }
    }

    /*Si l'enfant est dans la cage, on lui donne une destination bidon, afin d'éviter des erreurs.
      Sinon on l'envoie vers son point de destination, une fois qu'il l'a atteint, on l'arrête.*/

    private void Update()
    {
        if (inCage)
        {
            childAgent.SetDestination(player.position);
            childAgent.speed = 0f;
        }
        else
        {
            childAgent.SetDestination(target.position);
            childAgent.speed = 5f;
            childAnimator.SetBool("IsRunning?", true);

            if(childAgent.remainingDistance <= childAgent.stoppingDistance)
            {
                childAnimator.SetBool("IsRunning?", false);
                childAgent.speed = 0f;
                childAgent.transform.rotation = target.rotation;
            }
        }
    }
}

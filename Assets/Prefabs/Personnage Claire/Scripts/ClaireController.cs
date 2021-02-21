using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireController : MonoBehaviour {

    private Animator claireAnimator; // Animator du personnage
    private AudioSource claireAudioSource; // AudioSource du personnage
    public CharacterController characterController; // Character Controller du personnage
    public float gravity = -9.81f; // Valeur de la gravité terrestre

    private Vector3 velocity; // Vélocité à appliquer durant les sauts

    public Transform groundChecker; // L'objet qui contrôle si le joueur est au sol, ou non
    public float groundDistance = 0.4f; // Rayon de la sphère invisible de détection du sol
    public LayerMask groundMask; // Filtre de layer pour le contrôle du booléen isGrounded

    private bool isGrounded; // Contrôleur booléen pour savoir si le joueur touche le sol ou non
    private float axisH, axisV; // Axe verticale et horizontale du contrôleur

    [SerializeField]
    private float walkSpeed = 5f, runSpeed = 16f, jumpForce = 1f, turnSmoothTime = .1f; // Respectivement, la vitesse de larche, de course, la force de saut, le temps d'amortissement de la rotation du personnage.
    private float turnSmoothVelocity; // Vélocité de l'amortissemnt de la rotation 

    [SerializeField] private AudioClip sndJump, sndImpact, sndLeftFoot, sndRightFoot, sndHurt, sndDead; // Les sons du personnage
    private bool switchFoot = false; // Contrôleur booléen du son de footstep à jouer

    // Initialisation de certaines variables
    private void Awake()
    {
        claireAnimator = GetComponent<Animator>();
        claireAudioSource = GetComponent<AudioSource>();
    }

    void Update () {

        // On détermine la direction du perso dans un premier temps

        axisH = Input.GetAxis("Horizontal");
        axisV = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(axisH, 0, axisV).normalized;

        // On vérifie également si le personnage est au sol ou non.

        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        // Si le joueur est au sol, on définit la vélocité à -2 pour un meilleur rendu ergonomique
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        /* Si la magnitude du vecteur de direction est supérieur à 0.1, ça veut dire qu'il y a un déplacement
           On contrôle alors si le personnage cours ou non et édite le fonctionement en conséquence.
           On met également à jour la rotation du personnage en fonction de l'angle du vecteur directionnelle par rapport à l'angle de l'axe x (forward).*/
        if(direction.magnitude >= .1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                claireAnimator.SetFloat("run", 1);
                characterController.Move(direction * runSpeed * Time.deltaTime);
            }
            else
            {
                claireAnimator.SetFloat("run", 0);
                claireAnimator.SetBool("walk", true);
                characterController.Move(direction * walkSpeed * Time.deltaTime);
            }
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            claireAnimator.SetBool("walk", false);
        }

        /*On contrôle si le personnage effectue un saut.
         *Le cas échéant, on édite son animation, le son qu'il joue etc.
         *Enfin on applique un déplacement verticale au personnage*/

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            characterController.Move(velocity * Time.deltaTime);
            claireAudioSource.pitch = 1f;
            claireAnimator.SetTrigger("jump");
            claireAudioSource.PlayOneShot(sndJump);
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
              
    }

    // Déclenche un son de mort, et désactive le contrôleur du personnage, synonyme de GameOver
    public void ClaireDead()
    {
        claireAudioSource.pitch = 1f;
        claireAnimator.SetTrigger("dead");
        GetComponent<ClaireController>().enabled = false;
        claireAudioSource.PlayOneShot(sndDead);
    }

    // Son d'impact joué lorsque le joueur touche le sol après un saut
    public void PlaySoundImpact()
    {
        claireAudioSource.pitch = 1f;
        claireAudioSource.PlayOneShot(sndImpact);
    }

    // Sons de déplacement joués lorsque le joueur se déplace
    public void PlayFootStep()
    {
        if(!claireAudioSource.isPlaying)
        {
            switchFoot = !switchFoot;

            if(switchFoot)
            {
                claireAudioSource.pitch = 2f;
                claireAudioSource.PlayOneShot(sndLeftFoot);
            }
            else
            {
                claireAudioSource.pitch = 2f;
                claireAudioSource.PlayOneShot(sndRightFoot);
            }
        }
    }

    // Son de dégâts joué après avoir subi une perte de points de vie.
    public void GetHurt()
    {
        claireAudioSource.pitch = 1f;
        claireAudioSource.PlayOneShot(sndHurt);
    }
}

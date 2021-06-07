using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;    //Si fa riferimento allo script che controlla il personaggio
    public Animator animator;   //Si fa riferimento alla componente che anima il personaggio

    public float runSpeed = 40f;    //Velocità del personaggio

    public float horizontalMove = 0f;  //Distanza percorsa sull'asse orrizzontale

    bool jump = false;
    
    void Update()
    {
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Bandit_Attack"))
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //Per ottenere la distanza percorsa dal personaggio si moltiplica il valore della sua direzione (-1 a sinistra, 1 a destra) per la velocità

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));  //Si assegna al parametro dell'animatore il valore assoluto della distanza percorsa
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("IsJumping", true);//...e viene assegnato "vero" anche a quella per l'animazione del salto 
            }
        }
        else
        {
            horizontalMove = 0;
        }
    }

    public void OnLanding()
    {
        jump = false;
        animator.SetBool("IsJumping", false);   //Si assegna "falso" alla variabile che determina se va riprodotta l'animazione del salto
    }

    //FixedUpdate viene eseguita una sola volta, non importa quante volte venga chiamata nel mentre è in esecuzione
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
    }
}

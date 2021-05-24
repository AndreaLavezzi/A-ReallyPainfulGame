using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Public Variables
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackRange;  //Distanza minima per attaccare
    public float movementSpeed;
    public float attackRate;  //Tempo di distacco tra gli attacchi
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator animator;
    private float distance;  //Distanza tra nemico e giocatore;
    private bool attackMode;
    private bool inRange;  //Controlla se il giocatore è nel raggio d'azione
    private bool cooling; //Controlla se il nemico sta caricando l'attacco
    private float nextAttackTime;
    #endregion

    private void Awake()
    {
        nextAttackTime = attackRate;
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
            RaycastDebugger();
        }
        
        //Quando il giocatore è nel raggio d'azione del nemico...
        if(hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            inRange = false;
        }

        if(inRange == false)
        {
            animator.SetBool("canWalk", false);
            StopAttack();
        }
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > attackRange)
        {
            Move();
            StopAttack();
        }
        else if(attackRange >= distance && cooling == false)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.tag == "Player")
        {
            target = trig.gameObject;
            inRange = true;
            Debug.Log("Giocatore rilevato.");
        }
        else
        {
            return;
        }
    }

    private void Move()
    {
        animator.SetBool("canWalk", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyBandit_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        nextAttackTime = attackRate;  //Quando il giocatore entra nel raggio d'attacco il cooldown dell'attacco del nemico viene resettato
        attackMode = true;  //Finchè il nemico è in modalità di attacco non può attaccare di nuovo

        animator.SetBool("canWalk", false);
        animator.SetBool("Attack", true);
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        animator.SetBool("Attack", false);
    }

    void RaycastDebugger()
    {
        if(distance > attackRange)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if(attackRange > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }
}

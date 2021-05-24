using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    #region Public Variables
    public GameObject projectilePrefab;
    public GameObject pushSpellPrefab;
    public GameObject explosionPrefab;
    public GameObject endScreen;

    public float projectileSpeed = 500f;

    public float nextAttackTime = 0.0f;
    public float attackRate = 10f;
    #endregion

    #region Private Variables
    GameObject[] player;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        float distance = Mathf.Abs(player[0].transform.position.x) - Mathf.Abs(this.gameObject.transform.position.x);
        if (Time.time >= nextAttackTime && Random.Range(0, 9) == 0)
        {
            if(distance <= 10 && distance > 4 && Random.Range(0, 3) == 0)
            {
                Pull_Fire();
                Debug.Log("Distanza: " + distance);
            }else if(distance <= 4 && Random.Range(0, 4) == 0)
            {
                Pull_Explode();
                Debug.Log("Distanza: " + distance);
            }
            else if(distance > 10 && distance <= 14 && Random.Range(0, 2) == 0)
            {
                Pull();
                Debug.Log("Distanza: " + distance);
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Pull()
    {
        GameObject pullSpellCast = Instantiate(pushSpellPrefab, new Vector3(this.gameObject.transform.position.x - 3, this.gameObject.transform.position.y), Quaternion.identity) as GameObject;
    }

    void Fire()
    {
        Instantiate(projectilePrefab, new Vector3(transform.position.x - 3, transform.position.y, -6f), Quaternion.identity);
    }

    void Explode()
    {
        GameObject explosionSpellCast = Instantiate(explosionPrefab, new Vector3(transform.position.x - 1, transform.position.y, -6f), Quaternion.identity) as GameObject;
    }

    void Pull_Fire()
    {
        int projAmount = Random.Range(1, 4);
        Invoke("Pull", 0);
        for(float i = 0.2f; i < projAmount * 0.2f; i = i + 0.2f)
        {
            Invoke("Fire", i);
        }
        
    }

    void Pull_Explode()
    {
        Invoke("Pull", 0);
        Invoke("Explode", 0.3f);
    }

    private void OnDestroy()
    {
        Destroy(player[0].GetComponent<PlayerCombat>());
        Destroy(player[0].GetComponent<PlayerMovement>());
        endScreen.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossalCoreController : MonoBehaviour
{
    #region Public Variables
    public int maxHealth = 100;
    public int damage = 5;
    #endregion

    #region Private Variables
    int currentHealth;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Invoke("Destroy", 30);
    }

    void Destroy()
    {
        GetComponentInParent<Enemy>().Die();
    }
}

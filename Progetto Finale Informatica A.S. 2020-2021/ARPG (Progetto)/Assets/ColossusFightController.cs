using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusFightController : MonoBehaviour
{
    #region Public Variables
    public GameObject playerPrefab;
    #endregion

    #region Private Variables
    GameObject player;
    #endregion

    void Start()
    {
        player = Instantiate(playerPrefab, new Vector3(4, -4), Quaternion.identity) as GameObject;
        player.GetComponentInChildren<Camera>().orthographicSize = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

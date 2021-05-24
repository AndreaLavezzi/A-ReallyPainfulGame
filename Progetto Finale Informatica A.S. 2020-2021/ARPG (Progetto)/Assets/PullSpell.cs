using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullSpell : MonoBehaviour
{
    float velocity = 0.0f;
    float posY;

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        posY = players[0].transform.position.y;
        Invoke("Destroy", 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float newPosX = Mathf.SmoothDamp(player.transform.position.x, gameObject.GetComponentInParent<Transform>().position.x, ref velocity, 0.2f);
            player.transform.position = new Vector3(newPosX, posY, player.transform.position.z);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}

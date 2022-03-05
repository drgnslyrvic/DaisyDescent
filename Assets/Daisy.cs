using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Daisy : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameManager.daisyCount++;
            gameManager.PickupSound();
            this.gameObject.SetActive(false);
        }
        if (other.tag == "Mower")
        {
            this.gameObject.SetActive(false);
        }
    }
}

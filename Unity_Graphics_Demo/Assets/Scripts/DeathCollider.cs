using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCollider : MonoBehaviour
{
    BoxCollider DeathBox;
    // Start is called before the first frame update
    void Start()
    {
        DeathBox = this.GetComponent<BoxCollider>();
    }


    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

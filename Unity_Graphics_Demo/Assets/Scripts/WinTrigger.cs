using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public Canvas WinUI;
    public Transform World;
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        WinUI.gameObject.SetActive(true);
        World.gameObject.SetActive(false);
    }
}

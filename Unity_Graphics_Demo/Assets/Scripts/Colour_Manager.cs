using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Colour_Manager : MonoBehaviour
{
    // the top object in the heirachy with an empty child for each color which are parents for all platforms of their color.
    public Transform platParent;
    // List of platforms (Transforms or game objects?)
    public List<Transform> platforms;
    // Reference to the player Transform or material
    public Material playerMat;
    // References to the color buttons
    public Button Red, Green, Blue;
    // int that records the previous set colour
    int precol = 0;


    // Start is called before the first frame update
    void Awake()
    {
        // Code to populate the platforms array
        foreach (Transform col in platParent)
        {
            foreach (Transform platform in col)
            {
                if (platform.GetComponent<MeshRenderer>())
                {
                    platforms.Add(platform);
                }
            }
        }

        //

        // ensure the player start color is white
        playerMat.color = UnityEngine.Color.white;

        Red.onClick.AddListener(delegate { ChangeColor(1); });
        Green.onClick.AddListener(delegate { ChangeColor(2); });
        Blue.onClick.AddListener(delegate { ChangeColor(3); });
    }



    /// <summary>
    /// A function that takes an int based on a button press and using that int changes the player color
    /// and makes platforms of that color solid.
    /// This needs to be refined if I want to add more colours or colour combinations.
    /// </summary>
    /// <param name="color"></param>
    void ChangeColor(int color)
    {
        // early return if the same button is pressed more than once.
        // however I could also make a white state where all platforms are ghosts
        // could be good if I want to make walls/barriers
        if (color == precol) return;


        string colTag = GetColTag(color);
        string preColTag = GetColTag(precol);

        // if color is red make the red platforms solid if precol is red
        // then the platforms are solid so ghost them.

        foreach (Transform t in platforms)
        {
            if (t.CompareTag(colTag))
            {
                SwitchPlatformState(t);
            }
            if (preColTag != "Unset" && t.CompareTag(preColTag))
            {
                SwitchPlatformState(t);
            }
        }

        ChangePlayerColor(color);


        precol = color;
    }





    /// <summary>
    ///  Swaps the active platform between Solid and ghost states.
    ///  The parent platform has no colliders so deactivating it's mesh render component makes it invisible
    ///  whereas the child platform has a collider for the player to stand on 
    /// </summary>
    /// <param name="platform"></param>
    void SwitchPlatformState(Transform platform)
    {
        // gets a reference to the child platform
        Transform SolidPlat = platform.GetChild(0);

        // if the platform is solid activate the MeshRenderer on the trans platform and disable the solid platform
        if (SolidPlat.gameObject.activeInHierarchy)
        {
            platform.GetComponent<MeshRenderer>().enabled = true;
            SolidPlat.gameObject.SetActive(false);
        }
        else
        {
            SolidPlat.gameObject.SetActive(true);
            platform.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// The alternative to this is doing an if statement twice for every colour 
    /// </summary>
    /// <param name="colIndex"></param>
    /// <returns></returns>
    string GetColTag(int colIndex)
    {
        string colTag;
        switch (colIndex)
        {
            case 1:
                colTag = "Red";
                break;
            case 2:
                colTag = "Green";
                break;
            case 3:
                colTag = "Blue";
                break;
            default:
                colTag = "Unset";
                break;
        }
        return colTag;
    }

    void ChangePlayerColor(int col)
    {
        switch (col)
        {
            case 1:
                playerMat.color = UnityEngine.Color.red;
                break;
            case 2:
                playerMat.color = UnityEngine.Color.green;
                break;
            case 3:
                playerMat.color = UnityEngine.Color.blue;
                break;
            default:
                playerMat.color = UnityEngine.Color.white;
                break;
        }
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Egg : MonoBehaviour
{
    [SerializeField] private Dictionary<string, Slider> attributeSliders;
    public Dictionary<string, float> attributeValues;
    private placementManager placementManager;

    private bool grabbable = false;
    private bool grabbed = false;

    void Start()
    {
        placementManager = GameObject.Find("placementManager").GetComponent<placementManager>();
    }

    void Update()
    {
        //If mouse over and mouse pressed
        if (grabbable && Input.GetMouseButton(1))
        {
            //Egg grabbed
            grabbed = true;

        }

        //If egg already grabbed and mouse still down - allows for mouse to move off egg and still follow
        if (grabbed == true && Input.GetMouseButton(1))
        {
            //Move egg to mouse pos
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //If grabbed but mouse released, means egg put down
        else if (grabbed == true && !Input.GetMouseButton(1))
        {

        }
    }

    //Ran when egg put down to find closest possible placement position
    GameObject getClosestTraySlot()
    {
        
    }

    //Mouse enters egg's sprite
    void OnMouseEnter()
    {
        grabbable = true;
    }

    //Mouse leaves egg's sprite
    void OnMouseExit()
    {
        grabbable = false;
    }
}

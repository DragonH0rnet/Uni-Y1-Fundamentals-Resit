using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Egg : MonoBehaviour
{
    [SerializeField] private Slider tempSlider;
    [SerializeField] private Slider humidSlider;
    [SerializeField] private Slider turnSlider;
    [SerializeField] private Slider timeSlider;

    public float temp;
    public float humid;
    public float turn;
    public float time;

    public float maxTemp;
    public float maxHumid;
    public float maxTurn;
    public float maxTime;

    private placementManager placementManager;

    //specify ranges for values: [smallest, highest, variance factor]
    // variance factor will randomise the smalles and highest values by this amount e.g. [3,10,2] could create a range of 1-12 or 5-8 as extremes but could also do 5-12 and raise the entire accepted range
    [SerializeField] private float[] heatAcceptRange;
    [SerializeField] private float[] humidAcceptRange;

    //Time just has [max, variance] since it has an absolute end and no range is needed
    [SerializeField] public float[] timeAccept;

    [SerializeField] private GameObject tempStatus;
    [SerializeField] private GameObject humidStatus;

    private bool grabbable = false;
    private bool grabbed = false;

    void Start()
    {
        placementManager = GameObject.Find("placementManager").GetComponent<placementManager>();

        //set acceptable values using variance
        heatAcceptRange[0] = heatAcceptRange[0] + UnityEngine.Random.Range(-heatAcceptRange[2], heatAcceptRange[2]);
        heatAcceptRange[1] = heatAcceptRange[1] + UnityEngine.Random.Range(-heatAcceptRange[2], heatAcceptRange[2]);

        humidAcceptRange[0] = humidAcceptRange[0] + UnityEngine.Random.Range(-humidAcceptRange[2], humidAcceptRange[2]);
        humidAcceptRange[1] = humidAcceptRange[1] + UnityEngine.Random.Range(-humidAcceptRange[2], humidAcceptRange[2]);

        timeAccept[0] = timeAccept[0] + UnityEngine.Random.Range(-timeAccept[1], timeAccept[1]);

        temp = heatAcceptRange[1];
        humid = humidAcceptRange[1];
        turn = 30;
    }

    void Update()
    {
        //If mouse over and mouse pressed and egg not already picked up
        if (grabbable && Input.GetMouseButton(0) && placementManager.eggPickedUp == false)
        {
            //Egg grabbed
            grabbed = true;
            placementManager.eggPickedUp = true;

        }

        //If egg already grabbed and mouse still down - allows for mouse to move off egg and still follow
        if (grabbed == true && Input.GetMouseButton(0))
        {
            //Move egg to mouse pos
            transform.position = new UnityEngine.Vector3(0, 0, 10) + Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.Rotate(0, 0, transform.rotation.z * -4);
        }
        //If grabbed but mouse released, means egg put down
        else if (grabbed == true && !Input.GetMouseButton(0))
        {
            grabbed = false;
            placementManager.eggPickedUp = false;
            //Get closest slot
            GameObject slotToPlaceAt = getClosestTraySlot();
            //Move to closest slot
            this.transform.position = slotToPlaceAt.transform.position;
            transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);
            //Assign slot to this egg
            slotToPlaceAt.GetComponent<eggSlot>().attachedEgg = this.GameObject();
        }

        //Variable check
        //

        //humidity
        if (humid < humidAcceptRange[1] && humid > humidAcceptRange[0])
        {
            //Set status to valid and add to hatch meter
            humidStatus.GetComponent<SpriteRenderer>().color = Color.green;
            time += Time.deltaTime / 3;

        }
        else
        {
            humidStatus.GetComponent<SpriteRenderer>().color = Color.red;
            time -= Time.deltaTime / 3;
        }

        //temperature
        if (temp < heatAcceptRange[1] && temp > heatAcceptRange[0])
        {
            tempStatus.GetComponent<SpriteRenderer>().color = Color.green;
            time += Time.deltaTime / 3;
        }
        else
        {
            tempStatus.GetComponent<SpriteRenderer>().color = Color.red;
            time -= Time.deltaTime / 3;
        }

        //turn

        if (turn > 0)
        {
            time += Time.deltaTime / 3;
        }
        else
        {
            time -= Time.deltaTime / 3;
            turn -= Time.deltaTime;
        }

        //apply attributes to sliders
        //

        tempSlider.value = temp;
        humidSlider.value = humid;
        turnSlider.value = turn;
        timeSlider.value = time;

    }

    //Ran when egg put down to find closest possible placement position
    GameObject getClosestTraySlot()
    {
        //set inital small distance very high so always replaced
        float smallestDistance = 1000f;
        GameObject closestSlot = null;

        //iterate through each tray
        foreach (GameObject tray in placementManager.eggTrayList)
        {
            //iterate through each trays slots
            foreach (GameObject slot in tray.GetComponent<Egg_tray>().PLacementPositions)
            {
                //find distance from egg to slot, if slot closer than previous closest slot replace previous closest with this slot
                //checks if egg slot is empty
                float distance = UnityEngine.Vector3.Distance(this.transform.position, slot.transform.position);
                if (distance < smallestDistance && slot.GetComponent<eggSlot>().attachedEgg == null)
                {
                    smallestDistance = distance;
                    closestSlot = slot;
                }
            }
        }

        return closestSlot;
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

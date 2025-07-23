using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class Egg_tray : MonoBehaviour
{
    [SerializeField] private bool allowEggsIn = false;
    [SerializeField] private bool allowEggsOut = false;
    [SerializeField] public List<GameObject> PLacementPositions;

    [SerializeField] private float rateOfChange = 2;

    enum trayType { Neutral, Temperature, Humidity, Turn, In, Out };

    [SerializeField] private trayType trayTypeEnum;

    [SerializeField] private GameObject scoreManager;

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager");
    }

    void Update()
    {
        //repeat for each egg in the tray
        foreach (GameObject egg in getEggsInTray())
        {
            Egg eggScript = egg.GetComponent<Egg>();
            //if humidity tray
            if (trayTypeEnum == trayType.Humidity)
            {
                if (eggScript.humid >= eggScript.maxHumid)
                {
                    eggScript.humid = eggScript.maxHumid;
                }
                else
                {
                    eggScript.humid += Time.deltaTime * 4;
                }
                eggScript.temp -= Time.deltaTime * 0.6f;
                eggScript.turn -= Time.deltaTime;


            }

            //if temperature tray
            else if (trayTypeEnum == trayType.Temperature)
            {
                if (eggScript.temp >= eggScript.maxTemp)
                {
                    eggScript.temp = eggScript.maxTemp;
                }
                else
                {
                    eggScript.temp += Time.deltaTime * 2;
                }
                eggScript.humid -= Time.deltaTime;
                eggScript.turn -= Time.deltaTime;
            }

            //if rotate tray
            else if (trayTypeEnum == trayType.Turn)
            {
                eggScript.turn = 30;
                egg.transform.Rotate(0, 0, Time.deltaTime * 800);

                eggScript.temp -= Time.deltaTime * 0.6f;
                eggScript.humid -= Time.deltaTime;
            }

            //if any other tray, doesn't increase any attribute
            else
            {
                eggScript.temp -= Time.deltaTime * 0.6f;
                eggScript.humid -= Time.deltaTime;
                eggScript.turn -= Time.deltaTime;
            }
            
            //stop attributes from going below 0
            eggScript.temp = math.clamp(eggScript.temp, 0, 1000);
            eggScript.humid = math.clamp(eggScript.humid, 0, 1000);
            eggScript.turn = math.clamp(eggScript.turn, 0, 1000);
            eggScript.time = math.clamp(eggScript.time, 0, 1000);

            //if egg ready, delete it and add score
            if (trayTypeEnum == trayType.Out)
            {
                if (eggScript.time >= eggScript.timeAccept[0])
                {

                    GameObject.Destroy(egg);
                    scoreManager.GetComponent<scoreManager>().Score += 1;
                }
            }
        }
    }

    //get list of all eggs in tray
    List<GameObject> getEggsInTray()
    {
        List<GameObject> output = new List<GameObject>();
        foreach (GameObject slot in PLacementPositions)
        {
            GameObject attached = slot.GetComponent<eggSlot>().attachedEgg;
            if (attached != null)
            {
                output.Add(slot.GetComponent<eggSlot>().attachedEgg);
            }
        }

        foreach (GameObject egg in output)
        {
            Debug.Log(egg.name);
        }
        return output;
    }

}

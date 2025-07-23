using UnityEditor.UI;
using UnityEngine;

public class spawnManager : MonoBehaviour
{

    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private GameObject spawnTray;
    private Egg_tray trayScript;

    [SerializeField] private float cooldownCount;
    [SerializeField] private float cooldownTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trayScript = spawnTray.GetComponent<Egg_tray>();
    }

    // Update is called once per frame
    void Update()
    {
        //cooldown complete
        if (cooldownCount <= 0)
        {
            //reset count
            cooldownCount = cooldownTime;

            //get next slot
            GameObject freeSlot = findNextAvaliableSlot();

            if (freeSlot != null)
            {
                //create egg
                GameObject newEgg = Instantiate(eggPrefab);

                //put new egg in position
                newEgg.transform.position = freeSlot.transform.position;

                //attach new egg to slot
                freeSlot.GetComponent<eggSlot>().attachedEgg = newEgg;
            }
        }
        //cooldown not done
        else
        {
            //progress cooldown
            cooldownCount -= Time.deltaTime;
        }
    }

    //get the next slot which doesn't have an egg in it to spawn
    GameObject findNextAvaliableSlot()
    {
        bool foundEmpty = false;

        GameObject output = null;

        //iterate through all slots
        foreach (GameObject slot in trayScript.PLacementPositions)
        {
            eggSlot slotScript = slot.GetComponent<eggSlot>();
            //if slot is empty end loop and log the slot for output
            if (slotScript.attachedEgg == null)
            {
                foundEmpty = true;
                output = slot;
                break;
            }
        }
        //if no empty slots return null, otherwhise it will be overwritten by the slot
        return output;
    }

}

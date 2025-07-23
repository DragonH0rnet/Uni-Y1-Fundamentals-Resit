using UnityEditor.UI;
using UnityEngine;

public class eggSlot : MonoBehaviour
{

    public GameObject attachedEgg = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attachedEgg != null)
        {
            if (Vector3.Distance(attachedEgg.transform.position, this.transform.position) > 0.5f)
        {
            attachedEgg = null;
        }
        }
    }
}

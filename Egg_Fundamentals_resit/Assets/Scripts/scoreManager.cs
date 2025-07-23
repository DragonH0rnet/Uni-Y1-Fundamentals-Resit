using TMPro;
using UnityEngine;

public class scoreManager : MonoBehaviour
{
    public int Score = 0;
    [SerializeField] private TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + Score;
    }
}

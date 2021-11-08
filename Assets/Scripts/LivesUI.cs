using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    public Text livesText;

    // Update is called once per frame
    void Update()
    {
        // no need to use ToString to convert PlayerStats.Lives from int to string because we are adding a string at the end
        // unity automatically converts it to string
        livesText.text = PlayerStats.Lives.ToString() + " LIVES";
    }
}

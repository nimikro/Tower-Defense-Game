using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundsSurvived : MonoBehaviour
{
    public Text roundsText;

    // onEnable is called everytime the object is enabled, not only at the beginning like start does
    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    // show rounds text counting up
    IEnumerator AnimateText()
    {
        roundsText.text = "0";
        int round = 0;
        // wait for fade in
        yield return new WaitForSeconds(0.7f);

        while(round < PlayerStats.Rounds)
        {
            round++;
            roundsText.text = round.ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }
}

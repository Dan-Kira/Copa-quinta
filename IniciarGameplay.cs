using System.Collections;
using TMPro;
using UnityEngine;

public class IniciarGameplay : MonoBehaviour
{
    public TextMeshProUGUI[] initialTexts;
    public float countdownTime = 1f;
    public bool canIniciate = false;
    
    void Awake()
    {
        Time.timeScale = 0f;

        DisableAllTexts();
    }

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        for (int i = 0; i < initialTexts.Length; i++)
        {
            DisableAllTexts();

            initialTexts[i].gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(countdownTime);
        }

        DisableAllTexts();

        Time.timeScale = 1f;

        canIniciate = true;
    }

    void DisableAllTexts()
    {
        for (int i = 0; i < initialTexts.Length; i++)
        {
            initialTexts[i].gameObject.SetActive(false);
        }
    }
}

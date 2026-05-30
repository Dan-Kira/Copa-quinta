using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinish : MonoBehaviour
{
    public static GameFinish Instance;

    public List<RaceResult> results = new List<RaceResult>();

    public int maxPlayers = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ArmazenateInfos(float scoreRecebido, string nomeRecebido)
    {
        RaceResult newResult = new RaceResult();

        newResult.playerName = nomeRecebido;
        newResult.score = scoreRecebido;

        results.Add(newResult);

        if(results.Count >= maxPlayers)
        {
            FinishRace();
        }
    }

    void FinishRace()
    {
        results.Sort((a, b) => a.score.CompareTo(b.score));

        SceneManager.LoadScene(3);
    }
}

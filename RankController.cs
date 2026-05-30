using UnityEngine;

public class RankController : MonoBehaviour
{
    public RankPosition[] rankPositions;

    private void Start()
    {
        UpdateRank();
    }

    public void UpdateRank()
    {
        var results = GameFinish.Instance.results;

        for(int i = 0; i < rankPositions.Length; i++)
        {
            if(i >= results.Count)
                break;

            rankPositions[i].posText.text = (i + 1) + "º";

            rankPositions[i].scoreText.text = results[i].score.ToString("F2") + " seg";

            rankPositions[i].nameText.text = results[i].playerName;
        }
    }
}

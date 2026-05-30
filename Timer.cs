using UnityEngine;

public class Timer : MonoBehaviour
{
    public float currentTime;
    public bool isUnder3Min = true;

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= 180)
        {
            isUnder3Min = false;
        }
    }
}

using UnityEngine;

public class MultiplayerCamera : MonoBehaviour
{
    [Header("Câmeras")]
    public Camera cameraP1;
    public Camera cameraP2;

    [Header("Jogadores")]
    public Transform player1;
    public Transform player2;

    [Header("Configurações")]
    public float splitThreshold = 8f;
    public float splitSpeed = 3f;
    public bool isHorizontalSplit = false;

    [Header("Zoom")]
    public float baseCamSize = 5f;
    public float maxZoomOut = 1.4f;

    [Header("Linha Divisória")]
    public RectTransform splitLine;
    public float lineWidth = 4f;

    private bool isSplit = false;
    private float currentSplitAmount = 0f;

    void Update()
    {
        float distance = Vector2.Distance(player1.position, player2.position);
        bool shouldSplit = distance > splitThreshold;

        float targetSplit = shouldSplit ? 1f : 0f;
        currentSplitAmount = Mathf.MoveTowards(currentSplitAmount, targetSplit, splitSpeed * Time.deltaTime);

        isSplit = currentSplitAmount > 0.01f;
        cameraP2.gameObject.SetActive(isSplit);

        UpdateViewports();
        UpdateCameraPositions(distance);
        UpdateSplitLine();
    }

    public void UpdateViewports()
    {
        if (!isHorizontalSplit)
        {
            float splitPoint = 0.5f;
            float p1Width = Mathf.Lerp(1f, splitPoint, currentSplitAmount);
            float p2Start = Mathf.Lerp(1f, splitPoint, currentSplitAmount);

            cameraP1.rect = new Rect(0, 0, p1Width, 1f);
            cameraP2.rect = new Rect(p2Start, 0, 1f - p2Start, 1f);
        }
        else
        {
            float p1Height = Mathf.Lerp(1f, 0.5f, currentSplitAmount);
            float p2Start  = Mathf.Lerp(0f, 0.5f, currentSplitAmount);

            cameraP1.rect = new Rect(0, p2Start, 1f, p1Height);
            cameraP2.rect = new Rect(0, 0, 1f, 1f - p2Start);
        }
    }

    public void UpdateCameraPositions(float distance)
    {
        Vector3 midPoint = (player1.position + player2.position) / 2f;
        midPoint.z = cameraP1.transform.position.z;

        if (!isSplit)
        {
            float distNormalized = Mathf.Clamp01(distance / splitThreshold);
            float targetSize = baseCamSize * Mathf.Lerp(1f, maxZoomOut, distNormalized);
            cameraP1.orthographicSize = Mathf.Lerp(cameraP1.orthographicSize, targetSize, Time.deltaTime * 5f);

            cameraP1.transform.position = Vector3.Lerp(cameraP1.transform.position, midPoint, Time.deltaTime * 5f);
        }
        else
        {
            cameraP1.orthographicSize = Mathf.Lerp(cameraP1.orthographicSize, baseCamSize, Time.deltaTime * 5f);
            cameraP2.orthographicSize = Mathf.Lerp(cameraP2.orthographicSize, baseCamSize, Time.deltaTime * 5f);

            Vector3 targetP1 = player1.position;
            Vector3 targetP2 = player2.position;
            targetP1.z = cameraP1.transform.position.z;
            targetP2.z = cameraP2.transform.position.z;

            cameraP1.transform.position = Vector3.Lerp(cameraP1.transform.position, targetP1, Time.deltaTime * 5f);
            cameraP2.transform.position = Vector3.Lerp(cameraP2.transform.position, targetP2, Time.deltaTime * 5f);
        }
    }

    public void UpdateSplitLine()
    {
        if (splitLine == null) return;

        splitLine.gameObject.SetActive(isSplit);

        if (!isSplit) return;

        if (!isHorizontalSplit)
        {
            float screenX = Mathf.Lerp(Screen.width, Screen.width * 0.5f, currentSplitAmount);
            splitLine.anchoredPosition = new Vector2(screenX - Screen.width * 0.5f, 0);
            splitLine.sizeDelta = new Vector2(lineWidth, Screen.height);
        }
        else
        {
            float screenY = Mathf.Lerp(0, Screen.height * 0.5f, currentSplitAmount);
            splitLine.anchoredPosition = new Vector2(0, screenY - Screen.height * 0.5f);
            splitLine.sizeDelta = new Vector2(Screen.width, lineWidth);
        }
    }
}

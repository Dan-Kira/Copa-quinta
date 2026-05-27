using TMPro;
using UnityEngine;

public class LinhaFinal : MonoBehaviour
{
    public TextMeshProUGUI textoFinal;

    void Awake()
    {
        textoFinal.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            textoFinal.gameObject.SetActive(true);
            textoFinal.text = "Você ganhou";
        }
    }
}

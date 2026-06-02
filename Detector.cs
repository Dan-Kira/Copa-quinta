using UnityEngine;

public class Detector : MonoBehaviour
{
    public float raioDeDeteccao = 3f;
    public LayerMask camadaDoJogador;

    private Collider2D[] resultados = new Collider2D[1];

    void Update()
    {
        // Detecta se o jogador está dentro do raio deste ponto
        int detectou = Physics2D.OverlapCircleNonAlloc(transform.position, raioDeDeteccao, resultados, camadaDoJogador);

        if (detectou > 0)
        {
            // Tenta pegar o script de controle do jogador que foi detectado
            if (resultados[0].TryGetComponent(out ControleHabilidadeJogador jogador))
            {
                // Libera a habilidade no jogador e passa o MEU transform para ele
                jogador.LiberarHabilidade(transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioDeDeteccao);
    }
}

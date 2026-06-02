using UnityEngine;

public class ControleHabilidadeJogador : MonoBehaviour
{
    [Header("Componentes de Física e Visual")]
    public SpringJoint2D m_springJoint;
    public LineRenderer m_lineRenderer;

    [Header("Armazenamento Temporário (Leitura)")]
    public Transform pontoAlvoDisponivel;
    public bool podeUsarHabilidade;

    private bool estaBalancando;

    void Start()
    {
        // Garante que o Joint e a Linha comecem desligados
        if (m_springJoint) m_springJoint.enabled = false;
        if (m_lineRenderer) m_lineRenderer.positionCount = 0;
    }

    // Esta função continua sendo chamada pelo script do Ponto de Ancoragem
    public void LiberarHabilidade(Transform transformDoPonto)
    {
        pontoAlvoDisponivel = transformDoPonto;
        podeUsarHabilidade = true;
    }

    void Update()
    {
        // Se apertar Espaço, estiver no range e NÃO estiver balançando: CONECTA
        if (Input.GetKeyDown(KeyCode.Space) && podeUsarHabilidade && !estaBalancando)
        {
            IniciarBalanco();
        }
        // Se soltar o Espaço ou apertar de novo enquanto balança: DESCONECTA
        else if (Input.GetKeyUp(KeyCode.Space) && estaBalancando)
        {
            PararBalanco();
        }

        // Se a corda estiver ativa, atualiza as posições da linha a cada frame
        if (estaBalancando)
        {
            DesenharCorda();
        }

        // Reseta a liberação a cada frame (se sair do raio, o ponto limpa a flag)
        ResetarVerificacaoTemporaria();
    }

    void IniciarBalanco()
    {
        if (pontoAlvoDisponivel == null) return;

        estaBalancando = true;

        // 1. Configura e liga o SpringJoint2D no ponto capturado
        m_springJoint.connectedAnchor = pontoAlvoDisponivel.position;
        m_springJoint.enabled = true;

        // 2. Configura a linha visual (2 pontos: jogador e âncora)
        m_lineRenderer.positionCount = 2;
    }

    void DesenharCorda()
    {
        if (pontoAlvoDisponivel == null) return;

        // Ponto 0 da linha segue a posição atual do jogador
        m_lineRenderer.SetPosition(0, transform.position);

        // Ponto 1 da linha fixa na posição do ponto de apoio
        m_lineRenderer.SetPosition(1, pontoAlvoDisponivel.position);
    }

    void PararBalanco()
    {
        estaBalancando = false;
        m_springJoint.enabled = false;
        m_lineRenderer.positionCount = 0; // Apaga a linha
    }

    void ResetarVerificacaoTemporaria()
    {
        // Só limpa o alvo se o jogador não estiver ativamente pendurado nele
        if (!estaBalancando)
        {
            podeUsarHabilidade = false;
            pontoAlvoDisponivel = null;
        }
    }
}

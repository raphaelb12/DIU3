using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Jogador : MonoBehaviour
{
    private CharacterController controladorPersonagem;
    private Vector3 movimento;

    public float forcaPulo = 24f; // Força do pulo
    public float gravidade = 9.81f * 2f; // Aumenta a gravidade
    public float forcaVoo = 5f; // Força do voo
    private bool voando = false; // Controla se o jogador está voando

    private void Awake()
    {
        controladorPersonagem = GetComponent<CharacterController>(); // Pega o CharacterController do jogador
    }

    private void OnEnable()
    {
        movimento = Vector3.zero; // Zera o movimento
    }

    private void Update()
    {
        if (voando)
        {
            // Aplica o movimento vertical para manter o jogador fixo em y = 4
            movimento = Vector3.up * forcaVoo; // Aplica o movimento vertical para o voo
            // Nenhuma gravidade é aplicada durante o voo
            // movimento += gravidade * Time.deltaTime * Vector3.down; // Gravidade desabilitada durante o voo

            // Mantém o personagem fixo em y = 4 enquanto estiver voando
            Vector3 posicaoAtual = transform.position;
            posicaoAtual.y = 4f;  // Mantém a altura fixada em y = 4
            transform.position = posicaoAtual;
        }
        else
        {
            movimento += gravidade * Time.deltaTime * Vector3.down; // Aplica a gravidade normal

            if (controladorPersonagem.isGrounded) // Se o jogador estiver no chão
            {
                movimento = Vector3.down; // Zera o movimento

                if (Input.GetButton("Jump")) // Se o jogador apertar o botão de pulo
                {
                    movimento = Vector3.up * forcaPulo; // Faz o jogador pular
                }
            }
        }

        controladorPersonagem.Move(movimento * Time.deltaTime); // Move o jogador
    }

    // Função para ativar o voo
    public void AtivarVoo()
    {
        voando = true; // Marca que o jogador está voando
    }

    // Função para desativar o voo
    public void DesativarVoo()
    {
        voando = false; // Marca que o jogador não está mais voando
    }

    private void OnTriggerEnter(Collider other) // Quando o jogador colidir com um obstáculo
    {
        if (other.CompareTag("Obstaculo"))
        {
            GameManager.Instance.GameOver(); 
        }
    }
}
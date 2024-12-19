using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float velocidadeInicial = 5f;
    public float taxaDeAumentoDeVelocidade = 0.1f;
    public float Velocidade { get; private set; }

    public TextMeshProUGUI TextoGameOver;
    public Button Button;
    public TextMeshProUGUI Pontos;
    public TextMeshProUGUI TextoHiscore; // Adicionado para exibir o hiscore

    public Camera MainCamera;

    private Jogador jogador;
    private Spawner spawner;

    private float pontuacao = 0f;
    private float hiscore = 0f; // Variável para armazenar o hiscore
    private int proximaTrocaDeCor = 100;

    public GameObject menuInicial;  // Menu inicial
    public GameObject menuPausa;    // Menu de pausa

    private bool jogoPausado = false;

    private bool vooAtivado = false;  // Controla se o voo está ativado
    private int[] pontosVooInicio = { 150, 300, 450, 600, 750, 900 }; // Pontuação de início de voo
    private int[] pontosVooFim = { 180, 330, 480, 630, 780, 930 }; // Pontuação de fim de voo

    private int vooIndex = 0; // Controla o índice da próxima sequência de voo

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Garante que só exista uma instância do GameManager
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        jogador = FindObjectOfType<Jogador>(); // Encontra o jogador na cena
        spawner = FindObjectOfType<Spawner>(); // Encontra o spawner na cena

        Velocidade = velocidadeInicial;

        // Carrega o hiscore salvo
        hiscore = PlayerPrefs.GetFloat("hiscore", 0);
        AtualizarTextoHiscore();

        // Exibe o menu inicial e pausa o jogo
        menuInicial.SetActive(true);
        TextoGameOver.gameObject.SetActive(false);
        Button.gameObject.SetActive(false);
        Pontos.gameObject.SetActive(false);
        Time.timeScale = 0; // Pausa o jogo enquanto o menu inicial está ativo
    }

    private void Update()
    {
        // Aumenta a pontuação com o tempo
        pontuacao += Time.deltaTime * 5; 
        Pontos.text = Mathf.FloorToInt(pontuacao).ToString();

        // Verifica se o jogador atingiu a pontuação para voar
        if (!vooAtivado && Mathf.FloorToInt(pontuacao) >= pontosVooInicio[vooIndex])
        {
            AtivarVoo();
        }

        // Verifica se o jogador atingiu a pontuação para voltar ao solo
        if (vooAtivado && Mathf.FloorToInt(pontuacao) >= pontosVooFim[vooIndex])
        {
            DesativarVoo();
        }

        // Verifica se é hora de trocar a cor
        if (Mathf.FloorToInt(pontuacao) >= proximaTrocaDeCor)
        {
            TrocarCorDeFundo();
            proximaTrocaDeCor += 100; // Atualiza a próxima pontuação alvo
        }

        // Verifica se o jogador pressionou o botão de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TrocarCorDeFundo()
    {
        // Gera uma cor aleatória para o fundo
        Color novaCor = new Color(Random.value, Random.value, Random.value);
        MainCamera.backgroundColor = novaCor;
    }

    // Função que ativa o voo
    private void AtivarVoo()
    {
        vooAtivado = true; // Marca que o voo foi ativado
        jogador.AtivarVoo(); // Chama a função do Jogador para começar a voar
    }

    // Função que desativa o voo
    private void DesativarVoo()
    {
        vooAtivado = false; // Marca que o voo foi desativado
        jogador.DesativarVoo(); // Chama a função do Jogador para parar de voar
        vooIndex++; // Avança para a próxima sequência de voo
        if (vooIndex >= pontosVooInicio.Length) vooIndex = 0; // Reinicia se o último voo foi atingido
    }

    public void NewGame()
    {
        Obstaculos[] obstaculos = FindObjectsOfType<Obstaculos>();

        foreach (var obstaculo in obstaculos)
        {
            Destroy(obstaculo.gameObject); // Destroi todos os obstáculos na cena
        }

        Velocidade = velocidadeInicial;
        pontuacao = 0f;
        proximaTrocaDeCor = 100;

        menuInicial.SetActive(false);
        Pontos.gameObject.SetActive(true);
        Time.timeScale = 1; // Retoma o jogo
        enabled = true;

        jogador.gameObject.SetActive(true); // Ativa o jogador
        spawner.gameObject.SetActive(true); // Ativa o spawner

        TextoGameOver.gameObject.SetActive(false); // Desativa o texto de game over
        Button.gameObject.SetActive(false); // Desativa o botão de game over
    }

    public void GameOver()
    {
        Velocidade = 0f;
        enabled = false;
        jogador.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        // Atualiza o hiscore se necessário
        if (pontuacao > hiscore)
        {
            hiscore = pontuacao;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        TextoGameOver.gameObject.SetActive(true);
        Button.gameObject.SetActive(true);
        Pontos.gameObject.SetActive(false);

        AtualizarTextoHiscore();
    }

    private void AtualizarTextoHiscore()
    {
        TextoHiscore.text = $"Recorde: {Mathf.FloorToInt(hiscore)}";
    }

    public void TogglePauseMenu()
    {
        jogoPausado = !jogoPausado;
        menuPausa.SetActive(jogoPausado);
        Time.timeScale = jogoPausado ? 0 : 1; // Pausa ou retoma o tempo no jogo
    }

    public void QuitGame()
    {
        Application.Quit(); // Fecha o aplicativo
    }
}
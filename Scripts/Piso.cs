using UnityEngine;

public class Piso : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // Atribui o componente MeshRenderer
    }

    private void Update(){
        float velocidade = GameManager.Instance.Velocidade / transform.localScale.x;
        meshRenderer.material.mainTextureOffset += Vector2.right * velocidade * Time.deltaTime; 
    } // Move o piso de acordo com a velocidade do jogo
}

using UnityEngine;

public class Obstaculos : MonoBehaviour
{
    private float borda;

    private void Start(){
        borda = Camera.main.ViewportToWorldPoint(Vector3.zero).x -1f; // Pega a borda da tela
    }
    
    private void Update(){ 
        transform.position += Vector3.left * GameManager.Instance.Velocidade * Time.deltaTime;

        if(transform.position.x < borda){
            Destroy(gameObject);
        }   
    } // Move o obstáculo para a esquerda

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Obstaculo")){
            GameManager.Instance.GameOver();
        }
    } // Quando o jogador colidir com um obstáculo
}

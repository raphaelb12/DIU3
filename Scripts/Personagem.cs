using UnityEngine;

public class Personagem : MonoBehaviour
{
 public Sprite[] sprites;

 private SpriteRenderer spriteRenderer;
 private int frame;

private void AnimarPersonagem() // Método para animar o personagem
{
    frame = (frame + 1);
    if(frame >= sprites.Length)
    {
        frame = 0;
    }

    if (frame >= 0 && frame < sprites.Length) // Verifica se o frame está dentro do array
    {spriteRenderer.sprite = sprites[frame];} // Altera o sprite do personagem

    Invoke(nameof(AnimarPersonagem), 1f / GameManager.Instance.Velocidade); 

}

 
 private void Awake()
 {
     spriteRenderer = GetComponent<SpriteRenderer>();
 }

private void OnEnable() {
    Invoke(nameof(AnimarPersonagem), 0.1f); // Inicia a animação do personagem
}

private void OnDisable() {
    CancelInvoke(nameof(AnimarPersonagem)); } // Cancela a animação do personagem

}

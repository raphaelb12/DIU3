using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab; // Prefab do objeto a ser spawnado
        [Range(0f, 1f)]
        public float spawnChance; // Chance de spawnar o objeto
    }

    public SpawnableObject[] objects; // Array de objetos a serem spawnados
    public float minSpawn = 1f; // Tempo mínimo para spawnar
    public float maxSpawn = 2f; // Tempo máximo para spawnar

    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawn, maxSpawn)); // Inicia o spawn
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value; // Gera um valor aleatório

        foreach (SpawnableObject obj in objects) // Percorre o array de objetos
        {
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab); // Instancia o objeto
                obstacle.transform.position += transform.position; // Atribui a posição do spawner ao objeto
                break;
            }

            spawnChance -= obj.spawnChance; // Diminui a chance de spawnar o objeto
        }

        Invoke(nameof(Spawn), Random.Range(minSpawn, maxSpawn));
    } // Método para spawnar objetos

}
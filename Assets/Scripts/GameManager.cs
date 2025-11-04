using UnityEngine;

public class GameManagerSimple : MonoBehaviour
{
    public int lives = 10; // tweak

    public void LoseLife(int amount)
    {
        lives -= amount;
        if (lives <= 0)
        {
            lives = 0;
            Debug.Log("Game Over");
            // TODO: reload scene or show UI
        }
    }
}

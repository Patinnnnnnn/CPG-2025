using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; // Arraste aqui o painel GameOverUI no Inspector

    private bool isGameOver = false;

    // Chame este método quando quiser exibir a tela de Game Over
    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverUI.SetActive(true);  // Ativa o painel com a imagem
        Time.timeScale = 0f;         // Pausa o jogo (opcional)
        Debug.Log("Game Over!");
    }

    // Opcional: reiniciar o jogo
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

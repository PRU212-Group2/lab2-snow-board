using UnityEngine;

public class UIHelpGuide : MonoBehaviour
{
    GameManager gameManager; 

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnMainMenuPressed()
    {
        gameManager.MainMenu();
    }
}

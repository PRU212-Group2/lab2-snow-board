using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    CapsuleCollider2D bodyCollider;
    BoxCollider2D topCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        bodyCollider = GetComponent<CapsuleCollider2D>();
        topCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        bool isTouchingBody = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        if (isTouchingBody)
        {
            var player = FindFirstObjectByType<PlayerController>();
            player.Crash();
            Invoke("ReloadScene", 2);
        }
    }
    
    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}

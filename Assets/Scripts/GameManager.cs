using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private InputAction resetAction;

    private void OnEnable()
    {
        resetAction = new InputAction(binding: "<Keyboard>/r");
        resetAction.performed += ctx => ResetGame();
        resetAction.Enable();
    }

    private void OnDisable()
    {
        resetAction.Disable();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
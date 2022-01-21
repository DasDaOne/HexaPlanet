using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private BottomPanelAnimation shopAnimator;
    
    private SaveManager saveManager;
    
    
    private void Start()
    {
        saveManager = GetComponent<SaveManager>();
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void ShowShop()
    {
        shopAnimator.PlayAnimation();
    }

    public void ResetProgress()
    {
        saveManager.ResetGameData();
        SceneManager.LoadScene(0);
    }
}

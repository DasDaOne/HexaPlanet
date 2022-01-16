using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    public int stone;

    [Header("Text fields")] 
    [SerializeField] private TextMeshProUGUI stoneText;

    private SaveManager saveManager;
    
    
    private void Start()
    {
        saveManager = GetComponent<SaveManager>();
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void ResetProgress()
    {
        saveManager.ResetGameData();
        SceneManager.LoadScene(0);
    }

    private void UpdateUI()
    {
        stoneText.text = "Stone: " + stone;
    }

    public void AddMaterial(int amount)
    {
        stone += amount;
    }
}

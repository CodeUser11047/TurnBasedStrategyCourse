using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TurnNumberText;
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private GameObject enemyTurnVisual;
    private void Start()
    {
        SetNextTurnButton();
        UpdateTurnNumberText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();

        TurnSystem.Instance.OnTurnChanged += TuranSystem_OnTurnChanged;

    }
    private void SetNextTurnButton()
    {
        nextTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
    }
    private void TuranSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnNumberText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }
    public void UpdateTurnNumberText()
    {
        TurnNumberText.text = "当前回合数 ： " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEndTurnButtonVisibility()
    {
        nextTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}

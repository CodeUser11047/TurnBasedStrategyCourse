using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedImage;

    private BaseAction baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName();

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);

            // Debug.Log(baseAction + "tip");此处测试没问题
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedImage.SetActive(selectedBaseAction == baseAction);
    }

}

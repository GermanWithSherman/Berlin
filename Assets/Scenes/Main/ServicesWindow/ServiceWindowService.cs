using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceWindowService : MonoBehaviour
{
    public UnityEngine.UI.Button Button;
    public TMPro.TextMeshProUGUI textMeshProUGUI;

    private Service _service;

    public void setService(Service service)
    {
        _service = service;

        textMeshProUGUI.text = service.label;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(service.buy);

        update();
    }

    public void update()
    {
        if (_service == null)
            return;

        GameManager gameManager = GameManager.Instance;

        if (gameManager.PC.moneyCash < _service.price)
        {
            Button.interactable = false;
            textMeshProUGUI.text = $"{_service.label}\nInsufficient funds";
            textMeshProUGUI.color = Color.red;
        }
        else
        {
            Button.interactable = true;
            textMeshProUGUI.text = $"{_service.label}";
            textMeshProUGUI.color = Color.black;
        }

            
    }
}

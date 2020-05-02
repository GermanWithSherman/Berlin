using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsContainer : UIUpdateListener
{
    public UIOption optionPrefab;
    //public int optionCount = 4;



    private void Awake()
    {
        /*for (int i = 0; i < optionCount; i++)
        {
            Instantiate(optionPrefab, transform);
        }*/
    }

    private void optionsClear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void optionsSet(IEnumerable<Option> options)
    {
        optionsClear();

        foreach (Option option in options)
        {
            UIOption uiOption = Instantiate<UIOption>(optionPrefab, transform);
            uiOption.optionSet(Option.Inherited(option));
        }
    }

    public override void uiUpdate(GameManager gameManager)
    {
        optionsSet(gameManager.CurrentOptions);
    }
}

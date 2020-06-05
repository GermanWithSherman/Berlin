using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModsMenu : MonoBehaviour
{

    private ModsServer _modsServer;
    public ModsMenuMod ModPrefab;
    public Transform ModsContainer;


    public void show()
    {
        if (_modsServer == null)
            _modsServer = GameManager.Instance.ModsServer;

        ModsContainer.childrenDestroyAll();

        gameObject.SetActive(true);

        foreach(Mod mod in _modsServer.ActivatedMods)
        {
            ModsMenuMod uiMod = Instantiate(ModPrefab, ModsContainer);
            uiMod.SetMod(mod);
        }
    }
}

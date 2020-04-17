using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRLocations : MonoBehaviour
{
    public UIRLocation RLPrefab;

    public void setRLs(IEnumerable<LocationConnection> locationConnections)
    {
        if(locationConnections == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach(LocationConnection locationConnection in locationConnections)
        {
            UIRLocation uIRLocation = Instantiate(RLPrefab, transform);
            uIRLocation.setRLocation(locationConnection);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceWindow : UIPanelViewWindow
{
    public ServiceWindowService ServicePrefab;

    public void clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public override void setContent(IPanelData panelData)
    {
        clear();

        if(!(panelData is ServiceCategory))
        {
            Debug.LogError("Type mismatch");
            return;
        }

        ServiceCategory serviceCategory = (ServiceCategory)panelData;

        foreach(Service service in serviceCategory.Services)
        {
            ServiceWindowService uiService = Instantiate(ServicePrefab,transform);
            uiService.setService(service);
        }

    }

    public override void update()
    {
        foreach(Transform child in transform)
        {
            ServiceWindowService uiService = child.GetComponent<ServiceWindowService>();
            if (uiService != null)
                uiService.update();
        }
    }
}

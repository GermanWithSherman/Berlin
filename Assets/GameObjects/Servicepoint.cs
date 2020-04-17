using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servicepoint
{
    public List<string> services;
    [JsonIgnore]
    public List<Service> Services
    {
        get
        {
            List<Service> result = new List<Service>();

            foreach(string serviceId in services)
            {
                result.Add(GameManager.Instance.ServicesCache.service(serviceId));
            }
            //TODO: Cache result
            return result;
        }
    }

    [JsonIgnore]
    public IEnumerable<ServiceCategory> ServiceCategories
    {
        get
        {
            Dictionary<string,ServiceCategory> result = new Dictionary<string, ServiceCategory>();

            foreach(Service service in Services)
            {
                string cat = service.category;
                ServiceCategory serviceCategory;
                if (!result.ContainsKey(cat))
                    result.Add(cat, new ServiceCategory(cat));
                serviceCategory = result[cat];
                serviceCategory.Services.Add(service);
            }

            return result.Values;
        }
    }

    public string title;

    
}

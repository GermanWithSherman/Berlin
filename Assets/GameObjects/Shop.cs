using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop
{
    /*[JsonProperty]
    private List<string> itemIds = new List<string>();

    private IList<Item> _Items;
    [JsonIgnore]
    public IList<Item> Items
    {
        get => getItems();
    }*/

    public ItemsCollection ItemsAll { get => Items; } //Not limited by number, TODO

    public ItemsCollection Items = new ItemsCollection();

    [JsonProperty]
    private string type;
    [JsonIgnore]
    public ShopType ShopType
    {
        get => GameManager.Instance.ShopTypeCache[type];
        set => type = value.id;
    }

    public Shop() { }

    public Shop(ShopType shopType)
    {
        ShopType = shopType;
        generate();
    }


    public void generate()
    {
        GameManager gameManager = GameManager.Instance;
        ItemsLibrary itemsLibrary = gameManager.ItemsLibrary;

        IEnumerable<Item> items = itemsLibrary.items();
        IEnumerable<Item> filteredItems = ShopType.filter.filter(items);

        Items = new ItemsCollection(filteredItems);


    }

    /*private IList<Item> getItems()
    {
        if (_Items == null || _Items.Count == 0)
            updateItem();
        return _Items;
    }

    private void updateItem()
    {
        GameManager gameManager = GameManager.Instance;
        ItemsLibrary itemsLibrary = gameManager.ItemsLibrary;

        _Items = new List<Item>();
        foreach (string itemId in itemIds)
        {
            _Items.Add(itemsLibrary[itemId]);
        }
    }*/
}

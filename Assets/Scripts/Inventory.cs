using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;

        }
        instance = this;
    }

    #endregion


    //Inventory.instance.AddItem(Item) to add an item in inventory  


    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;


    
    private byte inventorySpace = 4;

    public List<Item> items = new List<Item>();

    [Header("AllItems")]
    public List<Item> AllItems = new List<Item>();


    /// <summary>
    /// bool=wasPickedUp
    /// if(wasPickedUp) add it to inv and destroy it from scene
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
            if (items.Count >= inventorySpace)
            {
                Debug.Log("Not enough InventorySpace");
                return false;
            }
            items.Add(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
            
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    /// <summary>
    /// itemTypes - all items in inventory script on player
    /// </summary>
    /// <param name="itemType"></param>
    public void AddItemOFType(int itemType)
    {
        if (items.Count >= inventorySpace)
        {
            Debug.Log("Not enough InventorySpace");
        }
        else
        {
        items.Add(AllItems[itemType]);
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public bool CheckItem(Item _item)
    {
        foreach(Item item in items)
        {
            if (item.name == _item.name)
                Debug.Log("has item named "+_item.name);
                return true;
        }
        Debug.Log("does not have item named " + _item.name);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return false;
    }

    public void SaveInventory(List<Item> saveListItems)
    {
        Debug.Log("Saved inventory");
        saveListItems.Clear();
        saveListItems.AddRange(items);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void ResetInventory()
    {
        items.Clear();
        Debug.Log("Reseted inventory");

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void LoadInventory(List<Item> savedInventory)
    {

        Debug.Log("Loaded inventory");
        items.Clear();
        items.AddRange(savedInventory);


        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}

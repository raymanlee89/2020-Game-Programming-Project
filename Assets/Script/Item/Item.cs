using UnityEngine;

[System.Serializable]
public class ItemData{
    public string name;
    public Sprite icon;
    public bool isResource;
    public Sprite image;
    [TextArea(1, 10)]
    public string discription;
}

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public ItemData itemData;
}

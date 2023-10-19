using System.Collections.Generic;

public class ManagerHelper<T>
{
    public static List<T> items;
    public static void AddMeToItems(T item)
    {
        if (items.Contains(item))
            return;
        items.Add(item);
    }
    public static void RemoveMeFromItems(T item) => items.Remove(item);
    public static void Clear() => items.Clear();



    
    //Add To Manager
    //private void Awake()
    //{
    //    Clear();
    //}
    //Add these to item
    //private void OnEnable()
    //{
    //    ManagerHelper<T>.AddMeToItems(this);
    //}
    //private void OnDisable()
    //{
    //    ManagerHelper<T>.RemoveMeFromItems(this);
    //}
}

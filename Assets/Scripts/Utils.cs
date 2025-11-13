using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Utils 
{
    public static List<T> GetListInChild<T>(Transform parent)
    {
        List<T> result = new List<T>();

        for(int i = 0; i < parent.childCount; i++)
        {
            var component = parent.GetChild(i).GetComponent<T>();
            if (component != null)
                result.Add(component);
        }

        return result;
    }

    public static List<T> TakeAndRemoveRandom<T>(List<T> source,int n)
    {
        List<T> result = new List<T>(); // khoi tao list de tra ve
        n = Mathf.Min(n, source.Count); // check de ddam bao so luong lay ve khong vuot qua so luong list co san

        for (int i = 0; i < n; i++)
        {
            int ranIndex = Random.Range(0, source.Count);
            result.Add(source[ranIndex]);
            source.RemoveAt(ranIndex);
        }

        return result;
    }

    public static T GetRayCastUI<T> (Vector2 position) where T : MonoBehaviour
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = position;
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, list);

        if(list.Count > 0)
        {
            for(int i = 0; i < list.Count; i++)
            {
                T component = list[i].gameObject.GetComponent<T>();
                if (component != null)
                    return component;
            }
        }

        return null;
    }
}

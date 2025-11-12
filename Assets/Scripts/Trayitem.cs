using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Trayitem : MonoBehaviour
{
    private List<Image> _foodList;
    public List<Image> FoodList => _foodList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _foodList = Utils.GetListInChild<Image>(this.transform);

        for (int i = 0; i < _foodList.Count; i++)
            _foodList[i].gameObject.SetActive(false);
    }

    public void OnSetFood(List<Sprite> items)
    {
        if(items.Count <= _foodList.Count)
        {
            for(int i = 0; i < items.Count; i++)
            {
                Image slot = this.RandomSlot();
                slot.gameObject.SetActive(true);
                slot.sprite = items[i];
                slot.SetNativeSize();
            }
        }
    }

    private Image RandomSlot()
    {
    rerand: int n = Random.Range(0, _foodList.Count);
        if (_foodList[n].gameObject.activeInHierarchy) goto rerand;

        return _foodList[n];
    }
}

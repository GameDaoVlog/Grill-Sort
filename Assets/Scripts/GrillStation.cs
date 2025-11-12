using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class GrillStation : MonoBehaviour
{
    [SerializeField] private Transform _trayContainer;
    [SerializeField] private Transform _slotContainer;


    private List<Trayitem> _totalTrays;
    private List<FoodSlot> _totalSlot;

    private Stack<Trayitem> _stackTrays = new Stack<Trayitem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _totalTrays = Utils.GetListInChild<Trayitem>(_trayContainer);
        _totalSlot = Utils.GetListInChild<FoodSlot>(_slotContainer);
    }

    public void OnInitGrill(int totalTray, List<Sprite> listFood)
    {
        // xu ly set gia tri cho bep truoc
        int foodCount = Random.Range(1, _totalSlot.Count + 1);
        List<Sprite> list = listFood;
        List<Sprite> listSlot = Utils.TakeAndRemoveRandom<Sprite>(list, foodCount);

        for(int i = 0; i < listSlot.Count; i++)
        {
            FoodSlot slot = this.RandomSlot();
            slot.OnSetSlot(listSlot[i]);
        }

    
        // xu ly dia
        List<List<Sprite>> remainFood = new List<List<Sprite>>();

        for(int i = 0; i < totalTray - 1; i++)
        {
            remainFood.Add(new List<Sprite>());
            int n = Random.Range(0, listFood.Count);
            remainFood[i].Add(listFood[n]);
            listFood.RemoveAt(n);
        }

        while (listFood.Count > 0)
        {
            int rans = Random.Range(0, remainFood.Count);
            if (remainFood[rans].Count < 4)
            {
                int n = Random.Range(0, listFood.Count);
                remainFood[rans].Add(listFood[n]);
                listFood.RemoveAt(n);
            }
        }

        for (int i = 0; i < _totalTrays.Count; i++)
        {
            bool active = i < remainFood.Count;
            _totalTrays[i].gameObject.SetActive(active);

            if (active)
            {
                _totalTrays[i].OnSetFood(remainFood[i]);
                Trayitem item = _totalTrays[i];
                _stackTrays.Push(item);
            }
        }
    }

    private FoodSlot RandomSlot()
    {
    reRand: int n = Random.Range(0, _totalSlot.Count);
        if (_totalSlot[n].HasFood) goto reRand;

        return _totalSlot[n];
    }

    public FoodSlot GetSlotNull()
    {
        for(int i = 0; i < _totalSlot.Count; i++)
        {
            if (!_totalSlot[i].HasFood)
                return _totalSlot[i];
        }

        return null;
    }

    private bool HasGrillEmpty()
    {
        for (int i = 0; i < _totalSlot.Count; i++)
        {
            if (_totalSlot[i].HasFood)
                return false;
        }

        return true;
    }

    public void OnCheckMerge()
    {
        if(this.GetSlotNull() == null) // kiem tra xem so luong slot du 3 item chua, neu chua du thi no == null
        {
            if (this.CanMerge())
            {
                Debug.Log("Complete Grill");

                for (int i = 0; i < _totalSlot.Count; i++)
                {
                    _totalSlot[i].OnActiveFood(false);
                }

                this.OnPrepareTray();
                GameManagers.Instance?.OnMinusFood();
            }
        }
    }

    public void OnCheckPrepareTray()
    {
        if(this.HasGrillEmpty())
        {
            this.OnPrepareTray();
        }
    }

    private void OnPrepareTray()
    {
        if (_stackTrays.Count > 0)
        {
            Trayitem item = _stackTrays.Pop();

            for (int i = 0; i < item.FoodList.Count; i++)
            {
                Image img = item.FoodList[i];
                if (img.gameObject.activeInHierarchy)
                {
                    _totalSlot[i].OnPrepareItem(img);
                    img.gameObject.SetActive(false);
                }
            }

            item.gameObject.SetActive(false);
        }
    }

    private bool CanMerge()
    {
        string name = _totalSlot[0].GetSpriteFood.name;

        for (int i = 1; i < _totalSlot.Count; i++)
        {
            if ( _totalSlot[i].GetSpriteFood.name != name)
                return false;
        }

        return true;
    }
}

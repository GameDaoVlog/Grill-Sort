using UnityEngine;
using UnityEngine.UI;

public class DropDragCtrl : MonoBehaviour
{
    [SerializeField] private Image _imgFoodDrag;

    private FoodSlot _currentFood, _cacheFood;
    private bool _hasDrag;
    private Vector3 _offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // check khi click chuot
        {
            _currentFood = Utils.GetRayCastUI<FoodSlot>(Input.mousePosition); // check o vi tri click chuot xem co Ui gan class FoodSlot
            if(_currentFood != null && _currentFood.HasFood)
            {
                _hasDrag = true;
                _cacheFood = _currentFood;
                // Gan sprite food cho dummy image
                _imgFoodDrag.gameObject.SetActive(true);
                _imgFoodDrag.sprite = _currentFood.GetSpriteFood;
                _imgFoodDrag.SetNativeSize();
                _imgFoodDrag.transform.position = _currentFood.transform.position; // gan vi tri


                //tinh offset
                Vector3 mouseWordPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _offset = mouseWordPos - _currentFood.transform.position;

                _currentFood.OnActiveFood(false);
            }
        }

        if (_hasDrag)
        {
            Vector3 mouseWordPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 foodPos = mouseWordPos + _offset;
            foodPos.z = 0f;
            _imgFoodDrag.transform.position = foodPos;

            FoodSlot slot = Utils.GetRayCastUI<FoodSlot>(Input.mousePosition);
            if(slot != null  )
            {
                if (!slot.HasFood) // vi tri item chua co food
                {
                    if (_cacheFood == null || _cacheFood.GetInstanceID() != slot.GetInstanceID())
                    {
                        _cacheFood?.OnHideFood();
                        _cacheFood = slot;
                        _cacheFood.OnFadeFood();
                        _cacheFood.OnSetSlot(_currentFood.GetSpriteFood);
                    }
                }
                else // vi tri tro chuot da co item
                {                    
                    FoodSlot slotAvalable = slot.GetSlotNull;
                    if(slotAvalable != null)
                    {
                        _cacheFood?.OnHideFood();
                        _cacheFood = slotAvalable;
                        _cacheFood.OnFadeFood();
                        _cacheFood.OnSetSlot(_currentFood.GetSpriteFood);
                    }
                    else
                    {
                        this.OnClearCacheSlot();
                    }
                }
            }
            //else
            //{
            //    if(_cacheFood != null)
            //    {
            //        _cacheFood.OnHideFood();
            //        _cacheFood = null;
            //    }
            //}
        }

        if(Input.GetMouseButtonUp(0) && _hasDrag)
        {
            _imgFoodDrag.gameObject.SetActive(false);
            _currentFood.OnActiveFood(true);
            _currentFood = null;
            _hasDrag = false;

            this.OnClearCacheSlot();
        }
    }

    private void OnClearCacheSlot()
    {
        if (_cacheFood != null && _cacheFood.GetInstanceID() != _currentFood.GetInstanceID())
        {
            _cacheFood.OnHideFood();
            _cacheFood = null;
        }
    }
}

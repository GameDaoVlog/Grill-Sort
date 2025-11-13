using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DropDragCtrl : MonoBehaviour
{
    [SerializeField] private Image _imgFoodDrag;
    [SerializeField] private float _timeCheckSuggest;

    private FoodSlot _currentFood, _cacheFood;
    private bool _hasDrag;
    private Vector3 _offset;
    private float _countTime;
    private float _timeAtClick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _countTime += Time.deltaTime;

        if(_countTime >= _timeCheckSuggest)
        {
            _countTime = 0f;
            GameManagers.Instance?.OnCheckAndShake();
        }

        if (Input.GetMouseButtonDown(0)) // check khi click chuot
        {
            FoodSlot tapSlot = Utils.GetRayCastUI<FoodSlot>(Input.mousePosition); // check o vi tri click chuot xem co Ui gan class FoodSlot
            if(tapSlot != null )
            {              
                if (tapSlot.HasFood)
                {
                    _hasDrag = true;
                    _currentFood?.OnActiveFood(true);
                    _cacheFood = _currentFood = tapSlot;
                    // Gan sprite food cho dummy image
                    _imgFoodDrag.gameObject.SetActive(true);
                    _imgFoodDrag.sprite = _currentFood.GetSpriteFood;
                    _imgFoodDrag.SetNativeSize();
                    _imgFoodDrag.transform.position = _currentFood.transform.position; // gan vi tri               

                    //tinh offset
                    Vector3 mouseWordPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _offset = _currentFood.transform.position - mouseWordPos;
                    _offset.z = 0f;

                    _currentFood.OnActiveFood(false);
                    _imgFoodDrag.transform.DOScale(Vector3.one * 1.2f, 0.2f);
                }
                else
                {
                    if(_currentFood != null) // di chuyen item vua slect toi vi tri nay
                    {
                        _imgFoodDrag.transform.DOMove(tapSlot.transform.position, 0.4f).OnComplete(()=> {
                            tapSlot.OnSetSlot(_currentFood.GetSpriteFood);
                            tapSlot.OnActiveFood(true);
                            tapSlot.OnCheckMerge();
                            _currentFood?.OnCheckPrepareTray();
                            _currentFood = null;
                            _imgFoodDrag.gameObject.SetActive(false);
                        });
                        _imgFoodDrag.transform.DOScale(Vector3.one, 0.4f);
                    }
                }
            }
            else
            {
                _currentFood?.OnActiveFood(true);
                _currentFood = null;
                _imgFoodDrag.gameObject.SetActive(false);
            }

            _timeAtClick = Time.time;
        }

        if (_hasDrag)
        {
            Vector3 mouseWordPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 foodPos = mouseWordPos + _offset;
            foodPos.z = 0f;
            _imgFoodDrag.transform.position = foodPos;
            _countTime = 0f;

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
            else
            {
                if (_cacheFood != null)
                {
                    _cacheFood.OnHideFood();
                    _cacheFood = null;
                }
            }
        }

        if(Input.GetMouseButtonUp(0) && _hasDrag)
        {
            if (Time.time - _timeAtClick < 0.15f) // Controll by click
            {
                
            }
            else
            {
                if (_cacheFood != null) // xu ly fill item
                {
                    _imgFoodDrag.transform.DOMove(_cacheFood.transform.position, 0.2f).OnComplete(() =>
                    {
                        _imgFoodDrag.gameObject.SetActive(false);
                        _cacheFood.OnSetSlot(_currentFood.GetSpriteFood);
                        _cacheFood.OnActiveFood(true);
                        _cacheFood.OnCheckMerge();
                        _currentFood?.OnCheckPrepareTray();
                        _cacheFood = null;
                        _currentFood = null;
                    });

                    _imgFoodDrag.transform.DOScale(Vector3.one, 0.22f);
                }
                else // xu ly tro ve vi tri ban dau
                {
                    _imgFoodDrag.transform.DOMove(_currentFood.transform.position, 0.3f).OnComplete(() =>
                    {
                        _imgFoodDrag.gameObject.SetActive(false);
                        _currentFood.OnActiveFood(true);
                    });

                    _imgFoodDrag.transform.DOScale(Vector3.one, 0.3f);
                }
            }
            _hasDrag = false;

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

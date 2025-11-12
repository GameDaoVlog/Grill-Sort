using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FoodSlot : MonoBehaviour
{
    private Image _imgFood;

    private Color _normalColor = new Color(1f, 1f, 1f, 1f);
    private Color _fadeColor = new Color(1f, 1f, 1f, 0.7f);

    private GrillStation _grillCtrl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _imgFood = this.transform.GetChild(0).GetComponent<Image>();
        _imgFood.gameObject.SetActive(false);
        _grillCtrl = this.transform.parent.parent.GetComponent<GrillStation>();
    }

    public void OnSetSlot(Sprite spr)
    {
        _imgFood.gameObject.SetActive(true);
        _imgFood.sprite = spr;
        _imgFood.SetNativeSize();
    }   

    public void OnActiveFood(bool active)
    {
        _imgFood.gameObject.SetActive(active);
        _imgFood.color = _normalColor;
    }

    public void OnFadeFood()
    {
        this.OnActiveFood(true);
        _imgFood.color = _fadeColor;
    }

    public void OnHideFood()
    {
        this.OnActiveFood(false);
        _imgFood.color = _normalColor;

    }

    public void OnCheckMerge()
    {
        _grillCtrl?.OnCheckMerge();
    }

    public void OnPrepareItem(Image img)
    {
        this.OnSetSlot(img.sprite);
        _imgFood.color = _normalColor;
        _imgFood.transform.position = img.transform.position;
        _imgFood.transform.localScale = img.transform.localScale;
        _imgFood.transform.localEulerAngles = img.transform.localEulerAngles;

        _imgFood.transform.DOLocalMove(Vector3.zero, 0.2f);
        _imgFood.transform.DOScale(Vector3.one, 0.2f);
        _imgFood.transform.DORotate(Vector3.zero, 0.2f);
    }

    public void OnCheckPrepareTray()
    {
        _grillCtrl?.OnCheckPrepareTray();
    }

    public FoodSlot GetSlotNull => _grillCtrl.GetSlotNull();

    public bool HasFood => _imgFood.gameObject.activeInHierarchy && _imgFood.color == _normalColor;
    public Sprite GetSpriteFood  => _imgFood.sprite;
}

using UnityEngine;

public class BugView : MonoBehaviour
{
    [SerializeField] private Sprite _mainSprite;

    private SpriteRenderer _spriteRenderer;
    private BugBase _model;

    private void Awake()
    {
        _model = this.GetComponent<BugBase>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (_model == null)
        {
            Debug.LogError($"{this.name} is not settuped! No BugBase attached!");
            return; // bug cant run without BugBase but can without sprite renderer
        }
        if (_spriteRenderer == null)
        {
            Debug.LogError($"{this.name} is not settuped! No sprite renderer attached!");
        }
        _model.OnDamageTaken += OnDamage;
        _model.OnHealGained += OnHeal;
        //_spriteRenderer.sprite = _mainSprite;
    }
    private void OnDestroy()
    {
        _model.OnDamageTaken -= OnDamage;
        _model.OnHealGained -= OnHeal;
    }
    private void OnDamage() { }
    private void OnHeal() { }
}

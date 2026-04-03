using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private Sprite _mainSprite;

    private SpriteRenderer _spriteRenderer;
    private Resource _model;

    private void Awake()
    {
        _model = this.GetComponent<Resource>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        if ( _model == null)
        {
            Debug.LogError($"{this.name} is not settuped! No IResource attached!");
            return; // resource cant run without IResource but can without sprite renderer
        }
        if (_spriteRenderer == null)
        {
            Debug.LogError($"{this.name} is not settuped! No sprite renderer attached!");
        }
        _model.OnPickingUp += OnPickingUp;
        //_spriteRenderer.sprite = _mainSprite;
    }
    private void OnDestroy()
    {
        _model.OnPickingUp -= OnPickingUp;
    }
    private void OnPickingUp()
    {

    }
}

using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private Sprite _mainSprite;

    private SpriteRenderer _spriteRenderer;
    private IResource _model;

    private void Awake()
    {
        _model = this.GetComponent<IResource>();
        if ( _model == null)
        {
            Debug.LogError($"{this.name} is not settuped! No IResource attached!");
            return; // resource cant run without IResource but can without sprite renderer
        }
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"{this.name} is not settuped! No sprite renderer attached!");
        }

    }
}

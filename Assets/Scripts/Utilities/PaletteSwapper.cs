using UnityEngine;

public class PaletteSwapper : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float lerpDuration = 2.0f;
    
    private float _lerpTime;
    private MaterialPropertyBlock _propBlock;
    private SpriteRenderer _spriteRenderer;
    private bool _freeze;
    private bool _isFrozen;
    
    private static readonly int Blend = Shader.PropertyToID("_Blend");
    
    private void Start()
    {
        _propBlock = new MaterialPropertyBlock();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (material != null)
        {
            material.SetFloat(Blend, 0.0f);
        }

        _lerpTime = 0.0f;
    }

    private void Update()
    {
        if (material == null)
            return;


        if (_freeze && !_isFrozen)
        {
            if (_lerpTime < lerpDuration)
            {
                _lerpTime += Time.deltaTime;
                float blendFactor = _lerpTime / lerpDuration;
                _spriteRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat(Blend, blendFactor);
                _spriteRenderer.SetPropertyBlock(_propBlock);
            }
            else
            {
                _isFrozen = true;
            }
        }
        else if (!_freeze && _isFrozen)
        {
            if (_lerpTime > 0)
            {
                _lerpTime -= Time.deltaTime;
                float blendFactor = _lerpTime / lerpDuration;
                _spriteRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat(Blend, blendFactor);
                _spriteRenderer.SetPropertyBlock(_propBlock);
            }
            else
            {
                _isFrozen = false;
                _lerpTime = 0.0f;
            }
        }
    }

    public void Freeze() => _freeze = true;
}
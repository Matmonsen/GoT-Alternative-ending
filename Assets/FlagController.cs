using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoBehaviour
{
    [SerializeField] private Sprite _flag1;

    [SerializeField] private Sprite _flag2;

    private float _changeRate = .5f;

    private float _timeSinceLastChange;

    void Awake()
    {
        _timeSinceLastChange = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastChange += Time.deltaTime;

        if (_timeSinceLastChange > _changeRate)
            FlipFlag();
    }

    void FlipFlag()
    {
        _timeSinceLastChange = 0;
        var renderer = transform.GetComponent<Image>();
        if (renderer.sprite.name.Equals(_flag1.name))
            renderer.sprite = _flag2;
        else
            renderer.sprite = _flag1;
    }
}

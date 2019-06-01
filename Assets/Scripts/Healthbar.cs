using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private Image _redImage;
    private float _originalLength;

    void Awake()
    {
        Debug.Log(transform.childCount);
        _currentHealth = _maxHealth;
        _redImage = transform.Find("Canvas").Find("ImageHolder").Find("Red").gameObject.GetComponent<Image>();
        _originalLength = _redImage.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        _redImage.rectTransform.sizeDelta = new Vector2(_originalLength / 100 * (_maxHealth / 100 * _currentHealth),_redImage.rectTransform.sizeDelta.y);
    }

    public void UpdateDamage(int damage = 10)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
    }
}

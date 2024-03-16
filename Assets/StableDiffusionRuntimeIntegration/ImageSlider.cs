using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ImageSlider : MonoBehaviour
    {
        public enum SliderOrientation { Vertical, Horizontal }
        
        [SerializeField] private Image _sliderImage;
        [SerializeField] private RectTransform _sliderSizeReference;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private SliderOrientation _sliderOrientation;

        private RectTransform _sliderImageRect;

        private void Awake()
        {
            _sliderImageRect = _sliderImage.GetComponent<RectTransform>();
        }

        public void UpdateSlider(float percent, string message)
        {
            _text.text = message;
            
            var sizeDelta = _sliderSizeReference.sizeDelta;
            switch (_sliderOrientation)
            {
                case SliderOrientation.Vertical:
                    _sliderImageRect.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y * percent);
                    break;
                case SliderOrientation.Horizontal:
                    _sliderImageRect.sizeDelta = new Vector2(sizeDelta.x * percent, sizeDelta.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

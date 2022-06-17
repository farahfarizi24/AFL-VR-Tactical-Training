using UnityEngine;
using TMPro;

namespace com.DU.CE.USER
{
    public class USER_FieldScaler : MonoBehaviour
    {
        [SerializeField] private SOC_StadiumProperties m_StadiumPropertiesSock = null;

        [SerializeField] private TextMeshProUGUI m_CurrentLength = null;
        [SerializeField] private TextMeshProUGUI m_CurrentWidth = null;

        public void SliderUpdateFieldLength(float length)
        {
            m_CurrentLength.text = length.ToString();
            ChangeNetworkFieldLength((int) length);
        }
        public void SliderUpdateFieldWidth(float width)
        {
            m_CurrentWidth.text = width.ToString();
            ChangeNetworkFieldWidth((int) width);
        }


        public void ButtonUpdateFieldLength(int _length)
        {
            int length = int.Parse(m_CurrentLength.text) + _length;
            m_CurrentLength.text = length.ToString();
            ChangeNetworkFieldLength(length);
        }
        public void ButtonUpdateFieldWidth(int _width)
        {
            int width = int.Parse(m_CurrentLength.text) + _width;
            m_CurrentWidth.text = width.ToString();
            ChangeNetworkFieldWidth(width);
        }


        private void ChangeNetworkFieldLength(int _length)
        {
            m_StadiumPropertiesSock.ChangeNetworkFieldLength(_length);
        }
        private void ChangeNetworkFieldWidth(int _width)
        {
            m_StadiumPropertiesSock.ChangeNetworkFieldWidth(_width);
        }
    }
}
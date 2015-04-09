using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Manager
{
    [UsedImplicitly]
    public class InputNavigator : MonoBehaviour
    {
        private EventSystem _system;

        [SerializeField, UsedImplicitly]
        private GameObject _loginPanel;

        [SerializeField, UsedImplicitly]
        private GameObject _registerPanel;

        [UsedImplicitly]
        void Start()
        {
            _system = EventSystem.current;

        }
        
        [UsedImplicitly]
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                var next = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

                if (next != null)
                {

                    var inputfield = next.GetComponent<InputField>();
                    if (inputfield != null)
                        inputfield.OnPointerClick(new PointerEventData(_system));

                    _system.SetSelectedGameObject(next.gameObject, new BaseEventData(_system));
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_loginPanel.activeSelf)
                {
                    _loginPanel.GetComponent<LoginManager>().LoginAction();
                }
                else if (_registerPanel.activeSelf)
                {
                    _registerPanel.GetComponent<RegisterManager>().RegisterAction();
                }
            }
        }
    }
}
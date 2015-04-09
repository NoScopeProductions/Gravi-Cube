using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Parse;

namespace UI.Manager
{
    [UsedImplicitly]
    public class LoginManager : MonoBehaviour
    {

        [SerializeField, UsedImplicitly]
        private InputField _email;

        [SerializeField, UsedImplicitly]
        private InputField _password;

        [SerializeField, UsedImplicitly]
        private GameObject _mainMenuPanel;

        [SerializeField, UsedImplicitly]
        private Text _errorText;


        [UsedImplicitly]
        public void Start()
        {
            if (ParseUser.CurrentUser != null)
            {
                ShowMainMenu();
            }
        }


        [UsedImplicitly]
        public void LoginAction()
        {
            StartCoroutine(LoginCoroutine());
        }

        
        private IEnumerator LoginCoroutine()
        {
            _errorText.text = "";
            var loginTask = ParseUser.LogInAsync(_email.text, _password.text);

            while (!loginTask.IsCompleted) yield return null;
            if (loginTask.IsFaulted || loginTask.IsCanceled)
            {
                _errorText.text = "Invalid Login Attempt!";
                yield break;
            }
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            Debug.Log("Login Successful");
            gameObject.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _errorText.text = "";
            _password.text = "";
        }
    }
}

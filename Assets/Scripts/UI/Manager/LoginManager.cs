using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Parse;
using System.Threading.Tasks;

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
            var loginTask = ParseUser.LogInAsync(_email.text, _password.text).ContinueWith(task =>
            {
                if (!task.IsFaulted && !task.IsCanceled) return;

                foreach (var exception in task.Exception.InnerExceptions)
                {
                    Debug.LogError(exception.Message);
                }
            });

            while (!loginTask.IsCompleted) yield return null;

            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            Debug.Log("Login Successful");
            gameObject.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }
    }
}

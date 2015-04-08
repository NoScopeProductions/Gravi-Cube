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

        [UsedImplicitly]
        public void LoginAction()
        {
            StartCoroutine(LoginCoroutine());
        }

        private IEnumerator LoginCoroutine()
        {
            ParseUser.LogOut();

            var loginTask = ParseUser.LogInAsync(_email.text, _password.text).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    // The login failed. Check the error to see why.
                    foreach (var exception in task.Exception.InnerExceptions)
                    {
                        Debug.LogError(exception.Message);
                    }
                }
            });

            while (!loginTask.IsCompleted) yield return null;
            Debug.Log("Login Successful");
            gameObject.SetActive(false);

        }
    }
}

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
            ParseUser.LogOut();

            ParseUser.LogInAsync(_email.text, _password.text).ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    // The login failed. Check the error to see why.

                    Debug.LogError(t.Exception.Message);
                }
                else
                {
                    Debug.Log("Login Success");
                }
            });
        }
    }
}

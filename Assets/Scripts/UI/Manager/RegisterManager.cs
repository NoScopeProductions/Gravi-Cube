using System;
using System.Collections;
using JetBrains.Annotations;
using Parse;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Manager
{
    [UsedImplicitly]
    public class RegisterManager : MonoBehaviour
    {
        [SerializeField, UsedImplicitly]
        private InputField _username;

        [SerializeField, UsedImplicitly]
        private InputField _email;

        [SerializeField, UsedImplicitly]
        private InputField _password;

        [SerializeField, UsedImplicitly]
        private InputField _confirmPassword;


        [UsedImplicitly]
        public void RegisterAction()
        {
            StartCoroutine(RegisterCoroutine());
        }

        private IEnumerator RegisterCoroutine()
        {
            if (!_password.text.Equals(_confirmPassword.text))
            {
                Debug.LogError("Passwords do not match!");
                yield return null;
            }

            var user = new ParseUser
            {
                Username = _username.text,
                Password = _password.text,
                Email = _email.text
            };

            var signUpTask = user.SignUpAsync().ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    throw task.Exception.Flatten();
                }
            });

            while (!signUpTask.IsCompleted) yield return null;

            Debug.Log("Sign Up Successful");
        }
    }
}


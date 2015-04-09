using JetBrains.Annotations;
using Parse;
using UnityEngine;

namespace UI.Manager
{
    [UsedImplicitly]
    public class MainMenuManager : MonoBehaviour
    {

        [SerializeField, UsedImplicitly]
        private GameObject _loginPanel;

        [UsedImplicitly]
        public void LogOutAction()
        {
            ParseUser.LogOutAsync();

            _loginPanel.GetComponent<LoginManager>().Show();
            gameObject.SetActive(false);
        }
    }
}

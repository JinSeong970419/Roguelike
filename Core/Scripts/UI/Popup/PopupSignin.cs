using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class PopupSignin : Popup
    {


        public void OnSignInAsGuestButtonClick()
        {
            Guest.Instance.SignIn(OnSignInSucceed);
        }

        public void OnGoogleSigninButtonClick()
        {
            Google.Instance.SignIn(OnSignInSucceed);
        }

        public void OnSignInSucceed()
        {
            SceneController.LoadScene("Lobby"); 
        }



    }
}

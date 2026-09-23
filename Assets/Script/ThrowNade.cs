using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Aegis.GrenadeSystem.HiEx
{
    public class ThrowNade : MonoBehaviour
    {

        //This system handles the grenade inventory (count) and throwing mechanic
        [Header("Grenade throwing system settings")]
        [SerializeField] InputActionReference _throwInput;
        [SerializeField] GameObject player;
        [SerializeField] Transform camera;
        [SerializeField] Transform throwPoint;
        [SerializeField] GameObject hiexgrenade;

        //throwing settings
        [SerializeField] float throwDelay = 0.3f; // delay after pressing throw before grenade is thrown to account for animations/audio
        [SerializeField] float throwForce = 4f; // tweak this to adjust how far the grenade is thrown


        Coroutine throwGrenade = null;


        //When the 'G' key is pressed, a grenade is thrown if grenadeCount is more than 0
        // You can update this to use the input system or key that you'd prefer


        private void Start()
        {
            // set grenade count in UI
            _throwInput.action.Enable();
        }


        private void Update()
        {

            //When G is pressed
            if (_throwInput.action.WasPressedThisFrame())
            {

                //Check if a grenade is already being thrown - if not, and there are grenades in the inventory, throw a grenade
                // This is so grenades can't be spammed

                if (throwGrenade == null)
                {
                    throwGrenade = StartCoroutine(ThrowGrenade());
                }
            }
        }


        //This is the grenade throwing Co-routine which handles the actions of throwing a grenade
        IEnumerator ThrowGrenade()
        {

            //play audio of throwing a grenade

            //wait for audio to begin playing - this delay is to account for the pin being pulled, but you can edit it above
            yield return new WaitForSeconds(throwDelay);

            //throw the grenade
            GameObject grenadeInstance = Instantiate(hiexgrenade, throwPoint.position, throwPoint.rotation);

            Rigidbody rb = grenadeInstance.GetComponent<Rigidbody>();

            rb.AddForce(camera.forward * throwForce, ForceMode.Impulse);

            //set throwGrenade verable to null, so another grenade can be thrown

            throwGrenade = null;
        }

    }
}
using UnityEngine;
using CoreBreach.Interfaces; //right now not using interfaces but in later we will entegrate systmens like IDamageable

namespace CoreBreach.Player
{
    [RequireComponent(typeof(CharacterController))] //add automatically CharacterController, it will add dependency to this script
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;

        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>(); //üzerinde bulunduüum objenin içine bak ve bana bir CharacterController bileşeni bul, sonra da onu kullanmam için kaydet 
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical   = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            if (direction.magnitude >= 0.1f)
            {
                direction.Normalize();//it will ensure that when we press w and d  at the same time it still will be the same speed
                Vector3 moveVector = direction * moveSpeed * Time.deltaTime;
                _characterController.Move(moveVector);
            }

            ApplyGravity();
        }

        private void ApplyGravity()
        {
            if (!_characterController.isGrounded) //if player is on the air use gravity
            {
                _characterController.Move(Vector3.down * 9.81f * Time.deltaTime);
            }
        }
    }
}

using UnityEngine;
using CoreBreach.Interfaces;

namespace CoreBreach.Player
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private BasicWeapon basicWeapon;

        private IWeaponBehavior _weapon;//eğer buraya BasicWeapon koysaydım oyuncunun elini BasicWeapon'a tightly coupled yapmış olurdum
        //yani neyi ateşlediğini bilmeyecek sadece ateş etme yeteneği olacak
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _weapon = basicWeapon;

            if (_weapon == null)
            {
                Debug.LogError("[PlayerShooter] dont have BasicWeapon! Gave this script a weapon on Inspector.");
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0)) //fare sol tuş basılı olduğu sürece true dönüyor
            {
                Vector3 mouseWorldPosition = GetMouseWorldPosition();
                if (mouseWorldPosition == Vector3.zero) return;

                Vector3 origin = transform.position;
                Vector3 direction = mouseWorldPosition - origin;
                direction.y = 0f;
                direction.Normalize(); //normalizing by making 1 unit long

                _weapon.Fire(origin, direction);
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); //kamera merceğinden farenin konumuna doğru 3 boyutlu düntanın içine uzanan sanal lazer ışını yani Ray fırlatıyor
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return Vector3.zero;
        }

        public void SetWeapon(IWeaponBehavior newWeapon) //dynamic weapon (Polymorphism)
        {
            _weapon = newWeapon;
            Debug.Log($"[PlayerShooter] Silah değiştirildi: {newWeapon.GetType().Name}");
            // TODO: Decorator gelince sadece SetWeapon(new DoubleDamageDecorator(weapon)) yapılacak, başka hiçbir şey değişmeyecek

        }
    }
}

using UnityEngine;

namespace Scratch
{
    public class MouseCollider : MonoBehaviour
    {
        private void Update()
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            transform.position += new Vector3(0, 0, 10);
        }
    }
}

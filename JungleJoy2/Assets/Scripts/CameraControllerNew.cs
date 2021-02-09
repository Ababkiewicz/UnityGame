using UnityEngine;

public class CameraControllerNew : MonoBehaviour
{
 //VARIABLES
 [SerializeField] private float camSensitivity;

 //REFERENCES
private Transform parent;

private void Start()
{
    parent = transform.parent;
}

private void Update()
{
    Rotate();
}

private void Rotate()
{
    float camX = Input.GetAxis("Horizontal") * camSensitivity * Time.deltaTime;

    parent.Rotate(Vector3.up, camX);


}

}

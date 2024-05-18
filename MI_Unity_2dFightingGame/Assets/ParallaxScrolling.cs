
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public float prlxFactorX, prlxFactorY; // Parallax Factor
    public float orix, oriy;
    private GameObject mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(orix + mainCamera.transform.position.x * prlxFactorX, oriy + mainCamera.transform.position.y * prlxFactorY);
    }
}

using UnityEngine;

public class CopyBackGround : MonoBehaviour
{
    public GameObject BackGroundObj;
    public int numberOfCopies = 5;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfCopies; i++)
        {
            Vector3 newPosition = new Vector3(17.6f * i, 0f, 0f);
            GameObject obj = Instantiate(BackGroundObj, newPosition, Quaternion.identity, gameObject.transform);
            obj.name = "BackGround_" + i;
            obj.GetComponent<ParallaxScrolling>().orix = 17.6f * i;
            obj.GetComponent<ParallaxScrolling>().oriy = 0f;
            obj.GetComponent<ParallaxScrolling>().prlxFactorX = 0.9f;
            obj.GetComponent<ParallaxScrolling>().prlxFactorY = 0.9f;
        }
        Destroy(BackGroundObj);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

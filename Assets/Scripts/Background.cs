using UnityEngine;

public class Background : MonoBehaviour {
    
    /* -------------------------------- Variables ------------------------------- */

    public Camera mainCamera;

    [Header("Background")]
    public GameObject starObject;
    public int starMinOpacity = 1;
    public int starMaxOpacity = 4;
    public float starMinScale = 0.8f;
    public float starMaxScale = 1.4f;
    public float starDieTime = 20f;


    /* ------------------------------ Main Classes ------------------------------ */

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}

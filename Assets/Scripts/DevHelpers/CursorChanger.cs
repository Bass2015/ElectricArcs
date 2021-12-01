
using UnityEngine;

public class CursorChanger : MonoBehaviour
{

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public float scaleMultiplier;

    const float ScaleMultiplier = 0.8f;

    Vector3 initFingerScale;
    Transform tapping;
    Transform finger;
    
    void Awake()
    {
        Cursor.visible = false;

    }

    private void Start()
    {
        
        tapping = transform.GetChild(1);
      //  tapping.gameObject.SetActive(false);
        finger = transform.GetChild(0);
        initFingerScale = finger.localScale;
        finger.localScale = initFingerScale * ScaleMultiplier;

    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 actualPos = transform.position;



        

        transform.position = Vector3.Lerp(mousePos, actualPos, scaleMultiplier); ;

        if (Input.GetMouseButton(0))
        {
            finger.localScale = initFingerScale * ScaleMultiplier;
            tapping.gameObject.SetActive(true);
        }
        else
        {
            finger.localScale = initFingerScale;
            tapping.gameObject.SetActive(false);
        }
    }

    

}

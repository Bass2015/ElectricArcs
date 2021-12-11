
using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour
{
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    const float ScaleMultiplier = 0.8f;

    Vector3 initFingerScale;
    Transform tapping;
    Transform finger;

    private void Start()
    {
        
        tapping = transform.GetChild(1);
        finger = transform.GetChild(0);
        initFingerScale = finger.localScale;
        finger.localScale = initFingerScale * ScaleMultiplier;

    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 actualPos = transform.position;

        transform.position = Vector3.Lerp(mousePos, actualPos, ScaleMultiplier); ;

        if (Input.GetMouseButton(0))
        {
            TappingAction();

        }
        else
        {
            UntappingAction();
        }
    }

    private void UntappingAction()
    {
        finger.localScale = initFingerScale;
        tapping.gameObject.SetActive(false);
    }

    private void TappingAction()
    {
        finger.localScale = initFingerScale * ScaleMultiplier;
        tapping.gameObject.SetActive(true);
    }

    IEnumerator CheckingCoroutine()
    {


        for (int i = 0; i < 5; i++)
       
        {
            print("Coroutina en marcha");
            yield return new WaitForSeconds(5);
        }
    }

}

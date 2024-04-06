using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushScript : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Slider slider;
    [SerializeField] float baseBrushSize;
    Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f);
        transform.position = camera.ScreenToWorldPoint(mousePosition);
    }

    public void ChangeBrushSize() {
        Debug.Log(slider.value);
        transform.localScale = Vector3.one * baseBrushSize * slider.value;
    }
}

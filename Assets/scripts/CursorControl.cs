using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CursorControl : MonoBehaviour
{
    [SerializeField]
    private Texture2D CursorImage;

    // Start is called before the first frame update
    void Start()
    {

        //Vector2 hotspot = new Vector2(CursorImage.width /2,CursorImage.height/2);//中点
        //Debug.Log("hotspot=" + hotspot);
        //hotspot.y *= -1f;
        //(Texture,ClickPosition(hotspot),Auto or forceSoftware)
        Cursor.SetCursor(CursorImage,Vector2.zero, CursorMode.Auto);
    }
    //texturetypeをcursorにしないとエラー出る

    // Update is called once per frame
    void Update()
    {
        
    }
}

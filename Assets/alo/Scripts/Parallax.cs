using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ParallaxLayer
{
    public Transform background;
    public Vector2 speed;

    [Tooltip("Background Infinite loop on X-Axis")]
    public bool loopOnX;

    [Tooltip("Background Infinite loop on Y-Axis")]
    public bool loopOnY;

    private float _textureUnitWidth;
    public float textureUnitWidth
    {
        get { return _textureUnitWidth; }
    }

    private float _textureUnitHeight;
    public float textureUnitHeight
    {
        get { return _textureUnitHeight; }
    }

    public void update()
    {
        Sprite sprite = background.gameObject.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitWidth = texture.width / sprite.pixelsPerUnit;
        _textureUnitHeight = texture.height / sprite.pixelsPerUnit;
    }
}

public class Parallax : MonoBehaviour
{
    [SerializeField] private List<ParallaxLayer> listLayers;

    Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = Camera.main.transform.position;

        foreach (ParallaxLayer item in listLayers)
        {
            item.update();

            Debug.Log("background: " + item.background + 
                " speed: " + item.speed +
                " textureUnitSize: " + item.textureUnitWidth +
                " textureUnitHeight: " + item.textureUnitHeight);
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 currentPosition = Camera.main.transform.position;

        Vector3 move = currentPosition - previousPosition;

        foreach (ParallaxLayer layer in listLayers)
        {
            //layer.background.position += move * layer.speed;
            layer.background.position += new Vector3(move.x * layer.speed.x, move.y * layer.speed.y);

            if (layer.loopOnX)
            {
                // Test for infinite background loop on X-Axis:
                if (Mathf.Abs(currentPosition.x - layer.background.position.x) >= layer.textureUnitWidth)
                {
                    /**
                     * To check, in this video: https://www.youtube.com/watch?v=wBol2xzxCOU
                     * The guy added an offset to the reset... it may be better.
                     */
                    //float offsetX = (currentPosition.x - layer.background.position.x) % layer.textureUnitWidth;
                    //layer.background.position = new Vector3(currentPosition.x + offsetX, layer.background.position.y);
                    layer.background.position = new Vector3(currentPosition.x, layer.background.position.y);
                }
            }

            if (layer.loopOnY)
            {
                // Test for infinite background loop on Y-Axis:
                if (Mathf.Abs(currentPosition.y - layer.background.position.y) >= layer.textureUnitHeight)
                {
                    layer.background.position = new Vector3(layer.background.position.x, currentPosition.y);
                }
            }
        }

        previousPosition = currentPosition;
    }
}

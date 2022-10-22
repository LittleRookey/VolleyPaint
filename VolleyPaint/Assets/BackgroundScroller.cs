using UnityEngine;
using UnityEngine.UI;
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private RawImage ri;
    public float _x, _y;

    private void Start()
    {
        ri = GetComponent<RawImage>();
    }
    // Update is called once per frame
    void Update()
    {
        ri.uvRect = new Rect(ri.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, ri.uvRect.size);
    }
}

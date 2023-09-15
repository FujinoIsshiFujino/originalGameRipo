using UnityEngine;

public class TstRender : MonoBehaviour
{
    /// <summary>
    /// Rendererが任意のカメラから見えると呼び出される
    /// </summary>
    private void OnBecameVisible()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    /// <summary>
    /// Rendererがカメラから見えなくなると呼び出される
    /// </summary>
    private void OnBecameInvisible()
    {
        this.GetComponent<Renderer>().material.color = Color.blue;
    }
}

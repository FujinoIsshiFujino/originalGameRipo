using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTextAnimator : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var transformCache = transform;
        var defaultPosition = transformCache.localPosition;
        transformCache.localPosition = new Vector3(0, 300f);
        transformCache.DOLocalMove(defaultPosition, 1f)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            transformCache.DOShakePosition(1.5f, 100);
        });

        DOVirtual.DelayedCall(7, () =>
        {
            SceneManager.LoadScene("TitleScene");
        });
    }


}

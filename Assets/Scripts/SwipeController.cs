using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;
using SystemVector3 = System.Numerics.Vector3;
public class SwipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    int currentPage;
    UnityVector3 targetPos;
    [SerializeField] UnityVector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
    }

    public void Next ()
    {
        if(currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous ()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    public void MovePage ()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }
}

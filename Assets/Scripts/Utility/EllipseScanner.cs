using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipseScanner : MonoBehaviour {

    private void Scanner()
    {
        //targetPosition 에 타겟의 위치를 넣는다 적용해야될 타겟이 하나가 아닐경우 배열을 이용해도됨
        Vector3 targetPosition = GameObject.Find("Fighter").gameObject.transform.position;

        // 스캐너의 중심좌표
        Vector3 center = this.gameObject.transform.position;


        float dx = targetPosition.x - center.x;
        float dy = targetPosition.y - center.y;

        // 스캐너의 범위 a는 장축 b는 단축
        float a =3.5f;
        float b =1.3f;


        float l1 = Mathf.Sqrt((dx + Mathf.Sqrt(a * a - b * b)) * (dx + Mathf.Sqrt(a * a - b * b)) + (dy * dy));
        float l2 = Mathf.Sqrt((dx - Mathf.Sqrt(a * a - b * b)) * (dx - Mathf.Sqrt(a * a - b * b)) + (dy * dy));

        // 타겟이 여러개여서 배열을 썻다면 반복문 알아서 사용
        if (l1+l2<=2*a)
        {
            // 범위스캐너 안에 타겟이 잡힘

        }
        else
        {
            // 범위스캐너 안에 타겟이 없음

        }
    }






}

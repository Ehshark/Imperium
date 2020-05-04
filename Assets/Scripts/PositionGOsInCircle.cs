using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGOsInCircle : MonoBehaviour
{
    public GameObject prefab;

    void circle() {
        Vector3 targetPos = Vector3.zero;
        for (int i = 0; i < 10; i++) {
            GameObject instance = Instantiate(prefab);

            float angle = i * (2 * 3.14159f / 10);
            float x = Mathf.Cos(angle) * 1.5f;
            float y = Mathf.Sin(angle) * 1.5f;

            targetPos = new Vector3(targetPos.x + x, targetPos.y + y, 0);

            instance.transform.position = targetPos;
        }
    }

    private void Start()
    {
        //circle();
    }
}

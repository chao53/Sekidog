using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billBoard : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(this.transform.position - Camera.main.transform.position);  //��ȡĿ�귽��
        transform.rotation = rotation;
        float dis = Vector3.Distance(this.transform.position, Camera.main.transform.position);
        //print(dis);
        if(dis < 100 && dis > 40)
        {
            transform.localScale = new Vector3(0.0015f * dis, 0.0015f * dis, 0.0015f * dis);
            this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1.7f + 0.015f * dis, 0);
        }
    }
}

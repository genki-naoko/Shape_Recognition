using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Line : MonoBehaviour
{
    private LineRenderer lineRenderer; 
    private int positionCount; 
    private Camera mainCamera; 
    private bool judge = true; 
    private bool judge2 = false; 
    private double xd; 
    private double yd; 
    private double xd2; 
    private double yd2; 
    public GameObject obj; 
    private double rote_x; 
    private double rote_y; 
    private double radius; 
    private float length;  
    private float new_radius; 
    private float aar = 0; 
    private bool judge3 = false; 
    private bool between = false; 
    private float length2; 
    private bool judge4 = false; 
    private List<double> mix = new List<double>(); 
    private List<double> miy = new List<double>(); 
    private bool kagi = true;
    private float length3; 
    private bool surround_judge = false;
    private bool conti_draw = true;
    void Start() 
    { 
        lineRenderer = GetComponent<LineRenderer>(); 
        // ラインの座標指定を、このラインオブジェクトのローカル座標系を基準にするよう設定を変更 
        // この状態でラインオブジェクトを移動・回転させると、描かれたラインもワールド空間に取り残されることなく、一緒に移動・回転 
        lineRenderer.useWorldSpace = false; 
        positionCount = 0; 
        mainCamera = Camera.main; 
    } 
    void Update() 
    { 
        // このラインオブジェクトを、位置はカメラ前方10m、回転はカメラと同じになるようキープさせる 
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 10; 
        transform.rotation = mainCamera.transform.rotation; 
        if (Input.GetMouseButton(0)) 
        { 
            obj.transform.localScale = new Vector3(1.0f,0.4f,1.0f); 
            // 座標指定の設定をローカル座標系にしたため、与える座標にも手を加える 
            Vector3 pos = Input.mousePosition; 
            pos.z = 10.0f; 
            // マウススクリーン座標をワールド座標に直す 
            pos = mainCamera.ScreenToWorldPoint(pos); 
            // さらにそれをローカル座標に直す。 
            pos = transform.InverseTransformPoint(pos);

            double nowx = pos.x;
            double nowy = pos.y;
            /* 
            if(between) 
            { 
                if(! kagi) 
                { 
                    length2 = (float)Math.Sqrt(Math.Pow(xd2-pos.x, 2) + Math.Sqrt(Math.Pow(yd2-pos.y, 2))); 
                } 
                else if(kagi) 
                { 
                    length2 = (float)Math.Sqrt(Math.Pow(pos.x-xd, 2) + Math.Sqrt(Math.Pow(pos.y-yd, 2))); 
                    kagi = false; 
                } 
                aar += length2; 
                length3 = aar / 8; 
            }
            */ 
            if(between && conti_draw)
            {
                if(!(nowx == xd2 && nowy == yd2))
                {
                    Debug.Log("現在値と１つ前の値は異なります。");

                    for(int i = 0; i < mix.Count; i++)
                    {
                        /*
                        if((nowx == mix[i]) && (nowy == miy[i]))
                        {
                            surround_judge = true;
                            conti_draw = false;
                            break;
                        }
                        */
                        
                        double dix = Math.Abs(nowx - mix[i]);
                        double diy = Math.Abs(nowy - miy[i]);
                        double diff = 0.09;
                        if((dix < diff) && (diy < diff))
                        {
                            surround_judge = true;
                            conti_draw = false;
                            break;    
                        }
                        
                    }
                    /*
                    for(int i = 0; i < mix.Count; i++)
                    {
                        if(Math.Floor((float)pos.x * 10) / 10 == Math.Floor((float)mix[i] * 10) / 10 && Math.Floor((float)pos.y * 10) / 10 == Math.Floor((float)miy[i] * 10)) 
                        {
                            surround_judge = true;
                            conti_draw = false;
                            break;
                        }
                    }
                    */
                }
            }
            if(judge) 
            { 
                xd = pos.x; yd = pos.y;
                Debug.Log("初期位置はx = " + xd + ", y = " + yd);
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                mix.Add(nowx); miy.Add(nowy); 
                judge = false; 
                between = true; 
            } 
            // 得られたローカル座標をラインレンダラーに追加する 
            positionCount++; 
            lineRenderer.positionCount = positionCount; 
            lineRenderer.SetPosition(positionCount - 1, pos); 

            if(between && (!(nowx == xd2 && nowy == yd2)))
            {
                mix.Add(nowx); miy.Add(nowy);
            }

            if(between && (!(nowx == xd2 && nowy == yd2)))
            {
                Debug.Log("現在位置はx = " + nowx + ", y = " + nowy);
                Debug.Log("過去の値はx = " + xd2 + ", y = " + yd2);
                Debug.Log(mix.Count + " " + miy.Count);
                for(int i = 0; i < mix.Count; i++)
                {
                    Debug.Log("x = " + mix[i] + ", y = " + mix[i]);
                }
            }

            /*Debug.Log(mix.Count + " " + miy.Count);*/

            xd2 = pos.x; yd2 = pos.y;

            if(xd != xd2 || yd != yd2) 
            { 
                judge4 = true; 
            } 
            //動的配列に座標を格納 

             

            if(surround_judge)
            {
                Debug.Log("当たってる");
            }
            /* 
            Debug.Log("xd2の型は" + xd2 + "、pos.xの型は" + pos.x);
            Debug.Log("キャスト時" + (double)pos.x);
            */
            /* 
            if(between) 
            { 
                length2 = Math.Sqrt(Math.Pow(xd2-xd, 2) + Math.Sqrt(Math.Pow(yd2-yd, 2))); 
                aar += length2; 
            } 
            xd2 = pos.x; yd2 = pos.y; 
            if(xd == xd2 && yd == yd2) 
            { 
                judge3 = true; 
            } 
            */ 
        } 
        //線形リセット 
        if (!(Input.GetMouseButton(0)) && judge4 && conti_draw) 
        { 
            /*
            judge = true; 
            judge4 = false; 
            kagi = true; 
            between = false; 
            length2 = 0;
            aar = 0;

             
            new_x = (pos.x + xd)/2; 
            new_y = (pos.y + yd)/2; 
            */ 
            if(xd > xd2) 
            { 
                rote_x = xd - xd2; rote_y = yd - yd2; 
            } 
            else if(xd2 > xd) 
            { 
                rote_x = xd2 - xd; rote_y = yd2 - yd; 
            } 
            radius = Math.Atan(rote_y / rote_x) * 180.0 / Math.PI; 
            new_radius = (float)radius; 
            length = (float)Math.Sqrt(Math.Pow(rote_x,2)+Math.Pow(rote_y,2)); 
            obj.transform.localScale = new Vector3(length,0.4f,1.0f); 
            Instantiate(obj, new Vector3(((float)xd + (float)xd2)/2.0f, ((float)yd + (float)yd2)/2.0f, 10.0f), Quaternion.Euler(0,0,new_radius)); 

            Debug.Log("直線の長さは" + length + "です。"); 
            positionCount = 0; 
        } 

        if(!(Input.GetMouseButton(0)) && !(conti_draw))
        {
            Debug.Log("重なりました.");
            mix.Clear(); miy.Clear();
            Debug.Log(mix.Count + " " + miy.Count);
            conti_draw = true;
        } 

        if(!(Input.GetMouseButton(0))) 
        { 
            judge = true; 
            judge4 = false; 
            kagi = true; 
            between = false; 
            length2 = 0;
            aar = 0;
            surround_judge = false;
            mix.Clear(); miy.Clear();
            conti_draw = true;
            positionCount = 0; 
            //*xd2 = 0; yd2 = 0;
        }

        /* 
        //短形リセット 
        if(!(Input.GetMouseButton(0))) 
        { 
        } 
        */ 
    } 
}

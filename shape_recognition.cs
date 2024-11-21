using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineAna : MonoBehaviour
{
    //################################################################################################
    private LineRenderer lineRenderer; private int positionCount; private Camera mainCamera;
    //オブジェクト
    public GameObject obj; public GameObject obj2; private List<GameObject> list_obj = new List<GameObject>();
    //パラメータ(bool)
    private bool first = true; private bool secound = true; private bool fin = true;
    //パラメータ(double)
    double cx; double cy; double px; double py; double first_x; double first_y; double rote_x; double rote_y; int number = 0;
    //外部因子
    GameObject box; Touch script; bool touch_judge = false; Touch t; string str = "no";
    //接地判定
    string seti = null; bool LaserBeam = false;
    //終了条件
    bool fin2 = true; bool say = true;


    //クロージャ判定用パラメータ
    List<float> mix = new List<float>(); List<float> miy = new List<float>();
    int Myplace = 0; int Eneplace = 0;
    float center_x = 0; float center_y = 0; float x_length = 0; float y_length = 0;
    int ymax; int ymin; int xmax; int xmin; bool exTry = true;
    public GameObject l_obj;
    int launcher_num = 0;

    //レーザービーム用パラメータ
    private List<GameObject> laser_list = new List<GameObject>();
    string current_laser; GameObject laser; Laser laser_script; bool com_laser = false;
    public GameObject obj3_laser;

    //################################################################################################

    //物体生成関数
    void MakeObj(double cx = 0.0, double cy = 0.0, double px = 0.0, double py = 0.0, string key = "")
    {
        //軌跡生成
        if (key == "locus")
        {
            if (px > cx)
            {
                rote_x = px - cx; rote_y = py - cy;
            }
            else if (cx > px)
            {
                rote_x = cx - px; rote_y = cy - py;
            }
            double e_radius = Math.Atan(rote_y / rote_x) * 180.0 / Math.PI;
            float radius = (float)e_radius;
            float length = (float)Math.Sqrt(Math.Pow(rote_x, 2) + Math.Pow(rote_y, 2));
            obj.transform.localScale = new Vector3(length, 0.1f, 1.0f);
            GameObject actual_obj = Instantiate(obj, new Vector3(((float)cx + (float)px) / 2.0f, ((float)cy + (float)py) / 2.0f, 10.0f), Quaternion.Euler(0, 0, radius)) as GameObject;
            actual_obj.name = number.ToString();

            seti = actual_obj.name;
            mix.Add(((float)cx + (float)px) / 2.0f); miy.Add(((float)cy + (float)py) / 2.0f);

            // box = GameObject.Find(actual_obj.name);
            //Debug.Log(box.name + "!!!");
            //script = box.GetComponent<Touch>();
            //this.t = FindObjectOfType<Touch>();
            //touch_judge = script.touch;
            //Debug.Log(touch_judge);
            //str = script.Update();
            //str = script.Restr();
            number++;
            list_obj.Add(actual_obj);
        }
        //直線生成(壁)
        else if (key == "line")
        {
            if (px > cx)
            {
                rote_x = px - cx; rote_y = py - cy;
            }
            else if (cx > px)
            {
                rote_x = cx - px; rote_y = cy - py;
            }
            double e_radius = Math.Atan(rote_y / rote_x) * 180.0 / Math.PI;
            float radius = (float)e_radius;
            float length = (float)Math.Sqrt(Math.Pow(rote_x, 2) + Math.Pow(rote_y, 2));
            obj2.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
            Instantiate(obj2, new Vector3(((float)cx + (float)px) / 2.0f, ((float)cy + (float)py) / 2.0f, 10.0f), Quaternion.Euler(0, 0, radius));
        }
        //発射台生成
        else if (key == "launcher")
        {
            l_obj.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
            GameObject actual_launcher = Instantiate(l_obj, new Vector3(center_x, center_y, 10.0f), Quaternion.identity) as GameObject;
            actual_launcher.name = "LauncherNo." + launcher_num.ToString();
            launcher_num++;
            laser_list.Add(actual_launcher);
            Laser ss = actual_launcher.GetComponent<Laser>();
            ss.center_X = center_x;
            ss.center_Y = center_y;

        }
        //レーザー光線生成
        else if (key == "SuperBeam")
        {
            if (px > cx)
            {
                rote_x = px - cx; rote_y = py - cy;
            }
            else if (cx > px)
            {
                rote_x = cx - px; rote_y = cy - py;
            }
            double e_radius = Math.Atan(rote_y / rote_x) * 180.0 / Math.PI;
            float radius = (float)e_radius;
            float length = (float)Math.Sqrt(Math.Pow(rote_x, 2) + Math.Pow(rote_y, 2));
            obj3_laser.transform.localScale = new Vector3(length, 1.0f, 1.0f);
            Instantiate(obj3_laser, new Vector3(((float)cx + (float)px) / 2.0f, ((float)cy + (float)py) / 2.0f, 10.0f), Quaternion.Euler(0, 0, radius));
        }

    }

    void Calc_for_Launcher(int myplace, int eneplace)
    {

        //x座標重心
        float id_x = 0;
        float sum_x = 0;
        for (int i = eneplace + 1; i < myplace; i++)
        {
            sum_x += mix[i];
            id_x += 1.0f;

        }
        center_x = sum_x / id_x;

        //y座標重心
        float id_y = 0;
        float sum_y = 0;
        for (int i = eneplace + 1; i < myplace; i++)
        {
            sum_y += miy[i];
            id_y += 1.0f;

        }
        center_y = sum_y / id_y;

    }



    //パラメータ初期化
    void ResetPara()
    {
        px = 0; py = 0; cx = 0; cy = 0; first_x = 0; first_y = 0; rote_y = 0; rote_x = 0; number = 0; fin2 = true; say = true; seti = null;
        LaserBeam = false; Myplace = 0; Eneplace = 0; mix.Clear(); miy.Clear();
        center_x = 0; center_y = 0; x_length = 0; y_length = 0; exTry = true;
        com_laser = false;
    }

    //物体消去
    void DeleObj()
    {
        for (int i = 0; i < list_obj.Count; i++)
        {
            Destroy(list_obj[i]);
        }
        list_obj.Clear();
    }

    //ログ参照（使い回し用)
    void log()
    {
        Debug.Log(list_obj.Count);
    }

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
            // 座標指定の設定をローカル座標系にしたため、与える座標にも手を加える
            Vector3 pos = Input.mousePosition;
            pos.z = 10.0f;

            // マウススクリーン座標をワールド座標に直す
            pos = mainCamera.ScreenToWorldPoint(pos);

            // さらにそれをローカル座標に直す。
            pos = transform.InverseTransformPoint(pos);

            // 得られたローカル座標をラインレンダラーに追加する
            positionCount++;
            lineRenderer.positionCount = positionCount;
            lineRenderer.SetPosition(positionCount - 1, pos);

            //オブジェクト生成
            //obj.transform.localScale = new Vector3(1.0f,0.4f,1.0f);
            //現在位置を取得
            cx = pos.x; cy = pos.y;

            //2回目以降
            if (!first)
            {
                //3回目以降
                if (!(secound))
                {
                    if (!(cx == px && cy == py))
                    {
                        MakeObj(cx, cy, px, py, "locus");



                    }
                }

                //2回目
                if (secound)
                {
                    if (!(cx == px && cy == py))
                    {
                        secound = false;
                        MakeObj(cx, cy, first_x, first_y, "locus");

                    }
                }

                //衝突判定
                int elu = int.Parse(seti);
                if (elu >= 10)
                {

                    for (int i = elu; i >= (elu - 10); i--)
                    {
                        seti = i.ToString();
                        box = GameObject.Find(seti);
                        script = box.GetComponent<Touch>();
                        if (script.Show() == "yes" && say)
                        {
                            LaserBeam = true;
                            say = false;
                            Myplace = script.ShowMe();
                            Eneplace = script.ShowEnemy();
                            break;
                        }

                    }
                }

            }

            //1回目
            if (first)
            {
                first_x = cx; first_y = cy;
                first = false;

                for (int i = 0; i < laser_list.Count; i++)
                {
                    laser = laser_list[i];
                    laser_script = laser.GetComponent<Laser>();
                    if (laser_script.Judge_laser() == "yes")
                    {
                        com_laser = true;
                        break;
                    }
                }
            }

            //現在位置を過去の位置として保存
            px = pos.x; py = pos.y;

            //終了判定
            if (!(first_x == px && first_y == py))
            {
                fin = false;
            }



        }
        //線形リセット(線形物体生成)
        if (!(Input.GetMouseButton(0)))
        {
            first = true;
            secound = true;
            //DeleObj();
            positionCount = 0;
            if (!(fin))
            {
                if (com_laser)
                {
                    Debug.Log("レーザー発射!!");
                    MakeObj(px, py, (double)laser_script.center_X, (double)laser_script.center_Y, "SuperBeam");
                    Destroy(laser);
                    laser_list.Remove(laser);
                    LaserBeam = false;
                    DeleObj();
                }


                else if (LaserBeam && exTry)
                {
                    Debug.Log("発射台生成");
                    exTry = false;
                    Calc_for_Launcher(Myplace, Eneplace);
                    MakeObj(0.0d, 0.0d, 0.0d, 0.0d, "launcher");
                    DeleObj();


                }
                else
                {
                    Debug.Log("直線生成");
                    MakeObj(px, py, first_x, first_y, "line");
                    DeleObj();
                }

            }
            ResetPara();
            fin = true;
        }
    }
}

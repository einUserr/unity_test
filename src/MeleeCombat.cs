using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeCombat : MonoBehaviour
{
    [Header("wichtige Parameter")]
    [SerializeField] public Transform Sword;
    [SerializeField] private Transform SwordOriginPosition;

    [SerializeField] private float BuffTime = 2f;

    [SerializeField] private float DistanceToSword;

    [Header("Mouse Settings")]
    [SerializeField] private float MouseMaxOffset = 1f;
    [SerializeField] private float MouseSpeed = 1f;

    [Header("Camera")]
    [SerializeField] private Camera mainCam;

    [Header("Audio Source")]
    [SerializeField] private AudioSource AS;

    [Header("Schwert Rotation")]
    [SerializeField] private float SwordRotationSpeed = 400f;
    [SerializeField] private float SwordRotationWhileBlocking = 200f;
    [SerializeField] private float StretchMinRot = -90f;
    [SerializeField] private float StretchMaxRot = 90f;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    [Header("BoxColliderOfSword")]
    [SerializeField] private BoxCollider swordCollider;

    //Parameter die man nicht unbedingt sehen muss im inspector
    private bool isPressed;
    private bool isPressedRight;
    public bool animRuns = false;
    private bool lastPosActive = false;
    private Vector3 lastRotation;
    private float SwordRotationZ = 0f;
    private Vector3 MousePos;
    private Quaternion HandNormalRotation = new Quaternion(0.451083183f, -0.548753858f, 0.565333605f, 0.419274449f);
    private float RotationXValue = 0f;
    public bool isStretched = false;
    private bool upperStrike = false;
    private float ActivateTimer = 0f;
    public bool gotBlocked = false;
    private bool alreadyRunning = false;
    public Transform SwordEndPos;
    public int randomBlockNum = 0;

    public bool SideStrike = false;
    public bool UpperStrike = false;


    private void Start()
    {
        //hier clampen wir die Werte, damit das Schwert sich nicht zu weit weg bewegt
        MousePos.x = Mathf.Clamp(MousePos.x, -MouseMaxOffset, MouseMaxOffset);
        MousePos.y = Mathf.Clamp(MousePos.y, -0.5f, 0.3f);
        MousePos.z = Mathf.Clamp(MousePos.z, -MouseMaxOffset, MouseMaxOffset);
    }

    void Update()
    {
        MousePos_Speed_ButtonPressed();
        ActivateTimer += Time.deltaTime;

        if(gotBlocked == true && ActivateTimer >= BuffTime)
        {
            gotBlocked = false;
            alreadyRunning = false;
        }

        //Wenn der isPressed bool true ist, wird der Code hier ausgeführt, der das Schwert bewegt
        if (isPressed && gotBlocked == false)
        {
            SwordPosAndRot();

            if (MousePos.x >= 0.42f || MousePos.z >= 0.42f || MousePos.x <= -0.42f || MousePos.z <= -0.42f)
            {
                if(lastPosActive == false)
                {
                   AS.Play();
                   lastRotation = Sword.eulerAngles;
                   lastPosActive = true;
                   swordCollider.enabled = true;
                   isStretched = true;
                   SideStrike = true;
                    GetRandomBlockingNum();
                }
           
            }
            else if(MousePos.y >= 0f)
            {
                lastRotation = Sword.eulerAngles;
                swordCollider.enabled = true;
                isStretched = true;
                upperStrike = true;
                UpperStrike = true;
                GetRandomBlockingNum();
            }             
        }
        else if(isPressedRight == true && gotBlocked == false)
        {
            rightMouseDown();
        }
        else
        {
            NoMouseButtonDown();
            if(alreadyRunning == false)
            {
                ActivateTimer = 0f;
                alreadyRunning = true;
            }
        }
    }

    private void SwordPosAndRot()
    {
        Sword.position = MousePos + mainCam.transform.position;
        float RotX = Input.GetAxis("Mouse X") * SwordRotationSpeed * Time.deltaTime;

        //hier clampen wir die maximale und minimale Schwertrotation
        SwordRotationZ = Mathf.Clamp(SwordRotationZ, -90f, 90f);
        SwordRotationZ -= RotX;

        //Die Rotation basierend von der Maus einfügen, bevor wir angreifen
        if (lastPosActive == false && upperStrike == false)
        {
            Sword.localRotation = Quaternion.Euler(0, 0, SwordRotationZ) * HandNormalRotation;
        }
        else if (lastPosActive == true && upperStrike == false)
        {
            float RotationX = Input.GetAxis("Mouse X") * Time.deltaTime * SwordRotationSpeed;
            float RotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * SwordRotationSpeed;
            //RotationXValue wird hier als allgemeine Summe benutzt, von der Maus X Axe, sowie die Maus Y Achse
            RotationXValue += RotationX;
            RotationXValue += RotationY;

            RotationXValue = Mathf.Clamp(RotationXValue, StretchMinRot, StretchMaxRot);

            Sword.rotation = Quaternion.Euler(lastRotation.x, lastRotation.y + RotationXValue, lastRotation.z);
            //Debug.Log(RotationSum);
        }
        else if (upperStrike)
        {
            float RotationX = Input.GetAxis("Mouse X") * Time.deltaTime * SwordRotationSpeed;
            float RotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * SwordRotationSpeed;
            //RotationXValue wird hier als allgemeine Summe benutzt, von der Maus X Axe, sowie die Maus Y Achse
            RotationXValue += RotationX;
            RotationXValue += RotationY;

            RotationXValue = Mathf.Clamp(RotationXValue, -160, -50);

            Sword.localRotation = Quaternion.Euler(-50 - RotationXValue, 0, 0) * HandNormalRotation;
            //Debug.Log(RotationSum);
        }
    }

    private void MousePos_Speed_ButtonPressed()
    {
        //Der Bool isPressed wird true wenn wir die linke Maustaste gedrückt halten
        isPressed = Input.GetMouseButton(0);
        isPressedRight = Input.GetMouseButton(1);

        //TargetChaser.position = Sword.position;

        //Die Position der Maus herausfinden
        MousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceToSword)) - mainCam.transform.position;
        //Vector3 MousePos = mainCam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceToSword)) - mainCam.transform.position;


        //Die Maus Geschwindigkeit berechnen
        float mouseX = Input.GetAxis("Mouse X") * MouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSpeed * Time.deltaTime;
        float Speed = mouseX + mouseY;
        Speed = Mathf.Abs(Speed);
        //Vector3 lastPos = MousePos;
    }

    private void rightMouseDown()
    {
        animRuns = true;
        Sword.position = MousePos + mainCam.transform.position;
        //RotX ist der Wert 1f bis -1f der von der Mausbewegung abhängt
        float RotX = Input.GetAxis("Mouse X") * SwordRotationWhileBlocking * Time.deltaTime;
        swordCollider.enabled = true;

        //hier clampen wir die maximale und minimale Schwertrotation
        SwordRotationZ = Mathf.Clamp(SwordRotationZ, -90f, 90f);
        SwordRotationZ -= RotX;
        Sword.localRotation = Quaternion.Euler(0, 0, 90f + SwordRotationZ) * HandNormalRotation;
    }

    public void NoMouseButtonDown()
    {
        Sword.position = SwordOriginPosition.position;
        Sword.rotation = SwordOriginPosition.rotation * HandNormalRotation;
        SwordRotationZ = 0f;
        RotationXValue = 0f;
        animRuns = false;
        lastPosActive = false;
        swordCollider.enabled = false;
        isStretched = false;
        upperStrike = false;
        UpperStrike = false;
        SideStrike = false;
    }

    private void GetRandomBlockingNum()
    {
        randomBlockNum = Random.Range(0, 100);
    }


}

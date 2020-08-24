using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Deklarasi variabel kecepatan jalan dan rotasi player 
    public float moveSpeed;
    public float TurnSpeed;

    //Deklarasi button movement bagi player
    Vector2 input;
    
    //Deklarasi sudut rotasi player
    Quaternion targetRotation;
    float angle;

    //Deklarasi animasi bagi karakter player
    public Animator anim;

    //Inisiasi awal program
    void Start ()
    {
        //Pemanggilan animator untuk animasi karakter player
        anim = GetComponent<Animator>();
    }

    //Kelas untuk mengambil nilai x dan y
    void GetInput()
    {
        //Pengambilan nilai x dan y sesuai dengan tombol yang ditekan oleh user (W, A, S, D) atau (Up, Down, Left, Right)
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }


    //Kelas untuk menghitung arah pergerakan player sesuai dengan nilai x dan y
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
    }


    //Kelas untuk menghitung arah rotasi dan kecepatan rotasi
    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);

        //Inisiasi animasi jalan pada karakter player
        anim.SetBool("isWalking", true);
    }

    //Kelas untuk menghitung pergerakan karakter player
    void Move()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //Inisiasi animasi jalan pada karakter player
        anim.SetBool("isWalking", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "NPC")
        {
            Application.Quit();
            Debug.Log("NPC menangkap player");
            
        }
    }
    //Update dijalankan setiap frame pada game (30 frame per detik)
    void Update ()
    {
        //Pemanggilan kelas GetInput
        GetInput();

        //Pengkondisian jika karakter diam maka animasi jalan akan dihentikan
        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1)
        {
            anim.SetBool("isWalking", false);
            return;
        }

        //Pemanggilan kelas CalculateDirection,Rotate dan Move
        CalculateDirection();
        Rotate();
        Move();
        //OnCollisionEnter();
    }
}

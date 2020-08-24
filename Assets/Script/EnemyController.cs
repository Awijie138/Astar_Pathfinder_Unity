using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class EnemyController : MonoBehaviour {

    //Deklarasi variabel player
    public GameObject player;

    //Deklarasi navmesh agent bagi karakter NPC
    public NavMeshAgent enemy;

    //Deklarasi animasi bagi karakter NPC
    public Animator anim;

    //Deklarasi variabel untuk penghitungan waktu komputasi, waktu running, dan status player 
    public Stopwatch computingTimer,runningTimer;
    public TimeSpan computing_timeSpan,running_timeSpan;
    public int mean,tick,minute,second,milisecond;
    public String status;

    //Deklarasi variabel untuk menampilkan node jalur
    private Vector3 path1;
    public GameObject cube,cubePath;
    public int pathDifference = 0;

    //Inisiasi awal program
    void Start () {

        //Pemanggilan variabel navmesh agent
        enemy = GetComponent<NavMeshAgent>();
        enemy.updateRotation = false;

        //Pemanggilan animator untuk animasi karakter NPC
        anim = GetComponent<Animator>();
        
        //Inisiasi waktu mulai penghitungan komputasi navmesh dan running program
        Debug.Log("Waktu komputasi mulai");
        computingTimer = Stopwatch.StartNew();
        runningTimer = Stopwatch.StartNew();

        //Inisiasi penghitungan waktu rata-rata komputasi navmesh
        mean = 0;
        tick = 0;     
    }
	
	//Update dijalankan setiap frame pada game (30 frame per detik)
	void Update () {  

        //Pengkondisian jika karakter player ditemukan
        if (player != null)
        {
            status = ("player ditemukan");

            //Pemanggilan proses penghitungan jalur berdasarkan posisi NPC, posisi player dan struktur navmesh yang telah dipanggil
            enemy.SetDestination(player.transform.position);
           

            //Pengkondisian untuk mengatur rotasi NPC sesuai dengan jalur
            if (enemy.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(enemy.velocity.normalized);
            }

            // Pengkondisian untuk menjalankan animasi jalan jika jarak NPC dan jarak player dibawah dengan jarak yang ditentukan
            if (enemy.remainingDistance < 8)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }
        }

        else
        {
            status = ("player tidak ditemukan");
        }

        //Penambahan waktu tick/update setiap kelas update dijalankan
        tick++;

        //Menginisaisi method untuk menampilkan node dalam bentuk kubus
        pathViewer();

        //Penghentian waktu komputasi navmesh
        computingTimer.Stop();
        computing_timeSpan = computingTimer.Elapsed;
        Debug.Log(" Waktu komputasi = " + computing_timeSpan.Minutes + " : " + computing_timeSpan.Seconds + " : " + computing_timeSpan.Milliseconds);
        //Penambahan waktu komputasi pada variabel mean untuk penghitungan waktu rata-rata komputasi
        computingTimer = Stopwatch.StartNew();
        mean += computing_timeSpan.Milliseconds; 
    }


    //Method untuk menampilkan node jalur NPC dan menampilkan koorninat node tersebut dalam bentuk kubus
    void pathViewer()
    {

        //Mengambil panjang array node jalur
        int pathLegth = enemy.path.corners.Length;

        //Mendeteksi jika jumlah array jalur berubah
        if (pathDifference != pathLegth)
        {

            //Perungalan untuk memunculkan dan menempatkan kubus sesuai koordinat node
            for (int j = 0; j < pathLegth; j++)
            {
                cubePath = Instantiate(cube) as GameObject;
                cubePath.transform.position = enemy.path.corners[j];

                /*Penghapusan objek kubus pada node awal dan node akhir agar tidak terjadi pemunculan kubus secara 
                berkelanjutan ketika karakter NPC dan player bergerak*/
                if (j==0 || j==pathLegth-1)
                {
                    Destroy(cubePath);
                }

                //Debug.log untuk menampiulkan koordinat node 
                /*Vector3 path1 = enemy.path.corners[j];
                Debug.Log(path1);*/
            }
        }
        else
        {

        }
    }

    //Kelas dijalankan ketika program dihentikan
    private void OnApplicationQuit()
    {
        //Penghentian waktu running program
        runningTimer.Stop();
        running_timeSpan = runningTimer.Elapsed;

        //Penghitungan waktu rata-rata komputasi navmesh
        mean /= tick;

        Debug.Log("Program dihentikan");
        Debug.Log(" Waktu running program = " + running_timeSpan.Minutes + " Menit, " + running_timeSpan.Seconds + " Detik, " + running_timeSpan.Milliseconds + " Milidetik ");
        Debug.Log("Rata-rata waktu komputasi navmesh = " + mean + " mili detik");
        Debug.Log(" Status player : " + status);
    }
}

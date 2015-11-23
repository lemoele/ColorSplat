using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAI : MonoBehaviour {

    public Animation anim;
    public GameObject camera;

    private List<Transform> RndPos;
    private float localTime = 0.0f;
    private string mode;
    private Transform target;

	void Start () {
        anim = GetComponent<Animation>();
        RndPos = GameObject.Find("Characters").GetComponent<RndPositions>().RandomPositions; 


        anim["Walk"].speed = 2.2f;

        // Set all animations to loop
        anim.wrapMode = WrapMode.Loop;

        camera = GameObject.Find("Main Camera");

        mode = "Idle";
        anim.Play("Idle");
	}
	
	// Update is called once per frame
	void Update () {
        localTime += Time.deltaTime/1.0f;

        if (mode == "Idle")
        {
            // Always face the camera
            RotateTowards(camera.transform, this.transform, 10.0f);

            if (localTime > 2)
            {
                mode = "Walk";
                anim.CrossFade("Walk");

                // Update target
                int maxIndex = RndPos.Count;
                int index = Random.Range(0, maxIndex);
                target = RndPos[index];
            }
        }
        else if (mode == "Walk")
        {
            // Check if to stop walk
            float distance = Vector3.Distance(target.position, this.transform.position);

            if (distance < 1.1f)
            {
                mode = "Idle";
                anim.CrossFade("Idle");
                localTime = 0.0f;
            }
            else
            {
                MoveCharacter(target, this.transform);
            }
        }
        else if (mode == "Shop")
        {

        }
	}


    void MoveCharacter(Transform target, Transform cePos){
        // Variables
        float MaxSpeed = 0.1f;
        float MaxRotationSpeed = 10.0f;
        Vector3 targetPos = target.position - Vector3.Normalize(this.transform.position);
        
        // Rotation
        RotateTowards(target, cePos, MaxRotationSpeed);

        // Movement
        this.transform.position = Vector3.MoveTowards(cePos.position, targetPos, MaxSpeed);
    }


    void RotateTowards(Transform target, Transform cePos, float MaxRotationSpeed)
    {
        Vector3 targetDir = target.position - cePos.position;
        float step = MaxRotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        newDir.y = 0;
        this.transform.rotation = Quaternion.LookRotation(newDir);
    }
}

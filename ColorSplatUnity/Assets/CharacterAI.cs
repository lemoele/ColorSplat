using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAI : MonoBehaviour {

	public Animation anim;
    public List<Transform> RndPositions; 
    private float localTime = 0.0f;

	void Start () {

        // except shooting
        anim = GetComponent<Animation>();

        // Set all animations to loop
        anim.wrapMode = WrapMode.Loop;

        //anim.Play("Walk");
        //anim.CrossFadeQueued("Walk");
	}
	
	// Update is called once per frame
	void Update () {
        localTime += Time.deltaTime/1.0f;
        if (localTime > 2 & anim.IsPlaying("Walk")==false)
        {
            anim.CrossFade("Walk");
        }
        if (anim.IsPlaying("Walk"))
        {
            MoveCharacter(RndPositions[0], this.transform);
        }
	}


    void MoveCharacter(Transform target, Transform cePos){
        
        float MaxSpeed = 0.01f;
        Vector3 tmpRotation = Vector3.RotateTowards(cePos.position, target.position, 100.0f, 100.0f);
        //tmpRotation.z = 0;
        this.transform.rotation = Quaternion.Euler(tmpRotation);
        this.transform.position = Vector3.MoveTowards(cePos.position, target.position, MaxSpeed);
    }
}

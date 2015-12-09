using UnityEngine;
using System.Collections;
using System;

public class VirParticleRenderer : MonoBehaviour {

    //----- Public Variables
    public int PPS = 100;
    public float lifetime = 5.0f;
    public float startSize = 1.0f;
    public float endSize = 1.0f;
    public float startRotation;
    public float endRotation;
    public float startVelocity = 10.0f;
    public float endVelovity = 1.0f;
    public Color startColor = new Color(255,255,255);

    public float cloudHeight = 14.0f;
    public float cloudWidth = 20.0f;
    public float cloudDepth = 8.0f;
    public Vector3 cloudForce = new Vector3(0,2.5f,0);
    public Vector3 cloudMove; 

    public bool velocityOnAge;
    public bool rotationOnVelocity;
    public bool wind;
    public bool castShadows = true;
    public bool physicalMaterial;

    public Material fogMaterial;
    public Shader fogShader;

    public enum myEnum // your custom enumeration
    {
        Point,
        Line,
        PlaneIsComing,
        CubeIsComing
    };
    public myEnum Emitter = myEnum.Point;  // this public var should appear as a drop down
    public Vector3 EmitterVector;

    //---- Private Var
    private GameObject Particlesdir;
    private GameObject particle;
    private float time;



    private bool test = false;

	// -------------------------------------------
    // -------------------------------------------
	void Start () {
        particle = new GameObject("PrefabParticle");
        particle.transform.parent = this.transform;
        particle.transform.position = this.transform.position;

        // Create particle prefab
        float size = startSize/10;
        Mesh m = new Mesh();
        m.vertices = new [] {new Vector3(-size, -size, 0.01f), new Vector3(size, -size, 0.01f), new Vector3(size, size, 0.01f), new Vector3(-size, size, 0.01f) };
        m.uv = new Vector2[] { new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 1), new Vector2 (1, 0)};
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        MeshFilter meshFilter = (MeshFilter)particle.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = m;
        MeshRenderer renderer = particle.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = fogMaterial;
        //renderer.material.shader = Shader.Find("Particles/Additive");
        renderer.material.shader = fogShader;
        renderer.enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (test == false) { 
            time += Time.deltaTime;

            int noParticles = (int)Math.Round(time * PPS, 0);
            if (noParticles > 0)
            {
                time = 0;
                test = false;
            }

            for (int i = 0; i < noParticles; i++)
            {
                GameObject clonePartic = Instantiate(particle);
                clonePartic.GetComponent<MeshRenderer>().enabled = true;
                clonePartic.transform.parent = this.transform;
                clonePartic.name = "ParticleX";

                // EMITTER
                if (Emitter == myEnum.Point)
                {
                    clonePartic.transform.position = this.transform.position;
                }else if (Emitter == myEnum.Line){
                    Vector3 line = EmitterVector / 2;
                    Vector3 pos = new Vector3(UnityEngine.Random.Range(-line.x, line.x), UnityEngine.Random.Range(-line.y, line.y), UnityEngine.Random.Range(-line.z, line.z));
                    clonePartic.transform.position = pos + this.transform.position;
                }




                //
                particleController particleScript = (particleController)clonePartic.AddComponent(typeof(particleController));
                particleScript.startVelocity = UnityEngine.Random.Range(startVelocity*0.99f, startVelocity*0.99f);

                particleScript.startDirection = new Vector3(UnityEngine.Random.Range(-cloudWidth / 2 + cloudForce.x, cloudWidth + cloudForce.x), UnityEngine.Random.Range(-cloudHeight / 2 + cloudForce.y, cloudHeight / 2 + cloudForce.y), UnityEngine.Random.Range(-cloudDepth/2 + cloudForce.z, cloudDepth/2 + cloudForce.z));
                


                // Move cloud
                Vector3 cloudpos = this.transform.position;
                if (cloudpos.x < -40.0f)
                {
                    cloudpos.x = 40.0f;
                    this.transform.position = cloudpos;
                }
                else
                {
                    this.transform.position = cloudpos + cloudMove / 50;
                }

            }
           
        }
	}
}

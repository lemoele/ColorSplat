using UnityEngine;
using System.Collections;

public class particleController : MonoBehaviour {

    public Vector3 startPos;
    public Vector3 startDirection;
    public float startVelocity;
    public MeshRenderer renderer;

    private float lifetime;
    private float age;
    private float startSize;
    private float endSize;
    
    private float endVelocity;
    private GameObject camera;
    private float selfRotation;

	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera");

        GameObject main = GameObject.Find("PARTICLE_RENDERER");
        VirParticleRenderer VirParticleRendererScript = main.GetComponent<VirParticleRenderer>();

        lifetime = VirParticleRendererScript.lifetime;
        startSize = VirParticleRendererScript.startSize;
        endSize = VirParticleRendererScript.endSize;
        //startVelocity = VirParticleRendererScript.startVelocity;
        endVelocity = VirParticleRendererScript.endVelovity;

        RotateTowards(camera.transform, this.transform, 1000.0f);

        renderer = this.gameObject.GetComponent<MeshRenderer>();

        selfRotation = Random.Range(-15.0f, 15.0f);
	}
	
	// Update is called once per frame
	void Update () {
        age += Time.deltaTime;
        if (age > lifetime)
        {
            Destroy(gameObject);
        }

        float process = age / lifetime;

        // Change size over time
        float size = process*(endSize-startSize) + 1;
        this.transform.localScale = new Vector3(size,size,size);

        // Change speed over time and move
        float velocity = (process*(endVelocity-startVelocity) + startVelocity)/400;
        // Move
        Vector3 cePos = this.transform.position;
        this.transform.position = Vector3.MoveTowards(cePos, cePos + startDirection, velocity);//0.025f);

        // Change transparancy over time
        renderer.material.SetColor("_TintColor", new Color(1, 1, 1, 1-process));

        // make sure it turns towards the camera all the time (advanced) removed for rendering time
        //RotateTowards(camera.transform, this.transform, 1000.0f);

        // Rotate
        this.transform.Rotate(Vector3.forward * Time.deltaTime * selfRotation, Space.Self);
	}


    void RotateTowards(Transform target, Transform CE, float MaxRotationSpeed)
    {
        Vector3 targetDir = target.position - CE.position;
        Quaternion temp = Quaternion.LookRotation(targetDir);
        this.transform.rotation = temp;

    }
}

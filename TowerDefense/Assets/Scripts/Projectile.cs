using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float speed = 4.0f;
	public float damage = 10.0f;
    public float timer = 5.0f;

	// Use this for initialization
	void Start () 
	{
        StartCoroutine(HariKari());
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	public void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Minion") 
		{
            other.gameObject.GetComponent<Minion>().TakeDamage(damage);
			Destroy (gameObject);
		}
	}

    IEnumerator HariKari ()
    {
        yield return new WaitForSeconds(timer);
        print("agljkhdashjkgf");
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}

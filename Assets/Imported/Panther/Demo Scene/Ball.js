#pragma strict

var speed : float = 200;
var range : float = 400;

var ExploPtcl : GameObject;

private var dist : float;

function Update () {

	// Move Ball forward
	transform.Translate(Vector3.forward * Time.deltaTime * speed);
	
	// Record Distance.
	dist += Time.deltaTime * speed;
	
	// If reach to my range, Destroy. 
	if(dist >= range) {
		DestroyBall();
	}
}

function DestroyBall()
{
	Instantiate(ExploPtcl, transform.position, transform.rotation);
	Destroy(gameObject);
}


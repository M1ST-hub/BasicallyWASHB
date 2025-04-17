using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform bulletExit;
    public GameObject bulletPref;

    public float range = 100f;
    public int damage = 10;

    public Camera cam;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }
    }

    public void ShootBullet()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        
        GameObject laser = Instantiate(bulletPref, bulletExit.position, bulletExit.rotation);
        Destroy(laser, 2f);
    }
}

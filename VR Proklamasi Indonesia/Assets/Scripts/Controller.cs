using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float _speed = 1f;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude < _speed)
        {
            float value = Input.GetAxis("Vertical");
            if (value != 0)
                _rigidbody.AddForce(0, 0, value * Time.fixedDeltaTime * 1000f);
        }
    }
}

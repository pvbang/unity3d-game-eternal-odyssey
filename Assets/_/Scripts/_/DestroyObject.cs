using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public GameObject destroyObject;
    public bool destroy = false;
    public bool destroyParentObject = false;
    public bool waitAnimation = false;

    private void Awake()
    {
        // nếu destroyParentObject = true thì sẽ destroy parent object
        if (destroyParentObject)
        {
            destroyObject = transform.parent.gameObject;
        }
        // nếu destroyObject = null thì sẽ destroy object hiện tại
        else if (destroyObject == null)
        {
            destroyObject = gameObject;
        }
    }

    private void Start()
    {
        if (destroy == false) return;

        // nếu waitAnimation = true thì sẽ delay destroy theo thời gian của animation
        if (waitAnimation)
        {
            StartCoroutine(CoroutineHelper.DelaySeconds(() => Destroy(destroyObject), GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
        }
        else
        {
            Destroy(destroyObject);
        }
    }

    // hàm này sẽ destroy object
    public void Destroy()
    {
        Destroy(destroyObject);
    }
}

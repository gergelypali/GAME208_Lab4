using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// just a small script to intantiate and spin a sample prefab on the spawn menu
public class prefabShower : MonoBehaviour
{
    private
        Rigidbody tempUnit;

    public
        Enemy prefab;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 tempLoc = this.gameObject.transform.position + new Vector3(0,0,-0.1f);
        tempUnit = Instantiate(prefab.rb, tempLoc, this.gameObject.transform.rotation, this.gameObject.transform);
        tempUnit.constraints = RigidbodyConstraints.FreezeAll;
        tempUnit.transform.localScale = new Vector3(40,40,40);
    }

    // Update is called once per frame
    void Update()
    {
        tempUnit.transform.Rotate(0, 0, 100 * Time.deltaTime);
    }

}

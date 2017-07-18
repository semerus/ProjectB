using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanner : MonoBehaviour {

    #region Monobehaviours
    void Awake()
    {
        holder = transform.root.gameObject.GetComponent<Character>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null && other.gameObject.layer != 10)
        {
            print(other.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.layer != 10)
        {
            print(other.gameObject.name);
        }
    }

    #endregion

    #region Field & Method
    [SerializeField]
    private Character holder;

    private List<Character> friendlyCharacters;
    private List<Character> NeutralCharacters;
    private List<Character> HostileCharacters;
    
    public void AddColider()
    {

    }

    public void RemoveColider()
    {

    }

    #endregion
}

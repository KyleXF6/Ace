using UnityEngine;
using UnityEngine.EventSystems;

public class Actor : MonoBehaviour
{
    public string Name { get; set; }

    public string ID { get; set; }
    public Actor()
    {

    }
    public Actor(string name)
    {
        Name = name;
    }

    public static Actor Phoenix = new Actor("Phoenix Wright");
    public static Actor Maya = new Actor("Maya Fey");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

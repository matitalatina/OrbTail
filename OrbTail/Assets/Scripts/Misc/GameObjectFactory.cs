using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Reduces the overhead of instantiating and destroying game objects
/// </summary>
public class GameObjectFactory {

    private static GameObjectFactory instance_;

    /// <summary>
    /// Returns the singleton's instance
    /// </summary>
    public static GameObjectFactory Instance
    {
        
        get
        {

            if (instance_ == null)
            {

                instance_ = new GameObjectFactory();

            }

            return instance_;

        }

    }

    /// <summary>
    /// Instantiate a new object
    /// </summary>
    /// <param name="resource_path">The path of the game object to instiate</param>
    public GameObject Instantiate(string resource_path, Vector3 positon, Quaternion rotation)
    {

        Stack<GameObject> object_stack = null;

        if (resource_table_.TryGetValue(resource_path, out object_stack) &&
            object_stack.Count > 0)
        {

            //Activate an idle object
            var game_object = object_stack.Pop();

            game_object.transform.position = positon;
            game_object.transform.rotation = rotation;
            game_object.SetActive(true);

            return game_object;

        }
        else
        {

            return InstantiateLocal(resource_path, positon, rotation);

        }

    }

    /// <summary>
    /// Destroy a game object knowing its resource path
    /// </summary>
    public void Destroy(string resource_path, GameObject game_object)
    {
  
        //Deactivate the object and push it into the stack
        game_object.SetActive(false);

        GetDefaultStack(resource_path).Push(game_object);
        
    }

    /// <summary>
    /// Preloads a some resources up to count
    /// </summary>
    public void Preload(string resource_path, int count)
    {

        var object_stack = GetDefaultStack(resource_path);

        PreloadMore(resource_path, object_stack.Count + count);

    }

    /// <summary>
    /// Preloads additional resources
    /// </summary>
    public void PreloadMore(string resource_path, int count)
    {

        var resource = Resources.Load(resource_path);

        var object_stack = GetDefaultStack(resource_path);

        GameObject game_object;

        while (count > 0)
        {

            game_object = GameObject.Instantiate(resource) as GameObject;

            game_object.SetActive(false);

            object_stack.Push(game_object);

            --count;

        }

    }

    private GameObjectFactory()
    {

        resource_table_ = new Dictionary<string, Stack<GameObject>>();

    }

    /// <summary>
    /// Instantiate a local object
    /// </summary>
    private GameObject InstantiateLocal(string resource_path, Vector3 positon, Quaternion rotation)
    {

        //Instantiate a new object
        var resource = Resources.Load(resource_path);
        var game_object = GameObject.Instantiate(resource, positon, rotation) as GameObject;

        return game_object;
        
    }

    /// <summary>
    /// Get a default stack for the resources or generates a new one
    /// </summary>
    private Stack<GameObject> GetDefaultStack(string resource_path)
    {

        Stack<GameObject> object_stack = null;

        if (!resource_table_.TryGetValue(resource_path, out object_stack))
        {

            //Create a new stack if it is not present
            object_stack = new Stack<GameObject>();

            resource_table_.Add(resource_path, object_stack);

        }

        return object_stack;

    }

    private IDictionary<string, Stack<GameObject>> resource_table_;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This base class is used across the kit in order to have types with unique identifiers that
/// increase automatically.
/// </summary>
public class Resource
{
    /// <summary>
    /// The unique identifier of this resource.
    /// </summary>
    public int id;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="id">The unique identifier of the resource.</param>
    public Resource(int id)
    {
        this.id = id;
    }
}

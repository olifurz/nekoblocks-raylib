namespace Gorge.Core;

/// <summary>
/// Base service class that all services derive from
/// </summary>
public class BaseService
{
    /// <summary>
    /// Called when the service first begins
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// Service update loop
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Called when the service should be stopped
    /// </summary>
    public virtual void Stop() { }
}
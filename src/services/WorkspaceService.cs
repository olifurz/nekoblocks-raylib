using System.Numerics;
using System.Threading.Tasks;
using Gorge.Core;
using Gorge.World;
using Microsoft.VisualBasic;

namespace Gorge.Services;

/// <summary>
/// Service that contains & manages all objects inside the loaded game
/// </summary>
public class WorkspaceService : BaseService
{
    public List<WorldObject> Objects = [];

    private Player? localPlayer;

    private PhysicsService? physics;
    public Skybox Skybox;
    public override void Start()
    {
        base.Start();
        physics = ServiceManager.GetService<PhysicsService>();

        var baseplate = new Part(Part.PartType.Brick, Vector3.Zero, Quaternion.Identity, new Vector3(16, 1, 16));
        baseplate.Name = "Baseplate";
        baseplate.Anchored = true;
        AddObject(baseplate);

        localPlayer = new Player();
        localPlayer.Start();
        AddObject(localPlayer);

        Skybox = new Skybox("textures.skybox.png", false);
    }

    public override void Update()
    {
        base.Update();

        localPlayer?.Update();
    }

    public Player? GetLocalPlayer()
    {
        return localPlayer;
    }

    /// <summary>
    /// Appends an object instance to the object collection
    /// </summary>
    /// <param name="obj">The object to add to the list</param>
    /// <returns>Unique ID to the object</returns>
    public int AddObject(WorldObject obj)
    {

        Objects.Add(obj);
        obj.Id = Objects.Count + 1;

        if (obj.GetType() == typeof(Part))
            physics?.AddBody((Part)obj);

        return obj.Id;
    }

    public override void Stop()
    {
        base.Stop();
    }
}
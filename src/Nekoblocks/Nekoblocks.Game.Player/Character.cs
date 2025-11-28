using System.Numerics;
using Jitter2;
using Jitter2.Dynamics.Constraints;
using Jitter2.LinearMath;
using Nekoblocks.Core;
using Nekoblocks.Game.Objects;
using Nekoblocks.Services;

namespace Nekoblocks.Game.Player;

public class Character : Part
{
    PhysicsService physicsService = ServiceManager.GetService<PhysicsService>();

    float walkCycle = 0;
    public Dictionary<string, Part> BodyParts = new Dictionary<string, Part>
    {
        { "Head", new Part(PartType.Brick)},
        { "Torso", new Part(PartType.Brick)},
        { "LeftArm", new Part(PartType.Brick)},
        { "RightArm", new Part(PartType.Brick)},
        { "LeftLeg", new Part(PartType.Brick)},
        { "RightLeg", new Part(PartType.Brick)},
    };
    public Character()
    {
        Name = "Character";
        Transform.SetScale(4, 5, 1);
        Transform.SetPosition(0, 30, 0);
        Transparency = 1;
        CanCollide = true;
        Transform.Anchored = false;

        var upright = physicsService.world.CreateConstraint<HingeAngle>(RigidBody, physicsService.world.NullBody);
        upright.Initialize(JVector.UnitY, AngularLimit.Full);

        foreach (var part in BodyParts)
        {
            part.Value.SetParent(this);
            part.Value.Name = part.Key;
            part.Value.Transform.Anchored = true;
            part.Value.CanCollide = false;

            switch (part.Value.Name)
            {
                case "Head":
                    part.Value.Transform.SetScale(1);
                    break;
                case "Torso":
                    part.Value.Transform.SetScale(2, 2, 1);
                    break;
                case "LeftArm":
                case "RightArm":
                case "LeftLeg":
                case "RightLeg":
                    part.Value.Transform.SetScale(1, 2, 1);
                    break;
            }
        }
    }

    public void StepWalkCycle()
    {
        walkCycle++;
        if (walkCycle > 60) walkCycle = 0;

        var trans = BodyParts["LeftArm"].Transform;
        var target = float.Lerp(-45, 45, walkCycle / 60);
        trans.SetRotation(trans.Rotation.X, target, trans.Rotation.Z);
        Log.Debug(target.ToString());

    }

    public void Update()
    {
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        BodyParts["Torso"].Transform.SetPosition(Transform.Position.X, Transform.Position.Y + 0.5f, Transform.Position.Z);
        BodyParts["Torso"].Transform.SetRotation(Transform.Rotation);
        Vector3 origin = BodyParts["Torso"].Transform.Position;
        BodyParts["Head"].Transform.SetPosition(origin.X, origin.Y + 1.5f, origin.Z);
        BodyParts["LeftArm"].Transform.SetPosition(origin.X - 1.5f, origin.Y, origin.Z);
        BodyParts["RightArm"].Transform.SetPosition(origin.X + 1.5f, origin.Y, origin.Z);
        BodyParts["LeftLeg"].Transform.SetPosition(origin.X - 0.5f, origin.Y - 2f, origin.Z);
        BodyParts["RightLeg"].Transform.SetPosition(origin.X + 0.5f, origin.Y - 2f, origin.Z);
    }
}
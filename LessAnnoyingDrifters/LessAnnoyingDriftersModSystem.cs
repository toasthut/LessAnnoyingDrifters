using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace LessAnnoyingDrifters;

public class LessAnnoyingDriftersModSystem : ModSystem
{
  public override void StartServerSide(ICoreServerAPI api)
  {
    ApiTaskAdditions.RegisterAiTask(api, "throwatentitylessoften", typeof(AiTaskThrowLessOften));
  }
}

public class AiTaskThrowLessOften : AiTaskThrowAtEntity
{
  long lastSearchTotalMs;
  float maxDist = 15f;
  float throwChance;
  float maxTurnAngleRad;
  float maxOffAngleThrowRad;
  float spawnAngleRad;
  bool immobile;

  public AiTaskThrowLessOften(EntityAgent entity) : base(entity)
  {

  }

  public override void LoadConfig(JsonObject taskConfig, JsonObject aiConfig)
  {
    base.LoadConfig(taskConfig, aiConfig);
    this.throwChance = taskConfig["throwChance"].AsFloat(0);
  }

  public override bool ShouldExecute()
  {
    // React immediately on hurt, otherwise only 1/10 chance of execution
    if (rand.NextDouble() > throwChance && (whenInEmotionState == null || IsInEmotionState(whenInEmotionState) != true)) return false;

    if (!EmotionStatesSatisifed()) return false;
    if (this.lastSearchTotalMs + searchWaitMs > entity.World.ElapsedMilliseconds) return false;
    if (cooldownUntilMs > entity.World.ElapsedMilliseconds) return false;

    float range = maxDist;
    lastSearchTotalMs = entity.World.ElapsedMilliseconds;

    targetEntity = partitionUtil.GetNearestEntity(entity.ServerPos.XYZ, range, (e) => IsTargetableEntity(e, range) && hasDirectContact(e, range, range / 2f) && aimableDirection(e), EnumEntitySearchType.Creatures);

    return targetEntity != null;
  }

  private bool aimableDirection(Entity e)
  {
    if (!immobile) return true;

    float aimYaw = getAimYaw(e);

    return aimYaw > spawnAngleRad - maxTurnAngleRad - maxOffAngleThrowRad && aimYaw < spawnAngleRad + maxTurnAngleRad + maxOffAngleThrowRad;
  }

  private float getAimYaw(Entity targetEntity)
  {
    Vec3f targetVec = new Vec3f();

    targetVec.Set(
        (float)(targetEntity.ServerPos.X - entity.ServerPos.X),
        (float)(targetEntity.ServerPos.Y - entity.ServerPos.Y),
        (float)(targetEntity.ServerPos.Z - entity.ServerPos.Z)
    );

    float desiredYaw = (float)Math.Atan2(targetVec.X, targetVec.Z);

    return desiredYaw;
  }
}

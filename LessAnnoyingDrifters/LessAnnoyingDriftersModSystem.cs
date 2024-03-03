using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace LessAnnoyingDrifters;

public class LessAnnoyingDriftersModSystem : ModSystem
{
  public override void StartServerSide(ICoreServerAPI api)
  {
    ApiTaskAdditions.RegisterAiTask(api, "seekentityquieter", typeof(AiTaskSeekEntityQuieter));
    ApiTaskAdditions.RegisterAiTask(api, "idlequieter", typeof(AiTaskIdleQuieter));
    ApiTaskAdditions.RegisterAiTask(api, "throwatentityquieter", typeof(AiTaskThrowQuieter));
  }
}

public class AiTaskSeekEntityQuieter : AiTaskSeekEntity
{
  public AiTaskSeekEntityQuieter(EntityAgent entity) : base(entity)
  {
    soundChance = 0;
  }
}

public class AiTaskIdleQuieter : AiTaskIdle
{
  public AiTaskIdleQuieter(EntityAgent entity) : base(entity)
  {
    soundChance = 0;
  }
}

public class AiTaskThrowQuieter : AiTaskThrowAtEntity
{
  public AiTaskThrowQuieter(EntityAgent entity) : base(entity)
  {
    soundChance = 0;
  }
}

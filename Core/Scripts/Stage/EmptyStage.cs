namespace Roguelike.Core
{
    public class EmptyStage : IStage
    {
        public EmptyStage(StageKind kind) : base(kind)
        {
        }

        public override StageType Type => StageType.None;

        protected override void OnInitialize()
        {

        }

        protected override void OnRelease()
        {

        }

        protected override void OnUpdate()
        {

        }
    }
}

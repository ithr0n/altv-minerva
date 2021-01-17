using AltV.Net.Elements.Entities;

namespace Minerva.Server.Core.PlayerJobs.PlayerJobSteps
{
    public class MoveToJobStep
        : BaseJobStep
    {
        public MoveToJobStep(PlayerJob job)
            : base(job)
        {
        }

        public Vehicle RequiredVehicle { get; }
    }
}

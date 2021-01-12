using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minerva.Server.PlayerJobs.Base.PlayerJobSteps
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

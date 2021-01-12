using Minerva.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minerva.Server.PlayerJobs.Base
{
    public class PlayerJobBuilder
    {
        private readonly string _jobName;
        private readonly ServerPlayer _player;
        private readonly Queue<BaseJobStep> _steps;

        public PlayerJobBuilder(string jobName, ServerPlayer player)
        {
            _jobName = jobName;
            _player = player;

            _steps = new Queue<BaseJobStep>();
        }

        public PlayerJobBuilder AddStep(BaseJobStep step)
        {
            _steps.Enqueue(step);

            return this;
        }

        public PlayerJob Build()
        {
            var job = new PlayerJob(_jobName, _player, _steps);

            return job;
        }
    }
}

using Minerva.Server.Entities;
using System;
using System.Collections.Generic;

namespace Minerva.Server.PlayerJobs.Base
{
    public class PlayerJob
    {
        public PlayerJob(string name, ServerPlayer player, Queue<BaseJobStep> steps)
        {
            Name = name;
            Player = player;
            JobSteps = steps;
        }

        public string Name { get; }
        public bool Success { get; private set; }
        public DateTime? ActivatedAt { get; private set; }
        public bool Active => ActivatedAt.HasValue;

        public Queue<BaseJobStep> JobSteps { get; }
        public ServerPlayer Player { get; }

        public bool Start()
        {
            if (Active)
            {
                return false;
            }

            if (Player.CurrentJob != null)
            {
                return false;
            }

            ActivatedAt = DateTime.Now;

            while (JobSteps.Count > 0)
            {
                var currentStep = JobSteps.Dequeue();

                if (!currentStep.Execute())
                {
                }
            }

            return true;
        }
    }
}
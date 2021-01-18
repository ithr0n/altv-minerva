using AltV.Net.Interactions;
using Minerva.Server.Core.Contracts.Abstractions;
using Minerva.Server.Data.Models;
using Minerva.Server.Interactions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Minerva.Server.ServerJobs
{
    public class UsableWorldAtms
        : IServerJob
    {
        private readonly List<Interaction> _interactions;

        public UsableWorldAtms()
        {
            _interactions = new List<Interaction>();
        }

        public async Task OnSave()
        {
            await Task.CompletedTask;
        }

        public async Task OnShutdown()
        {
            foreach (var interaction in _interactions)
            {
                AltInteractions.RemoveInteraction(interaction);
            }

            _interactions.Clear();

            await Task.CompletedTask;
        }

        public async Task OnStartup()
        {
            var data = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "dumps", "objectslocations", "worldAtms.json"));

            var allAtms = JsonConvert.DeserializeObject<List<WorldAtmObject>>(data);

            foreach (var atm in allAtms)
            {
                var interaction = new AtmInteraction((ulong)_interactions.Count, atm.Position, 0, 2);
                _interactions.Add(interaction);

                AltInteractions.AddInteraction(interaction);
            }

            await Task.CompletedTask;
        }
    }
}

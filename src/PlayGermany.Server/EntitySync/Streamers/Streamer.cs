using AltV.Net.EntitySync;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Numerics;

namespace PlayGermany.Server.EntitySync.Streamers
{
    public class Streamer<T> where T : Entity
    {
        private ILogger<Streamer<T>> Logger { get; }

        public List<T> ManagedEntities { get; }

        public Streamer(ILogger<Streamer<T>> logger)
        {
            Logger = logger;
            ManagedEntities = new List<T>();
        }

        public ulong Create(T entity)
        {
            ManagedEntities.Add(entity);
            AltEntitySync.AddEntity(entity);

            Logger.LogDebug($"[EntitySync] Created entity with id {entity.Id}");

            return entity.Id;
        }

        public bool Delete(T entity)
        {
            if (entity == null || entity.Id == 0 || !ManagedEntities.Contains(entity))
            {
                return false;
            }

            ManagedEntities.Remove(entity);
            AltEntitySync.RemoveEntity(entity);

            Logger.LogDebug($"[EntitySync] Deleted entity with id {entity.Id}");

            return true;
        }

        public void DeleteAll()
        {
            foreach (var entity in ManagedEntities)
            {
                AltEntitySync.RemoveEntity(entity);
            }

            ManagedEntities.Clear();

            Logger.LogDebug($"[EntitySync] Deleted all entities");
        }

        public bool TryGetClosestEntity(Vector3 position, out T entity, out float distance)
        {
            entity = null;
            distance = float.MaxValue;

            if (ManagedEntities.Count == 0)
            {
                return false;
            }

            foreach (var element in ManagedEntities)
            {
                float dist = Vector3.Distance(element.Position, position);

                if (dist < distance)
                {
                    entity = element;
                    distance = dist;
                }
            }

            return true;
        }
    }
}

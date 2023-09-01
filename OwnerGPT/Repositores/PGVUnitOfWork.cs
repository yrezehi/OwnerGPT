using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using OwnerGPT.Utilities;
using OwnerGPT.Utilities.Extenstions;
using Pgvector;
using Pgvector.Npgsql;

namespace OwnerGPT.Repositories
{
    public class PGVUnitOfWork
    {
        private readonly NpgsqlConnection Connection;
        private int DEFAULT_NEAREST_NEIGHBORS = 5;

        public PGVUnitOfWork() {
            NpgsqlDataSourceBuilder builder = new NpgsqlDataSourceBuilder(ConfigurationUtil.GetValue<string>("PG_V_CONNECTION_STRING"));
            
            builder.UseVector();

            Connection = builder.Build().OpenConnection();
        }

        public async Task<IEnumerable<T>> NearestVectorNeighbor<T>(Vector vector)
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.NearestVectorNeighborsQuery<T>(DEFAULT_NEAREST_NEIGHBORS), Connection))
            {
                IList<T> neighbors = new List<T>();
                command.Parameters.AddWithValue(vector);

                await using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                       neighbors.Add(reader.MapToObject<T>());
                    }

                    return neighbors;
                }
            }
        }
        
        public async Task<Vector> InsertVector<T>(Vector vector, string context)
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.InsertVectorQuery<T>(), Connection))
            {
                command.Parameters.AddWithValue(vector);
                command.Parameters.AddWithValue(context);

                await command.ExecuteNonQueryAsync();

                return vector;
            }
        }

        public async Task<IEnumerable<T>> All<T>()
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.VectorsQuery<T>(), Connection))
            {
                IList<T> vectors = new List<T>();

                await using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        vectors.Add(reader.MapToObject<T>());
                    }
                    return vectors;
                }
            }
        }

        public async Task CreateTable<T>()
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.InsertVectorQuery<T>(), Connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

    }
}

using Npgsql;
using OwnerGPT.Databases.Repositores.Extensions;
using OwnerGPT.Databases.Repositores.PGVDB.Interfaces;
using Pgvector;
using Pgvector.Npgsql;

namespace OwnerGPT.Databases.Repositores.PGVDB
{
    public class PGVUnitOfWork : IPGVUnitOfWork
    {
        private readonly NpgsqlConnection Connection;
        private int DEFAULT_NEAREST_NEIGHBORS = 5;

        public PGVUnitOfWork()
        {
            NpgsqlDataSourceBuilder builder = new NpgsqlDataSourceBuilder("Host=localhost;Database=ownergpt;User ID=owner;Password=owner;");

            builder.UseVector();

            Connection = builder.Build().OpenConnection();
        }

        public async Task<IEnumerable<T>> NearestVectorNeighbor<T>(float[] vector)
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

        public async Task<Vector> InsertVector<T>(float[] vector, string context)
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.InsertVectorQuery<T>(), Connection))
            {
                command.Parameters.AddWithValue(new Vector(vector));
                command.Parameters.AddWithValue(context);

                await command.ExecuteNonQueryAsync();

                return new Vector(vector);
            }
        }

        public async Task<int> DeleteVector<T>(int id)
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.DeleteVectorQuery<T>(), Connection))
            {
                command.Parameters.AddWithValue(id);

                await command.ExecuteNonQueryAsync();

                return id;
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

﻿using Microsoft.Extensions.Configuration;
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

        public PGVUnitOfWork() {
            NpgsqlDataSourceBuilder builder = new NpgsqlDataSourceBuilder(ConfigurationUtil.GetValue<string>("PG_V_CONNECTION_STRING"));
            
            builder.UseVector();

            Connection = builder.Build().OpenConnection();
        }

        public async Task<T> NearestVectorNeighbor<T>(Vector vector)
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.NearestVectorNeighborsQuery<T>(5), Connection))
            {
                command.Parameters.AddWithValue(vector);

                await using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                       return reader.MapToObject<T>();
                    }
                }
            }
            return default(T);
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

        public async Task CreateTable<T>()
        {
            await using (var command = new NpgsqlCommand(PGVQueryExtension.InsertVectorQuery<T>(), Connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

    }
}

using OwnerGPT.Models.Interfaces;

namespace OwnerGPT.Utilities.Extenstions
{
    public static class PGVQueryExtension
    {

        // This query assumes all vectors has properties have column named embedding as embedding vector
        public static string InsertVectorQuery<T>() =>
            $"INSERT INTO {EntityToTableName<T>()} (embedding) VALUES($1)";

        public static string NearestVectorNeighborsQuery<T>(int limit) =>
            $"SELECT * FROM {EntityToTableName<T>()} ORDER BY embedding <-> $1 LIMIT {limit}";
        

        // converts entity name to snake case
        public static string EntityToTableName<T>() =>
            string.Concat(typeof(T).Name.Select((character, index) => index > 0 && char.IsUpper(character) ? "_" + character.ToString() : character.ToString())).ToLower();
        
    }
}

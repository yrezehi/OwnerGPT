using Hisuh.Utilities;
using Npgsql;

namespace OwnerGPT.Utilities.Extenstions
{
    public static class PGVQueryExtension
    {

        // This query assumes all vectors has properties have column named embedding as embedding vector
        public static string InsertVectorQuery<T>() =>
            $"INSERT INTO {EntityToTableName<T>()} (embedding) VALUES($1)";

        public static string NearestVectorNeighborsQuery<T>(int limit) =>
            $"SELECT * FROM {EntityToTableName<T>()} ORDER BY embedding <-> $1 LIMIT {limit}";

        public static string CreateVectorTableQuery<T>(int limit) =>
            $"CREATE TABLE {EntityToTableName<T>()} (embedding vector(3), context varchar(n))";

        // converts data reader result into object
        public static T MapToObject<T>(this NpgsqlDataReader reader)
        {
            T entity = (T)Activator.CreateInstance(typeof(T))!;

            for (int index = 0; index < reader.FieldCount; index++)
            {
                var propertyName = reader.GetName(index);

                if (ReflectionUtil.ContainsProperty(entity, propertyName))
                {
                    ReflectionUtil.SetValueOf(entity, propertyName, reader.GetValue(index));
                }
            }
            return entity;
        }

       // converts snake case to camel case
       public static string TablePropertyToOjbectProperty(this string propertyName) =>
            propertyName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);

        // converts entity name to snake case
        public static string EntityToTableName<T>() =>
            string.Concat(typeof(T).Name.Select((character, index) => index > 0 && char.IsUpper(character) ? "_" + character.ToString() : character.ToString())).ToLower();
        
    }
}

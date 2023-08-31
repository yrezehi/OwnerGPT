using Npgsql;
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

        public static string CreateVectorTableQuery<T>(int limit) =>
            $"CREATE TABLE {EntityToTableName<T>()} (embedding vector(3), context varchar(n))";

        // converts data reader result into object
        public static T ReaderToObject<T>(this NpgsqlDataReader reader)
        {
            T entity = (T)Activator.CreateInstance(typeof(T))!;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                //var name = propertiesHashSet.FirstOrDefault(a => a.Equals(dataReader.GetName(i), StringComparison.InvariantCultureIgnoreCase));
                if (!String.IsNullOrEmpty(name))
                {
                    //dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);
                }
            }
        }

       // converts snake case to camel case
       public static string TablePropertyToOjbectProperty(this string propertyName)
       {
            return propertyName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries).Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
       }

        // converts entity name to snake case
        public static string EntityToTableName<T>() =>
            string.Concat(typeof(T).Name.Select((character, index) => index > 0 && char.IsUpper(character) ? "_" + character.ToString() : character.ToString())).ToLower();
        
    }
}

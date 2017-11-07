using ESGroupUpdator.Models;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace ESGroupUpdator
{
    public static class ConnectionString
    {
        public static string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
    }

    public static class SqlText
    {
        public static string Group_Select = "SELECT [Group_Id] AS Id ,[Nm] FROM[test].[dbo].[Group];";
    }

    public class Extractor
    {
        public ElasticSearchIndex GetGroups()
        {
            using (var db = new SqlConnection(ConnectionString.Connection))
            {
                var documents = db.Query<Group>(SqlText.Group_Select).ToArray();

                return new ElasticSearchIndex
                {
                    Documents = documents,
                    Name = "liberty-test-group"
                };
            }
        }
    }
}

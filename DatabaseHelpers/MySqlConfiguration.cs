using System.Data.Entity;
using MySqlHistoryContext = BombVacuum.DatabaseHelpers.MySqlHistoryContext;

namespace BombVacuum.DatabaseHelpers
{
    public class MySqlConfiguration : DbConfiguration
    {
        public MySqlConfiguration()
        {
            SetHistoryContext(
            "MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
        }
    }
}
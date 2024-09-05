using Microsoft.Data.Sqlite;

namespace Zone_OFI_ISO8583_Processor.Models
{
    public static class SQLite
    {
        public static void CreateTable(SqliteConnection connection)
        {
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS KeyVault (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ZMK TEXT NOT NULL,
                                    ZMK_Date DATE NOT NULL,
                                    ZPK TEXT NOT NULL,
                                    ZPK_Date DATE NOT NULL)";
            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void InsertOrUpdateKeyVault(SqliteConnection connection, KeyVault keyVault)
        {
            // Check if the table is empty
            string checkQuery = "SELECT COUNT(*) FROM KeyVault;";
            bool isTableEmpty;

            using (var checkCommand = new SqliteCommand(checkQuery, connection))
            {
                isTableEmpty = Convert.ToInt32(checkCommand.ExecuteScalar()) == 0;
            }

            if (isTableEmpty)
            {
                // If table is empty, insert a new record
                string insertQuery = @"
                INSERT INTO KeyVault (ZMK, ZMK_Date, ZPK, ZPK_Date)
                VALUES (@ZMK, @ZMK_Date, @ZPK, @ZPK_Date);";

                using (var insertCommand = new SqliteCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ZMK", keyVault.ZMK);
                    insertCommand.Parameters.AddWithValue("@ZMK_Date", keyVault.ZMK_Date);
                    insertCommand.Parameters.AddWithValue("@ZPK", keyVault.ZPK);
                    insertCommand.Parameters.AddWithValue("@ZPK_Date", keyVault.ZPK_Date);
                    insertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                // If table is not empty, update the existing record
                string updateQuery = @"
                UPDATE KeyVault 
                SET ZMK = @ZMK, ZMK_Date = @ZMK_Date, ZPK = @ZPK, ZPK_Date = @ZPK_Date;";

                using (var updateCommand = new SqliteCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@ZMK", keyVault.ZMK);
                    updateCommand.Parameters.AddWithValue("@ZMK_Date", keyVault.ZMK_Date);
                    updateCommand.Parameters.AddWithValue("@ZPK", keyVault.ZPK);
                    updateCommand.Parameters.AddWithValue("@ZPK_Date", keyVault.ZPK_Date);
                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateZMK(SqliteConnection connection, KeyVault keyVault)
        {
            string updateQuery = @"UPDATE KeyVault
                               SET ZMK = @zmk, ZMK_Date = @zmkDate
                                Where Id=1";
            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@zmk", keyVault.ZMK);
                command.Parameters.AddWithValue("@zmkDate", keyVault.ZMK_Date.ToString("yyyy-MM-dd"));
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateZPK(SqliteConnection connection, KeyVault keyVault)
        {
            string updateQuery = @"UPDATE KeyVault
                               SET ZPK = @zpk, ZPK_Date = @zpkDate
                               Where Id=1";
            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@zpk", keyVault.ZPK);
                command.Parameters.AddWithValue("@zpkDate", keyVault.ZPK_Date.ToString("yyyy-MM-dd"));
                command.ExecuteNonQuery();
            }
        }

        public static KeyVault FetchKeyVaultData(SqliteConnection connection)
        {
            string selectQuery = "SELECT * FROM KeyVault Where Id=1";
            KeyVault keyVault = null;

            using (var command = new SqliteCommand(selectQuery, connection))
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    keyVault = new KeyVault
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ZMK = reader["ZMK"].ToString(),
                        ZMK_Date = Convert.ToDateTime(reader["ZMK_Date"]),
                        ZPK = reader["ZPK"].ToString(),
                        ZPK_Date = Convert.ToDateTime(reader["ZPK_Date"])
                    };
                }
            }

            return keyVault;
        }
    }

    public class KeyVault
    {
        public int Id { get; set; }
        public string ZMK { get; set; }
        public DateTime ZMK_Date { get; set; }
        public string ZPK { get; set; }
        public DateTime ZPK_Date { get; set; }
    }
}

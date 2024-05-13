using MySqlConnector;

namespace Server;

public struct Entity
{
    public int Id;
    public string Name;
    public int Age;

    public override string ToString() => $"Id: {Id}, Name: {Name}, Age: {Age}";
}

public class DataBase
{
    private readonly string _connectionString =
        "server=152.136.50.204;user=root;password=123321;database=test";

    public DataBase() { }

    public async Task InsertAsync(string name, int age)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO data (name, age) VALUES (@name, @age)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                await command.ExecuteNonQueryAsync();
            }
            await connection.CloseAsync();
        }
    }

    public async Task<List<Entity>> GetAllAsync()
    {
        var result = new List<Entity>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM data";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(
                            new Entity
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Age = reader.GetInt32(2)
                            }
                        );
                    }
                }
            }
            await connection.CloseAsync();
        }
        return result;
    }

    public async Task UpdateAsync(int id, string newName)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE data SET name = @newName WHERE id = @id";
                command.Parameters.AddWithValue("@newName", newName);
                command.Parameters.AddWithValue("@id", id);
                await command.ExecuteNonQueryAsync();
            }
            await connection.CloseAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE Data WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                await command.ExecuteNonQueryAsync();
            }
            await connection.CloseAsync();
        }
    }
}

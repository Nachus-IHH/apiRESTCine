using MySqlConnector;
using Microsoft.Extensions.Configuration;

namespace apiRESTCine.WebApp.Models;

public class Conexion
{
    private readonly string _connectionString;

    public Conexion(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionExa2") 
            ?? throw new InvalidOperationException("La cadena de conexión 'ControlAccesoConnection' no está configurada.");
    }

    public MySqlConnection ObtenerConexion()
    {
        return new MySqlConnection(_connectionString);
    }
}


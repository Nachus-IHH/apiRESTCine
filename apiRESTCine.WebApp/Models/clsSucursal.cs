using System.Data;
using MySqlConnector;

namespace apiRESTCine.WebApp.Models;

public class clsSucursal
{
    public string? clave { get; set; }
    public string? nombre { get; set; }
    public string? direccion { get; set; }
    public string? url { get; set; }
    public string? logo { get; set; }

    public clsSucursal()
    {
    }

    public clsSucursal(string clave, string nombre, string direccion, string url, string logo)
    {
        this.clave = clave;
        this.nombre = nombre;
        this.direccion = direccion;
        this.url = url;
        this.logo = logo;
    }

    /* Inserción de Sucursal usando Stored Procedure (spInsSucursales) */
    public static int spInsSucursales(Conexion conexion, clsSucursal sucursal)
    {
        using var cnn = conexion.ObtenerConexion();
        using var cmd = new MySqlCommand("spInsSucursales", cnn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Parámetros de entrada (IN)
        cmd.Parameters.AddWithValue("p_nombre", sucursal.nombre);
        cmd.Parameters.AddWithValue("p_direccion", sucursal.direccion);
        cmd.Parameters.AddWithValue("p_homeweb", sucursal.url);
        cmd.Parameters.AddWithValue("p_logo", sucursal.logo);

        // Parámetro de salida (OUT) requerido por MySQL
        var paramBandera = new MySqlParameter("p_bandera", MySqlDbType.Int32)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(paramBandera);

        cnn.Open();
        cmd.ExecuteNonQuery();

        // Recuperar el valor de la bandera de estatus retornada por el SP
        var resultado = paramBandera.Value;
        return Convert.ToInt32(resultado ?? 0);
    }

    /* Obtener reporte de sucursales consultando la vista vwRptsucursales*/
    public static List<clsSucursal> vwRptsucursales(Conexion conexion)
    {
        var lista = new List<clsSucursal>();
        using var cnn = conexion.ObtenerConexion();

        string query = "SELECT * FROM vwRptSucursales";
        using var cmd = new MySqlCommand(query, cnn);

        cnn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new clsSucursal
            {
                clave = reader["SUC_CVE_SUCURSAL"]?.ToString(),
                nombre = reader["SUC_NOMBRE"]?.ToString(),
                direccion = reader["SUC_DIRECCION"]?.ToString(),
                url = reader["SUC_HOMEWEB"]?.ToString(),
                logo = reader["SUC_LOGO"]?.ToString()
            });
        }

        return lista;
    }
}

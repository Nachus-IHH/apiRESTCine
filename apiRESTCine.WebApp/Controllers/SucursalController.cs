using Microsoft.AspNetCore.Mvc;
using apiRESTCine.WebApp.Models;

namespace apiRESTCine.WebApp.Controllers;

[ApiController]
[Route("cine/[controller]")]
public class SucursalController : ControllerBase
{
    private readonly Conexion _conexion;

    // CORREGIDO: El nombre del constructor debe coincidir con la clase (SucursalController)
    public SucursalController(Conexion conexion)
    {
        _conexion = conexion;
    }

    /* POST: cine/sucursal/spinssucursales */
    [HttpPost("spinssucursales")]
    public IActionResult SpInsSucursal([FromBody] clsSucursal modelo)
    {
        if (modelo == null)
        {
            return BadRequest(new { mensaje = "El modelo de sucursal no puede ser nulo." });
        }

        try
        {
            int resultado = clsSucursal.spInsSucursales(_conexion, modelo);

            return Ok(new
            {
                mensaje = "Sucursal registrada exitosamente!",
                bandera = resultado
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Sucursal NO registrada...",
                error = ex.Message
            });
        }
    }

    /* GET: cine/sucursal/vwrptsucursales */
    [HttpGet("vwrptsucursales")]
    public IActionResult vwRptsucursales()
    {
        try
        {
            var sucursales = clsSucursal.vwRptsucursales(_conexion);
            // CORREGIDO: Se retorna 'sucursales' en lugar de 'usuarios'
            return Ok(sucursales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al consultar el reporte", detalle = ex.Message });
        }
    }
}

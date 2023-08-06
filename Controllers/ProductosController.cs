using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using PruebasResidenciasHugo.Models;


namespace PruebasResidenciasHugo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly AspitantesApiContext _dbcontext;

        public ProductoController(AspitantesApiContext _context)
        {
            _dbcontext = _context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<ProductosHugo> lista = new List<ProductosHugo>();
            try
            {
                lista = _dbcontext.ProductosHugos.ToList();

                return StatusCode(StatusCodes.Status200OK, new { lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { ex.Message, lista });
            }
        }

        [HttpPost]
        [Route("Crear")]
        public IActionResult Crear([FromBody] ProductosHugo nuevoProducto)
        {
            try
            {
                if (nuevoProducto != null)
                {
                    if (_dbcontext.ProductosHugos.Any(p => p.Nombre == nuevoProducto.Nombre))
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new { Mensaje = "Ya existe un producto con este nombre" });
                    }

                    if (nuevoProducto.Precio < 0 || nuevoProducto.Stock < 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { Mensaje = "El precio y el stock deben ser valores positivos" });
                    }

                    _dbcontext.ProductosHugos.Add(nuevoProducto);
                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status201Created, new { Mensaje = "Producto creado exitosamente", Producto = nuevoProducto });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { Mensaje = "Datos de producto no válidos" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensaje = "Error al crear el producto", Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("ObtenerProducto/{id}")]
        public IActionResult ObtenerProducto(int id)
        {
            try
            {
                var producto = _dbcontext.ProductosHugos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { Mensaje = "Producto no encontrado" });
                }

                return StatusCode(StatusCodes.Status200OK, producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensaje = "Error al obtener el producto", Error = ex.Message });
            }
        }

        [HttpPut]
        [Route("ActualizarProducto/{id}")]
        public IActionResult ActualizarProducto(int id, [FromBody] ProductosHugo productoActualizado)
        {
            try
            {
                var producto = _dbcontext.ProductosHugos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { Mensaje = "Producto no encontrado" });
                }

                if (productoActualizado != null)
                {
                    if (productoActualizado.Precio < 0 || productoActualizado.Stock < 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { Mensaje = "El precio y el stock deben ser valores no negativos" });
                    }

                    if (_dbcontext.ProductosHugos.Any(p => p.Nombre == productoActualizado.Nombre && p.Id != id))
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new { Mensaje = "Ya existe un producto con este nombre" });
                    }

                    if (!string.IsNullOrWhiteSpace(productoActualizado.Nombre))
                    {
                        producto.Nombre = productoActualizado.Nombre;
                    }

                    if (productoActualizado.Descripcion != null)
                    {
                        producto.Descripcion = productoActualizado.Descripcion;
                    }

                    if (productoActualizado.Precio > 0)
                    {
                        producto.Precio = productoActualizado.Precio;
                    }

                    if (productoActualizado.Stock >= 0)
                    {
                        producto.Stock = productoActualizado.Stock;
                    }

                    _dbcontext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { Mensaje = "Producto actualizado exitosamente", Producto = producto });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { Mensaje = "Datos de producto no válidos" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensaje = "Error al actualizar el producto", Error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("EliminarProducto/{id}")]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                var producto = _dbcontext.ProductosHugos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { Mensaje = "Producto no encontrado" });
                }

                _dbcontext.ProductosHugos.Remove(producto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { Mensaje = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensaje = "Error al eliminar el producto", Error = ex.Message });
            }
        }

    }
}

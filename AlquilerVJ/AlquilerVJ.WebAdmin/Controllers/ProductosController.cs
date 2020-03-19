﻿using AlquilerVJ.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlquilerVJ.WebAdmin.Controllers
{
    public class ProductosController : Controller
    {
        ProductosBL _ProductosBL;
        CategoriasBL _CategoriasBL;

        public ProductosController()
        {
            _ProductosBL = new ProductosBL();
            _CategoriasBL = new CategoriasBL();
        }

        // GET: Productos
        public ActionResult Index()
        {
            var ListadeProductos = _ProductosBL.ObtenerProductos();

            return View(ListadeProductos);
        }

        public ActionResult Crear()
        {
            var NuevoProducto = new Producto();
            var categorias = _CategoriasBL.ObtenerCategorias();

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Descripcion");

            return View(NuevoProducto);
            
        }


        [HttpPost]
        public ActionResult Crear(Producto producto, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid)
            {
                if (producto.CategoriaId == 0)
                {
                    ModelState.AddModelError("CategoriaId", "Seleccione una categoria");//el generador de lista de categorias
                    return View(producto);
                }
                if (imagen != null)  //si el valor de la imagen no esta nulo,  guarda la imagen 
                {
                    producto.UrlImagen = GuardarImagen(imagen);//----
                }
                _ProductosBL.GuardarProducto(producto);
                return RedirectToAction("Index");
            }
                var categorias = _CategoriasBL.ObtenerCategorias();
                ViewBag.CategoriaId = new SelectList(categorias, "Id", "Descripcion");

                return View(producto);
        }


        private string GuardarImagen(HttpPostedFileBase imagen)
        {
            string path = Server.MapPath("~/Imagenes/" + imagen.FileName);//el encargago de guardar la imagen en la carpeta "Imagenes"
            imagen.SaveAs(path);

            return "/Imagenes/" + imagen.FileName;
        }



        public ActionResult Editar(int id)
        {
            var productos = _ProductosBL.ObtenerProducto(id);
            var categorias = _CategoriasBL.ObtenerCategorias();

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Descripcion", productos.CategoriaId);

            return View(productos);
        }



        [HttpPost]
        public ActionResult Editar(Producto productos, HttpPostedFileBase imagen)
        {
            if (ModelState.IsValid) 
            {
                if (productos.CategoriaId == 0)
                {
                    ModelState.AddModelError("CategoriaId", "Seleccione una categoria");
                    return View(productos);
                }

                if (imagen != null)//si el valor de imagen es nula, no la edita
                {
                    productos.UrlImagen = GuardarImagen(imagen);
                }

                _ProductosBL.GuardarProducto(productos);
                return RedirectToAction("Index");
            }
            var categorias = _CategoriasBL.ObtenerCategorias();
            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Descripcion");

            return View(productos);
        }

        public ActionResult Detalles(int id)
        {
            var producto = _ProductosBL.ObtenerProducto(id);
            return View(producto);
        }


        public ActionResult Eliminar(int id)
        {
            var productos = _ProductosBL.ObtenerProducto(id);

            return View(productos);
        }

        [HttpPost]
        public ActionResult Eliminar(Producto productos)
        {
            _ProductosBL.EliminarProducto(productos.Id);
            return RedirectToAction("Index");
        }
    }
}
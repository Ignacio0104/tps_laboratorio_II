﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaDeClases;
using System.Media;

namespace Blockbuster_UI
{
    public partial class AgregarAlquiler : Form
    {
        public Socio socioAtendido;
        public List<Alquiler<Pelicula>> listaAlquilerAux;
        public List<Producto> listaProductosAux;
        public double acumuladorPrecio;
        public bool Editar = false;

        public AgregarAlquiler(Socio socioAtendido)
        {
            InitializeComponent();
            listaAlquilerAux = new List<Alquiler<Pelicula>>();
            listaProductosAux = new List<Producto>();
            this.socioAtendido = socioAtendido;
            dGridPeliculas.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#003566");
            dGridProducto.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#003566");
            dGridProducto.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#ffc300");
            dGridPeliculas.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#ffc300");
        }

        private void CargarPeliculas()
        {
            foreach (Pelicula item in Blockbuster.ListaDePeliculas)
            {
                
                if (item.Stock > 0)
                {
                    int indice = dGridPeliculas.Rows.Add();
                    dGridPeliculas.Rows[indice].Cells[0].Value = item.IdPelicula;
                    dGridPeliculas.Rows[indice].Cells[1].Value = item.TituloPelicula;
                    dGridPeliculas.Rows[indice].Cells[2].Value = item.DuracionPelicula + " min";
                    dGridPeliculas.Rows[indice].Cells[3].Value = item.GeneroPelicula.ToString();
                    dGridPeliculas.Rows[indice].Cells[4].Value = item.Stock.ToString();
                    dGridPeliculas.Rows[indice].Cells[5].Value = "$" + (int)item.PrecioDeAlquiler;
                    indice++;
                }

            }
        }

        private void CargarProductos()
        {
            foreach (Producto item in Blockbuster.ListaDeProductos)
            {
               
                if(item.StockProducto > 0)
                {
                    int indice = dGridProducto.Rows.Add();
                    dGridProducto.Rows[indice].Cells[0].Value = item.IdProducto;
                    dGridProducto.Rows[indice].Cells[1].Value = item.NombreProducto;
                    dGridProducto.Rows[indice].Cells[2].Value = "$" + item.PrecioProducto;
                    dGridProducto.Rows[indice].Cells[3].Value = item.StockProducto;
                    indice++;
                }

            }
        }

        private void dGridPeliculas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if ((listaAlquilerAux.Count + socioAtendido.ListaDeAlquileres.Count) < socioAtendido.LimitePeliculas)
                    {
                        DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dGridPeliculas.Rows[e.RowIndex].Cells[0];
                        listaAlquilerAux.Add(new Alquiler<Pelicula>(Blockbuster.BuscarPelicula((int)cell.Value)));
                        Blockbuster.BuscarPelicula((int)cell.Value).Stock--;
                        dGridPeliculas.Rows.Clear();
                        CargarPeliculas();
                        ActualizarCuenta();
                    }
                    else
                    {
                        throw new Exception("Error, este socio no tiene lugar para alquilar más películas");
                    }

                }
   
            }catch(Exception exception)
            {
                MessageBox.Show(exception.Message,"Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{DateTime.Now} - Alquiler/Producto - Total ${acumuladorPrecio} - Socio {socioAtendido.IdSocio}/{socioAtendido.ApellidoSocio }");
            Blockbuster.FacturacionDiaria += sb.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void dGridProducto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {                                
                    DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dGridProducto.Rows[e.RowIndex].Cells[0];
                    listaProductosAux.Add(Blockbuster.BuscarProducto((int)cell.Value));
                    Blockbuster.BuscarProducto((int)cell.Value).StockProducto--;
                    dGridProducto.Rows.Clear();
                    CargarProductos();
                    ActualizarCuenta();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ActualizarCuenta()
        {
            StringBuilder sb = new StringBuilder();
            checkEditar.Items.Clear();
            richAlquileresParcial.Text = "";
            acumuladorPrecio = 0;
            if (listaAlquilerAux.Count > 0)
            {
                foreach (Alquiler<Pelicula> item in listaAlquilerAux)
                {
                    sb.AppendLine($"{item.Pelicula.ToString()}");
                    sb.AppendLine("---------------------------");
                    checkEditar.Items.Add(item);
                    acumuladorPrecio += (int)item.Pelicula.PrecioDeAlquiler;
                }
            }

            if (listaProductosAux.Count > 0)
            {
                foreach (Producto item in listaProductosAux)
                {
                    sb.AppendLine($"{item.ToString()}");
                    sb.AppendLine("---------------------------");
                    checkEditar.Items.Add(item);
                    acumuladorPrecio += item.PrecioProducto;
                }
            }

            richAlquileresParcial.Text = sb.ToString();
            lblFacturacion.Text = $"Facturacion: ${acumuladorPrecio}";
        }

        private void AgregarAlquiler_Load(object sender, EventArgs e)
        {
            CargarPeliculas();
            CargarProductos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar = !Editar;
            if (Editar)
            {
                btnBorrar.Visible = true;
                checkEditar.Visible = true;

            }
            else
            {
                btnBorrar.Visible = false;
                checkEditar.Visible = false;
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            btnBorrar.Visible = false;
            checkEditar.Visible = false;
            foreach (int indexChecked in checkEditar.CheckedIndices)
            {
                if (listaAlquilerAux.Count > 0 || listaProductosAux.Count > 0)
                {
                    try
                    {
                        if (checkEditar.Items[indexChecked] is Alquiler<Pelicula> alquiler)
                        {
                            Blockbuster.BuscarPelicula(alquiler.Pelicula.IdPelicula).Stock++;
                            listaAlquilerAux.Remove(alquiler);
                        }
                        else
                        {
                            if(checkEditar.Items[indexChecked] is Producto producto)
                            {
                                Blockbuster.BuscarProducto(producto.IdProducto).StockProducto++;
                                listaProductosAux.Remove(producto);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            StringBuilder sb = new StringBuilder();
            checkEditar.Items.Clear();//Se refresca el checkboxlist con los items actualizados
            ActualizarCuenta();
            dGridProducto.Rows.Clear();
            CargarProductos();
            dGridPeliculas.Rows.Clear();
            CargarPeliculas();
        }

        private void txtTitulo_TextChanged(object sender, EventArgs e)
        {
            dGridPeliculas.Rows.Clear();
            foreach (Pelicula item in Blockbuster.ListaDePeliculas)
            {
                int indice;
                if (item.TituloPelicula.ToLower().Contains(txtTitulo.Text.ToLower()))
                {
                    indice = dGridPeliculas.Rows.Add();
                    dGridPeliculas.Rows[indice].Cells[0].Value = item.IdPelicula;
                    dGridPeliculas.Rows[indice].Cells[1].Value = item.TituloPelicula;
                    dGridPeliculas.Rows[indice].Cells[2].Value = item.DuracionPelicula + " min";
                    dGridPeliculas.Rows[indice].Cells[3].Value = item.GeneroPelicula.ToString();
                    dGridPeliculas.Rows[indice].Cells[4].Value = item.Stock.ToString();
                    dGridPeliculas.Rows[indice].Cells[5].Value = "$" + (int)item.PrecioDeAlquiler;
                    indice++;
                }             
            }
        }
    }

}

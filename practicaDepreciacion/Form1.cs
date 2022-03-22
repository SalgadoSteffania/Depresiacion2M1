﻿using AppCore.IServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practicaDepreciacion
{
    public partial class Form1 : Form
    {
        IActivoServices activoServices;
        private int idSeleccionado;
        int Posicion;
        public Form1(IActivoServices ActivoServices)
        {
            this.activoServices = ActivoServices;
            InitializeComponent();
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede numeros");
            }
        }

    

        private void txtValor_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtValorR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtVidaU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtEnviar_Click(object sender, EventArgs e)
        {
            bool verificado = verificar();
            if (verificado == false)
            {
                MessageBox.Show("Tienes que llenar todos los formularios.");
            }
            else
            {

                Activo activo = new Activo()
                {
                    Nombre = txtNombre.Text,
                    Valor = double.Parse(txtValor.Text),
                    ValorResidual=double.Parse(txtValorR.Text),
                    VidaUtil= int.Parse(txtVidaU.Text)
                };
                activoServices.Add(activo);
                dataGridView1.DataSource = null;
                limpiar();
                dataGridView1.DataSource = activoServices.Read();

            }
        }
        private bool verificar()
        {
            if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtValor.Text) || String.IsNullOrEmpty(txtVidaU.Text) || String.IsNullOrEmpty(txtValorR.Text))
            {
              
                return false;
            }
            return true;
        }
        private void limpiar()
        {
            this.txtNombre.Text = String.Empty;
            this.txtValor.Text = String.Empty;
            this.txtValorR.Text = String.Empty;
            this.txtVidaU.Text = String.Empty;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idSeleccionado = dataGridView1.CurrentRow.Index;
            txtNombre.Text = dataGridView1[1, idSeleccionado].Value.ToString();
            txtValor.Text = dataGridView1[2, idSeleccionado].Value.ToString();
            txtVidaU.Text = dataGridView1[3, idSeleccionado].Value.ToString();
            txtValorR.Text = dataGridView1[4, idSeleccionado].Value.ToString();

            txtNombre.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = activoServices.Read();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idSeleccionado = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            MessageBox.Show(idSeleccionado.ToString());
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FrmDepreciacion depreciacion = new FrmDepreciacion(activoServices.Read()[e.RowIndex]);
                depreciacion.ShowDialog();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string producto, valor, valorR, vida;
            producto = txtNombre.Text;
            valor = txtValor.Text;
            valorR = txtValorR.Text;
            vida = txtVidaU.Text;

            dataGridView1[1, idSeleccionado].Value = txtNombre.Text;
            dataGridView1[2, idSeleccionado].Value = txtValor.Text;
            dataGridView1[3, idSeleccionado].Value = txtVidaU.Text;
            dataGridView1[4, idSeleccionado].Value = txtValorR.Text;

            limpiar();

            txtNombre.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (idSeleccionado!=0)
            {
                //se puede cambiar a que solo reciba el id
                Activo activo = activoServices.GetById(idSeleccionado);
                if (activoServices.Delete(activo))
                {
                    MessageBox.Show($"El elemento con ID {idSeleccionado} fue eliminado correctamente!");
                    dataGridView1.DataSource = null;
                   
                    dataGridView1.DataSource = activoServices.Read();
                }
                else
                {
                    MessageBox.Show("El elemento no fue eliminado correctamente");
                } 
            }
            else
            {
                MessageBox.Show("No se ha seleccionado nada");
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

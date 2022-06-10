﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases
{
    public class SocioClasico : Socio
    {
        private int penalidad;
        private int limitePeliculas;
        private string tarjetaDeCredito;

        public SocioClasico()
        {

        }
        public SocioClasico(string nombre, string apellido, string email, string telefono,string tarjetaDeCredito) : base(nombre, apellido, email, telefono)
        {
            this.tarjetaDeCredito = tarjetaDeCredito;
            this.penalidad = 100;
            this.limitePeliculas = 5;
        }


        public override int Penalidad { get => penalidad; set => penalidad = value; }
        public override int LimitePeliculas { get => limitePeliculas; set => limitePeliculas = value; }
        public string TarjetaDeCredito { get => tarjetaDeCredito; set => tarjetaDeCredito = value; }

        public override string Mostrar()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(base.Mostrar());
            sb.AppendLine($"Penalidad: {penalidad}%");
            sb.AppendLine($"Limite peliculas: {limitePeliculas}");
            sb.AppendLine($"Tarjeta: {tarjetaDeCredito}");

            return sb.ToString();
        }
    }
}

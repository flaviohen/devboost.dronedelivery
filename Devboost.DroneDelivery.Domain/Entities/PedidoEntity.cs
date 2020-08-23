using System;
using Devboost.DroneDelivery.Domain.Enums;
using Microsoft.SqlServer.Types;

namespace Devboost.DroneDelivery.Domain.Entities
{
    public class PedidoEntity
    {
        public Guid Id { get; set; }
        public int PesoGramas {get; set;}
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public SqlGeography LatLong
        {
            get
            {
                SqlGeography point = SqlGeography.Point(this.Latitude, this.Longitude, 4326);
                return point;
            }
        }
        public DateTime? DataHora { get; set; }
        public PedidoStatus Status { get; set; }

		public string DescricaoStatus { 
            get {
                string descricao = "";
                if (Status == PedidoStatus.EmTransito)
                    descricao = PedidoStatus.EmTransito.ToString();
                if (Status == PedidoStatus.Entregue)
                    descricao = PedidoStatus.Entregue.ToString();
                if (Status == PedidoStatus.PendenteEntrega)
                    descricao = PedidoStatus.PendenteEntrega.ToString();
                return descricao;
            } 
        }
		public DroneEntity Drone { get; set; }
        public int? DroneId { get; set; }
        public readonly double DistanciaMaxima = 17;
        public readonly int PesoGamasMaximo = 12000;

        public bool ValidaPedido(double distanciaKm)
        {
            return distanciaKm <= DistanciaMaxima && PesoGramas <= PesoGamasMaximo;
        }
    }
}
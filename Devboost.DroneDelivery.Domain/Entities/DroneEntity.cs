using System;
using Devboost.DroneDelivery.Domain.Enums;

namespace Devboost.DroneDelivery.Domain.Entities
{
    public class DroneEntity
    {
        public int Id { get; set; }
        public int CapacidadeGamas { get; set; }
        public int VelocidadeKmH { get; set; }
        public int AutonomiaMinitos { get; set; }
        public int CargaGramas { get; set; }
        public DroneStatus Status { get; set; }

		public string DescricaoStatus {
            get 
            {
                string status = "";
                if (this.Status == DroneStatus.Carregando)
                    status = DroneStatus.Carregando.ToString();
                if (this.Status == DroneStatus.EmTransito)
                    status = DroneStatus.EmTransito.ToString();
                if (this.Status == DroneStatus.Pronto)
                    status = DroneStatus.Pronto.ToString();
                return status;
            } 
        }
		public DateTime? DataAtualizacao { get; set; }

        public readonly double AUTONOMIA_MAXIMA = 35;
        public readonly double TEMPO_RECARGA_MINUTOS = 60;
        public readonly double AUTONOMIA_RECARGA = 95;
        

    }
}
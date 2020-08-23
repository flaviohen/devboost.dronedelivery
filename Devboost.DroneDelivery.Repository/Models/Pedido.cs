using Dapper.Contrib.Extensions;
using System;
using Microsoft.SqlServer.Types;


namespace Devboost.DroneDelivery.Repository.Models
{
    [Table("dbo.Pedido")]
    public class Pedido
    {        

        [ExplicitKey]
        public Guid Id { get; set; }
        public int Peso {get; set;}
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
        public string Status { get; set; }
        public DateTime? DataHora { get; set; }
        public int DroneId { get; set; }
    }
}
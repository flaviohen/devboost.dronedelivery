using GeoCoordinatePortable;
using System;

namespace Devboost.DroneDelivery.DomainService
{
    public class GeolocalizacaoService
    {
	    public static readonly double LATITUDE_INICIAL = -23.5880684;
	    public static readonly double LONGITUDE_INICIAL = -46.6564195;
		public static double CalcularDistanciaEmKm(double latitudeFinal, double longitudeFinal)
		{
			GeoCoordinate saidaCoordenada = new GeoCoordinate();
			saidaCoordenada.Latitude = LATITUDE_INICIAL;
			saidaCoordenada.Longitude = LONGITUDE_INICIAL;

			GeoCoordinate entradaCordenada = new GeoCoordinate();
			entradaCordenada.Latitude = latitudeFinal;
			entradaCordenada.Longitude = longitudeFinal;

			double distancia = saidaCoordenada.GetDistanceTo(entradaCordenada);

			double distanciaEmKm = distancia / 1000;
			return distanciaEmKm;
		}
	}
}

using Dapper;
using Dapper.Contrib.Extensions;
using Devboost.DroneDelivery.Domain.Entities;
using Devboost.DroneDelivery.Domain.Enums;
using Devboost.DroneDelivery.Domain.Interfaces.Repository;
using Devboost.DroneDelivery.Repository.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Devboost.DroneDelivery.Repository.Implementation
{
	public class PedidosRepository : IPedidosRepository
	{

		protected readonly string _configConnectionString = "DroneDelivery";

		private IConfiguration _configuracoes;
		public PedidosRepository(IConfiguration config)
		{
			_configuracoes = config;
		}

		public async Task<List<PedidoEntity>> GetAll()
		{
			using (SqlConnection conexao = new SqlConnection(
				_configuracoes.GetConnectionString(_configConnectionString)))
			{

				var list = await conexao.GetAllAsync<Pedido>();

				return ConvertListModelToModelEntity(list.AsList());
			}
		}

		public async Task<PedidoEntity> GetByDroneID(int droneID)
		{
			try
			{
				using (SqlConnection conexao = new SqlConnection(
					_configuracoes.GetConnectionString(_configConnectionString)))
				{
					var p = await conexao.QueryFirstOrDefaultAsync<Pedido>(
						@"SELECT *
                    FROM dbo.Pedido
                    WHERE DroneId = @droneID
                    AND Status = 'PendenteEntrega' ",
						new { DroneId = droneID }
					);

					return ConvertModelToModelEntity(p);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task Inserir(PedidoEntity pedido)
		{
			try
			{
				using (SqlConnection conexao = new SqlConnection(
					_configuracoes.GetConnectionString(_configConnectionString)))
				{

					var Id = Guid.NewGuid();

					var query = @"INSERT INTO Dbo.Pedido(Id, Peso, LatLong, Latitude, Longitude, Datahora, Status, DroneId)		
				VALUES(
				@Id,
				@Peso,
				@LatLong,
				@Latitude,
				@Longitude,
				@DataHora,
				@Status,
				@DroneId
				)";

					await conexao.ExecuteAsync(query, new { Id = Id, Peso = pedido.PesoGramas, LatLong = pedido.LatLong, Latitude = pedido.Latitude, Longitude = pedido.Longitude, DataHora = pedido.DataHora, Status = pedido.DescricaoStatus, DroneId = pedido.DroneId }
				  );
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task Atualizar(PedidoEntity pedido)
		{
			using (SqlConnection conexao = new SqlConnection(
				_configuracoes.GetConnectionString(_configConnectionString)))
			{
				var dataAtualizacao = DateTime.Now;

				var query = @"UPDATE Dbo.Pedido		
			        SET Status = @status, DataHora = @DataHora
                    WHERE Id = @id
                ";

				await conexao.ExecuteAsync(query, new { pedido.DescricaoStatus, pedido.DataHora }
			  );
			}
		}

		public async Task Incluir(PedidoEntity pedido)
		{
			using (SqlConnection conexao = new SqlConnection(
				_configuracoes.GetConnectionString(_configConnectionString)))
			{
				await conexao.InsertAsync<PedidoEntity>(pedido);
			}
		}


		protected List<PedidoEntity> ConvertListModelToModelEntity(List<Pedido> listPedido)
		{

			List<PedidoEntity> newListD = new List<PedidoEntity>();

			foreach (var item in listPedido)
			{
				newListD.Add(ConvertModelToModelEntity(item));
			}
			return newListD;

		}

		protected PedidoEntity ConvertModelToModelEntity(Pedido pedido)
		{
			if (pedido != null)
			{
				PedidoEntity p = new PedidoEntity()
				{
					Id = pedido.Id,
					Status = RetornaPedidoStatus(pedido.Status),
					DroneId = pedido.DroneId,
					DataHora = pedido.DataHora,
					Latitude = pedido.Latitude,
					Longitude = pedido.Longitude,
					PesoGramas = pedido.Peso
				};
				return p;
			}
			else 
			{
				return null;
			}
			

			

		}

		private PedidoStatus RetornaPedidoStatus(string descricaoStatus)
		{
			PedidoStatus status = PedidoStatus.Entregue;
			if (descricaoStatus == PedidoStatus.Entregue.ToString())
				status = PedidoStatus.Entregue;
			if (descricaoStatus == PedidoStatus.EmTransito.ToString())
				status = PedidoStatus.EmTransito;
			if (descricaoStatus == PedidoStatus.PendenteEntrega.ToString())
				status = PedidoStatus.PendenteEntrega;
			return status;
		}

	}
}

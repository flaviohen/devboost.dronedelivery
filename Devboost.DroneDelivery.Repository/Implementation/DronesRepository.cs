using Dapper;
using Dapper.Contrib.Extensions;
using Devboost.DroneDelivery.Domain.Entities;
using Devboost.DroneDelivery.Domain.Enums;
using Devboost.DroneDelivery.Domain.Interfaces.Repository;
using Devboost.DroneDelivery.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Devboost.DroneDelivery.Repository.Implementation
{
    public class DronesRepository : IDronesRepository
    {

		protected readonly string _configConnectionString = "DroneDelivery";

		private IConfiguration _configuracoes;
		public DronesRepository(IConfiguration config)
		{
			_configuracoes = config;
		}

		public async Task<List<DroneEntity>> GetAll()
		{
			using (SqlConnection conexao = new SqlConnection(
				_configuracoes.GetConnectionString(_configConnectionString)))
			{
				var list = await conexao.GetAllAsync<Drone>();
                
                return ConvertListModelToModelEntity(list.AsList());
			}
		}

        public async Task<List<DroneEntity>> GetByStatus(string status)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuracoes.GetConnectionString(_configConnectionString)))
            {
                var list = await conexao.QueryAsync<Drone>(
                    "SELECT * " +
                    "FROM dbo.Drone " +
                    "WHERE Status = @Status",
                    new { Status = status }
                );

                return ConvertListModelToModelEntity(list.AsList());
            }
        }

            public async Task Atualizar(DroneEntity drone)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuracoes.GetConnectionString(_configConnectionString)))
            {
                var dataAtualizacao = DateTime.Now;

                var query = @"UPDATE Dbo.Drone		
			        SET Status = @status, DataAtualizacao = @dataAtualizacao
                    WHERE Id = @id
                ";

               await conexao.ExecuteAsync(query, new { drone.DescricaoStatus, dataAtualizacao, drone.Id }
              );
            }
        }

        public async Task Incluir(DroneEntity drone)
        {
            using (SqlConnection conexao = new SqlConnection(
                _configuracoes.GetConnectionString(_configConnectionString)))
            {
                await conexao.InsertAsync<DroneEntity>(drone);
            }
        }

        protected List<DroneEntity> ConvertListModelToModelEntity(List<Drone> listDrone)
        {

            List<DroneEntity> newListD = new List<DroneEntity>();

            foreach (var item in listDrone)
            {
                newListD.Add(ConvertModelToModelEntity(item));
            }
            return newListD;

        }

        protected DroneEntity ConvertModelToModelEntity(Drone drone)
        {

            DroneEntity p = new DroneEntity()
            {
                Id = drone.Id,
                Status = RetornaStatus(drone.Status),
                AutonomiaMinitos = drone.Autonomia,
                CapacidadeGamas = drone.Capacidade,
                VelocidadeKmH = drone.Velocidade,
                DataAtualizacao = drone.DataAtualizacao
            };

            return p;

        }

        private DroneStatus RetornaStatus(string descricaoStatus) 
        {
            DroneStatus status = DroneStatus.Carregando;
            if (descricaoStatus == DroneStatus.Carregando.ToString())
                 status = DroneStatus.Carregando;
            if (descricaoStatus == DroneStatus.EmTransito.ToString())
                 status = DroneStatus.EmTransito;
            if (descricaoStatus == DroneStatus.Pronto.ToString())
                status = DroneStatus.Pronto;
            return status;
        }
    }
}

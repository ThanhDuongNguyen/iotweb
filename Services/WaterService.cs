using iotweb.Interfaces;
using iotweb.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace iotweb.Services
{
    public delegate void WaterDelegate(object sender, WaterChangeEventArgs args);

    public class WaterChangeEventArgs : EventArgs
    {
        public WaterData NewWater { get; }
        public WaterData OldWater { get; }

        public WaterChangeEventArgs(WaterData newWater, WaterData oldWater)
        {
            this.NewWater = newWater;
            this.OldWater = oldWater;
        }
    }

    public class WaterService : IWaterService, IDisposable
    {
        private const string TableName = "WaterData";
        private SqlTableDependency<WaterData> _notifier;
        private IConfiguration _configuration;

        public event WaterDelegate OnWaterChanged;

        public WaterService(IConfiguration configuration)
        {
            _configuration = configuration;

            _notifier = new SqlTableDependency<WaterData>(_configuration["ConnectionString"], TableName);
            _notifier.OnChanged += this.TableDependency_Changed;
            _notifier.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<WaterData> e)
        {
            if (this.OnWaterChanged != null)
            {
                this.OnWaterChanged(this, new WaterChangeEventArgs(e.Entity, e.EntityOldValues));
            }
        }

        public IList<WaterData> GetCurrentWater()
        {
            var result = new List<WaterData>();

            using (var sqlConnection = new SqlConnection(_configuration["ConnectionString"]))
            {
                sqlConnection.Open();

                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM " + TableName;
                    command.CommandType = CommandType.Text;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(new WaterData
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    MessageId = reader.GetString(reader.GetOrdinal("MessageId")),
                                    Time = reader.GetDateTime(reader.GetOrdinal("Time")),
                                    Temperature = reader.GetString(reader.GetOrdinal("Temperature")),
                                    Turbidity = reader.GetString(reader.GetOrdinal("Turbidity")),
                                    PH = reader.GetString(reader.GetOrdinal("PH")),
                                    Waterflow = reader.GetString(reader.GetOrdinal("Waterflow"))
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void Dispose()
        {
            _notifier.Stop();
            _notifier.Dispose();
        }
    }
}

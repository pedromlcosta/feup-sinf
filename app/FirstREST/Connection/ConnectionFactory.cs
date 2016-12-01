using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Npgsql;

namespace FirstREST {
    public static class ConnectionFactory {

        public static NpgsqlConnection MakePostgresConnection() {

            NpgsqlConnection con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgresConnection"].ToString());
            return con;

        }
    }
}
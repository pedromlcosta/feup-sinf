using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.ErpBS900;         // Use Primavera interop's [Path em C:\Program Files\Common Files\PRIMAVERA\SG800]
using Interop.StdPlatBS900;
using Interop.StdBE900;
using ADODB;

namespace FirstREST.Lib_Primavera
{
    public class PriEngine
    {
        public static StdPlatBS Platform { get; set; }
        public static ErpBS Engine { get; set; }
        public static bool initialized = false;

        public static bool InitializeCompany(string Company, string User, string Password)
        {
            if (!initialized)
            {
                StdBSConfApl objAplConf = new StdBSConfApl();
                objAplConf.Instancia = "Default";
                objAplConf.AbvtApl = "GCP";
                objAplConf.PwdUtilizador = Password;
                objAplConf.Utilizador = User;
                objAplConf.LicVersaoMinima = "9.00";
                StdBETransaccao objStdTransac = new StdBETransaccao();

                StdPlatBS Plataforma = new StdPlatBS();
                ErpBS MotorLE = new ErpBS();

                EnumTipoPlataforma objTipoPlataforma = new EnumTipoPlataforma();
                objTipoPlataforma = EnumTipoPlataforma.tpProfissional;

                objAplConf.Instancia = "Default";
                objAplConf.AbvtApl = "GCP";
                objAplConf.PwdUtilizador = Password;
                objAplConf.Utilizador = User;
                objAplConf.LicVersaoMinima = "9.00";

                // Retuns the ptl.
                Platform = Plataforma;
                // Returns the engine.
                Engine = MotorLE;

                try
                {
                    Platform.AbrePlataformaEmpresa(ref Company, ref objStdTransac, ref objAplConf, ref objTipoPlataforma, "");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error on open Primavera Platform.");
                }

                // Is plt initialized?
                if (Platform.Inicializada)
                {
                    bool blnModoPrimario = true;
                    // Open Engine
                    Engine.AbreEmpresaTrabalho(EnumTipoPlataforma.tpProfissional, ref Company, ref User, ref Password, ref objStdTransac, "Default", ref blnModoPrimario);
                    Engine.set_CacheActiva(false);
                    initialized = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return true;
        }
    }
}
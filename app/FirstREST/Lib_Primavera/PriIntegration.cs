﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Interop.ErpBS900;
using Interop.StdPlatBS900;
using Interop.StdBE900;
using Interop.GcpBE900;
using ADODB;
using System.Data;
using FirstREST.Controllers;

namespace FirstREST.Lib_Primavera
{
    public class PriIntegration
    {


        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {

            StdBELista objList;

            List<Model.Cliente> listClientes = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Moeda, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo FROM  CLIENTES");


                while (!objList.NoFim())
                {
                    listClientes.Add(new Model.Cliente
                    {
                        CodCliente = objList.Valor("Cliente"),
                        NomeCliente = objList.Valor("Nome"),
                        Moeda = objList.Valor("Moeda"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        Morada = objList.Valor("campo_exemplo")
                    });
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {


            GcpBECliente objCli = new GcpBECliente();


            Model.Cliente myCli = new Model.Cliente();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.CodCliente = objCli.get_Cliente();
                    myCli.NomeCliente = objCli.get_Nome();
                    myCli.Moeda = objCli.get_Moeda();
                    myCli.NumContribuinte = objCli.get_NumContribuinte();
                    myCli.Morada = objCli.get_Morada();
                    myCli.NumTelemovel = objCli.get_Telefone();
                    myCli.CodPostal = objCli.get_CodigoPostal();
                    myCli.LocalidadeCP = objCli.get_LocalidadeCodigoPostal();
                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodCliente);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(cliente.NomeCliente);
                        objCli.set_NumContribuinte(cliente.NumContribuinte);
                        objCli.set_Moeda(cliente.Moeda);
                        objCli.set_Morada(cliente.Morada);

                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }


        public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }



        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();


            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myCli.set_Cliente(cli.CodCliente);
                    myCli.set_Nome(cli.NomeCliente);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda(cli.Moeda);
                    myCli.set_Morada(cli.Morada);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }


        public static string getClienteName(string codCliente)
        {
            if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
            {
                // Cliente does not exist in primavera
                GcpBECliente objCli = new GcpBECliente();
                objCli = Lib_Primavera.PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                return objCli.get_Nome();
            }
            return null;
        }

        public static int registerCliente(string codCliente, string email, string nome, string morada, string nif, string telemovel, string localidade, string cp)
        {
            Debug.Write("NOME: " + nome);
            Debug.Write("EMAIL: " + email);

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    GcpBECliente objCli;
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                    {
                        Debug.Write("USER JÁ EXISTE NO PRIMAVERA\n");
                        return -1;
                    }

                    objCli = new GcpBECliente();
                    objCli.set_Cliente(codCliente);
                    objCli.set_Moeda("EUR");
                    objCli.set_B2BEnderecoMail(email);
                    objCli.set_Nome(nome);
                    objCli.set_Morada(morada);
                    objCli.set_Telefone(telemovel);
                    objCli.set_Localidade(localidade);
                    objCli.set_LocalidadeCodigoPostal(localidade);
                    objCli.set_CodigoPostal(cp);
                    objCli.set_NumContribuinte(nif);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return -1;
            }
            return -1;

        }
        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------

        # region OrderStatus

        public static List<Model.OrderStatus> ListaOrderStatus()
        {


            StdBELista objList;

            List<Model.OrderStatus> listOrdStatus = new List<Model.OrderStatus>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT IdCabecDoc,Estado,Anulado,Fechado FROM  CabecDocStatus");


                while (!objList.NoFim())
                {
                    listOrdStatus.Add(new Model.OrderStatus
                    {
                        idCabecDoc = objList.Valor("idcabecdoc"),
                        Estado = objList.Valor("estado"),
                        Anulado = objList.Valor("anulado"),
                        Fechado = objList.Valor("fechado")
                    });
                    objList.Seguinte();

                }

                return listOrdStatus;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.OrderStatus GetOrderStatus(string idCabecDoc)
        {
            StdBELista objO;

            Model.OrderStatus myOrderStatus = new Model.OrderStatus();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objO = PriEngine.Engine.Consulta("SELECT Estado,Anulado,Fechado FROM  CabecDocStatus WHERE IdCabecDoc='" + idCabecDoc + "' ");
                myOrderStatus.idCabecDoc = idCabecDoc;
                myOrderStatus.Estado = objO.Valor("Estado");
                myOrderStatus.Anulado = objO.Valor("Anulado");
                myOrderStatus.Fechado = objO.Valor("Fechado");
                return myOrderStatus;
            }
            else
                return null;
        }

        #endregion OrderStatus

        #region Artigo

        public static string getCurrencySymbol(String symbol)
        {
            string currSymbol;
            if (Model.CurrencyTools.TryGetCurrencySymbol(symbol, out currSymbol))
            {

                return currSymbol;
            }

            return "€";
        }

        public static Double getPrecoCambio(Double valor, String moeda)
        {
            StdBELista objList;
            Double retorno;
            objList = PriEngine.Engine.Consulta("SELECT * FROM Moedas WHERE Moeda = '" + moeda + "';");
            retorno = Convert.ToDouble(objList.Valor("compra")) * valor;
            return retorno;
        }

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            StdBELista objList;
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    objList = PriEngine.Engine.Consulta("SELECT Artigo.Artigo,Artigo.Descricao,Artigo.Observacoes,PVP1,ArtigoMoeda.PVP1IvaIncluido,STKActual,Marca,Artigo.Familia,Artigo.SubFamilia,Familias.Descricao AS FamiliaDesc,SubFamilias.Descricao AS SubFamiliaDesc,IVA,ArtigoMoeda.moeda AS moeda FROM ArtigoMoeda,Artigo,Familias,SubFamilias WHERE ArtigoMoeda.Artigo= Artigo.Artigo AND Artigo.Familia = Familias.Familia AND Artigo.SubFamilia = SubFamilias.SubFamilia AND SubFamilias.Familia=Familias.Familia AND PVP1>=0 ");
                    String moeda = objList.Valor("moeda");

                    myArt.CodArtigo = objList.Valor("artigo");
                    myArt.DescArtigo = objList.Valor("descricao");
                    myArt.FullDesc = objList.Valor("Observacoes");
                    myArt.PVP1 = objList.Valor("PVP1");
                    myArt.PVP1_IVA = objList.Valor("PVP1IvaIncluido");
                    myArt.StockActual = objList.Valor("STKActual");
                    myArt.Marca = objList.Valor("Marca");
                    myArt.familia = objList.Valor("Familia");
                    myArt.subFamilia = objList.Valor("SubFamilia");
                    myArt.subFamiliaDesc = objList.Valor("SubFamiliaDesc");
                    myArt.familiaDesc = objList.Valor("FamiliaDesc");
                    myArt.IVA = objList.Valor("iva");

                    //TODO: get artigo image from pgsql
                    myArt.imageURL = ImageUploadController.getArtigoImg(myArt.CodArtigo);
                    myArt.reviewInfo = ReviewController.getReviews(myArt.CodArtigo);

                    if (moeda != "EUR")
                        myArt.PVP1 = getPrecoCambio(myArt.PVP1, moeda);

                    myArt.moeadaSymbol = getCurrencySymbol("EUR");

                    return myArt;
                }
            }
            else
            {
                return null;
            }
        }

        public static List<Model.Artigo> ListaArtigos()
        {

            StdBELista objList;

            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT Artigo.Artigo,Artigo.Descricao,Artigo.Observacoes,PVP1,ArtigoMoeda.PVP1IvaIncluido,STKActual,Marca,Artigo.Familia,Artigo.SubFamilia,Familias.Descricao AS FamiliaDesc,SubFamilias.Descricao AS SubFamiliaDesc,IVA,ArtigoMoeda.moeda AS moeda FROM ArtigoMoeda,Artigo,Familias,SubFamilias WHERE ArtigoMoeda.Artigo= Artigo.Artigo AND Artigo.Familia = Familias.Familia AND Artigo.SubFamilia = SubFamilias.SubFamilia AND SubFamilias.Familia=Familias.Familia AND PVP1>=0");
                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {

                    art = new Model.Artigo();
                    String moeda = objList.Valor("moeda");

                    art.CodArtigo = objList.Valor("artigo");
                    art.DescArtigo = objList.Valor("descricao");
                    art.FullDesc = objList.Valor("Observacoes");
                    art.PVP1 = objList.Valor("PVP1");
                    art.PVP1_IVA = objList.Valor("PVP1IvaIncluido");
                    art.StockActual = objList.Valor("STKActual");
                    art.Marca = objList.Valor("Marca");
                    art.familia = objList.Valor("Familia");
                    art.subFamilia = objList.Valor("SubFamilia");
                    art.subFamiliaDesc = objList.Valor("SubFamiliaDesc");
                    art.familiaDesc = objList.Valor("FamiliaDesc");
                    art.IVA = objList.Valor("iva");

                    //TODO: get artigo image from pgsql
                    art.imageURL = ImageUploadController.getArtigoImg(art.CodArtigo);
                    art.reviewInfo = ReviewController.getReviews(art.CodArtigo);

                    if (moeda != "EUR")
                        art.PVP1 = getPrecoCambio(art.PVP1, moeda);
                    art.moeadaSymbol = getCurrencySymbol("EUR");
                    listArts.Add(art);

                    objList.Seguinte();

                }
                Debug.Write(" ARTIGO END \n\n" + listArts.Count);
                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static bool editArtigo(RequestObjects.EditArtigoData data)
        {
            String aritgoID;
            bool returnFlag = false;
            if (data.fieldToEdit.Equals("desc"))
            {
                aritgoID = data.idOfProduct;
                GcpBEArtigo objArtigo = new GcpBEArtigo();

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Artigos.Existe(aritgoID) == false)
                    {
                        Debug.Write("Artigo não Existe");
                        return false;
                    }
                    else
                    {
                        objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(aritgoID);
                        objArtigo.set_EmModoEdicao(true);
                        objArtigo.set_Observacoes(data.valueToSet);
                        PriEngine.Engine.Comercial.Artigos.Actualiza(objArtigo);
                        returnFlag = true;
                    }
                }
            }
            return returnFlag;
        }

        
        #endregion Artigo

        #region ArtigoArmazem

        //retorna armazéns que vendem artigo com codArtigo
        public static List<Lib_Primavera.Model.ArtigoArmazem> GetArtigoArmazens(string codArtigo)
        {

            StdBELista objArtigoArmazem;
            Model.ArtigoArmazem myArtArm = new Model.ArtigoArmazem();
            List<Model.ArtigoArmazem> listArtArms = new List<Model.ArtigoArmazem>();
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {


                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigoArmazem = PriEngine.Engine.Consulta("SELECT Armazem,Artigo,PCMedio,sum(StkActual) as StockActual FROM ArtigoArmazem WHERE Artigo LIKE '" + codArtigo + "'  GROUP BY Armazem,Artigo,PCMedio HAVING sum(StkActual)>=0 and PCMedio>0");
                    while (!objArtigoArmazem.NoFim())
                    {
                        myArtArm = new Model.ArtigoArmazem();
                        myArtArm.Armazem = objArtigoArmazem.Valor("armazem");
                        myArtArm.CodArtigo = objArtigoArmazem.Valor("artigo");
                        myArtArm.StockActual = objArtigoArmazem.Valor("stockactual");
                        myArtArm.PCMedio = objArtigoArmazem.Valor("pcmedio");

                        listArtArms.Add(myArtArm);
                        objArtigoArmazem.Seguinte();
                    }
                    return listArtArms;
                }

            }
            else
            {
                return null;
            }

        }




        #endregion ArtigoArmazem

        #region DocsVenda

        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();

            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();

            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    myEnc.set_Morada(dv.Morada);
                    myEnc.set_CodigoPostal(dv.CodPostal);
                    myEnc.set_TotalMerc(dv.TotalMerc);
                    myEnc.set_TotalIva(dv.TotalIva);
                    myEnc.set_TotalDesc(dv.TotalDesc);
                    myEnc.set_NumContribuinte(dv.numContribuinte);
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasDoc;
                    //PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc);
                    foreach (Model.LinhaDocVenda lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto, "", 0, 0, 0, 0, 0, 0, 0, true, false, lin.PrecoUnitario * lin.TaxaIva);
                    }


                    // PriEngine.Engine.Comercial.Compras.TransformaDocumento(

                    PriEngine.Engine.IniciaTransaccao();
                    //PriEngine.Engine.Comercial.Vendas.Edita Actualiza(myEnc, "Teste");
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }



        public static List<Model.DocVenda> Encomendas_List()
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade,NumContribuinte,Morada,Data, NumDoc, TotalMerc, TotalIVA,TotalDesc From CabecDoc where TipoDoc='ECL'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.Morada = objListCab.Valor("Morada");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.numContribuinte = objListCab.Valor("NumContribuinte");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.TotalIva = objListCab.Valor("TotalIva");
                    dv.TotalDesc = objListCab.Valor("TotalDesc");

                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido,TaxaIVA,Armazem from LinhasDoc where Quantidade>0 and IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindv.TaxaIva = objListLin.Valor("TaxaIVA");
                        lindv.Armazem = objListLin.Valor("Armazem");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }


        public static List<Model.DocVenda> Encomenda_Get_EncomendaCliente(string codCliente)
        {

            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade,Morada,NumContribuinte,CodPostal, Data, NumDoc, TotalMerc,TotalIva,TotalDesc From CabecDoc where TipoDoc='ECL' and Entidade='" + codCliente + "' ORDER BY Data DESC");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.Morada = objListCab.Valor("Morada");
                    dv.numContribuinte = objListCab.Valor("NumContribuinte");
                    dv.CodPostal = objListCab.Valor("CodPostal");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.TotalIva = objListCab.Valor("TotalIva");
                    dv.TotalDesc = objListCab.Valor("TotalDesc");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido,TaxaIVA,Armazem from LinhasDoc where Quantidade>0 and IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindv.TaxaIva = objListLin.Valor("TaxaIVA");
                        lindv.Armazem = objListLin.Valor("Armazem");


                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        public static Model.DocVenda Encomenda_Get(string numdoc)
        {


            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {


                string st = "SELECT id, Entidade,NumContribuinte, Data, NumDoc, TotalMerc, Morada,TotalIva,TotalDesc From CabecDoc where TipoDoc='ECL' and NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.numContribuinte = objListCab.Valor("NumContribuinte");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.TotalMerc = objListCab.Valor("TotalMerc");
                dv.Morada = objListCab.Valor("Morada");
                dv.TotalIva = objListCab.Valor("TotalIva");
                dv.TotalDesc = objListCab.Valor("TotalDesc");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido,TaxaIVA,Armazem from LinhasDoc where Quantidade>0 and IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");

                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    lindv.TaxaIva = objListLin.Valor("TaxaIVA");
                    lindv.Armazem = objListLin.Valor("Armazem");
                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }

        #endregion DocsVenda

        # region CategoriaArtigo

        public static List<Lib_Primavera.Model.Categoria> ListaCategorias()
        {
            StdBELista objList;

            List<Lib_Primavera.Model.Categoria> listCategorias = new List<Lib_Primavera.Model.Categoria>();
            Lib_Primavera.Model.Categoria categoriaObj;
            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT Familias.Familia,SubFamilias.SubFamilia,SubFamilias.Ordem, Familias.Descricao AS FamiliaDesc ,SubFamilias.Descricao AS SubFamiliaDesc FROM Familias,SubFamilias;");

                while (!objList.NoFim())
                {
                    string familiaCodigo = objList.Valor("Familia");
                    if ((categoriaObj = listCategorias.Find(obj => obj.familiaCod == familiaCodigo)) == null)
                    {
                        listCategorias.Add(new Model.Categoria
                        {

                            familiaCod = familiaCodigo,
                            familiaDesc = objList.Valor("FamiliaDesc"),

                        });
                        categoriaObj = listCategorias[listCategorias.Count - 1];
                    }
                    categoriaObj.addSubFamilia(new Lib_Primavera.Model.SubCategoria(objList.Valor("SubFamilia"), objList.Valor("SubFamiliaDesc")));
                    objList.Seguinte();

                }
                return listCategorias;
            }
            else
                return null;
        }

        public static Model.CategoriaArtigo GetCategoriaArtigos(string familia)
        {
            GcpBEFamilia objFamilia = new GcpBEFamilia();
            Model.ArtigoCategoria categoriaArtigo = new Model.ArtigoCategoria();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Familias.Existe(familia) == true)
                {
                    objFamilia = PriEngine.Engine.Comercial.Familias.Edita(familia);

                    string familiaCod = objFamilia.get_Familia();
                    string query = System.String.Empty;
                    query += "SELECT Artigo.Artigo,Familias.Familia,SubFamilias.SubFamilia, Familias.Descricao AS FamiliaDesc ,SubFamilias.Descricao AS SubFamiliaDesc FROM Artigo,Familias,SubFamilias WHERE Familias.Familia= '";
                    query += familiaCod + "' AND Artigo.Familia = Familias.Familia AND SubFamilias.SubFamilia = Artigo.SubFamilia AND Familias.Familia = SubFamilias.Familia;";

                  
                    StdBELista objList = PriEngine.Engine.Consulta(query);
                    Model.CategoriaArtigo categoriaArtigoObj = new Model.CategoriaArtigo();
                    if (!objList.Vazia())
                    {
                        categoriaArtigoObj.familia = objList.Valor("Familia");
                        categoriaArtigoObj.familiaDesc = objList.Valor("FamiliaDesc");
                        categoriaArtigoObj.subFamiliaDesc = objList.Valor("SubFamiliaDesc");

                        categoriaArtigoObj.artigo = new List<Tuple<string, string>>();
                        while (!objList.NoFim())
                        {

                            categoriaArtigoObj.addArtigo(objList.Valor("Artigo"), objList.Valor("SubFamilia"));
                            objList.Seguinte();

                        }

                        return categoriaArtigoObj;
                    }
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        #endregion CategoriaArtigo;   // -----------------------------  END   CategoriaArtigo    -----------------------


    }
}
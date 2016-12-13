using System;
using Interop.ErpBS900;
using Interop.StdPlatBS900;
using Interop.StdBE900;
using Interop.GcpBE900;
using ADODB;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Lib_Primavera
{
    public class CartController : ApiController
    {
        public HttpResponseMessage Post(CartData data)
        {

            string userID = data.id;
            string date = data.date;
            string address = data.address;
            List<Purchase> products = data.products;
            if (userID != null && date != null && address != null && products != null)
            {
                Lib_Primavera.Model.RespostaErro erro;
                erro = encomendaPrimavera(userID, date, address, products);
                if (erro.Descricao.Equals("Sucesso"))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    Debug.WriteLine(erro.Descricao);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable);
                }
            }
            return Request.CreateResponse(HttpStatusCode.NotAcceptable);
        }
        public Lib_Primavera.Model.RespostaErro encomendaPrimavera(string userID, string date, string address, List<Purchase> products)
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
                    //parse date to Datetime or wtv
                    //myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(userID);
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C"); //Client
                    myEnc.set_ModoPag("TRA"); //Online transfer
                    myEnc.set_CondPag("1"); //Request immediate payment.
                    myEnc.set_DataDoc(DateTime.Today);
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc);
                    foreach (Purchase p in products)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, p.id, p.quantity, "", "", p.price, 0);
                    }
                    PriEngine.Engine.IniciaTransaccao();
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
    }
}

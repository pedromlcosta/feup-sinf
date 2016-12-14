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
            string name = data.name;
            string email = data.email;
            string nif = data.nif;
            string address = data.address;
            string postal = data.postal;
            string payment = data.payment;
            List<Purchase> products = data.products;
            bool debug = true;
            if (userID != null && name != null && email != null && nif != null  && address != null && postal != null && payment != null)
            {
                if(debug)
                {
                    Debug.WriteLine(userID);
                    Debug.WriteLine(name);
                    Debug.WriteLine(email);
                    Debug.WriteLine(nif);
                    Debug.WriteLine(address);
                    Debug.WriteLine(postal);
                    Debug.WriteLine(payment);
                }
                Lib_Primavera.Model.RespostaErro erro;
                erro = encomendaPrimavera(userID, name, email,nif, address, postal, payment, products);
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
        public Lib_Primavera.Model.RespostaErro encomendaPrimavera(string userID, string name, string email, string nif, string address, string postal, string payment, List<Purchase> products)
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
                    myEnc.set_Entidade(userID);
                    myEnc.set_NumContribuinte(nif);

                    myEnc.set_Tipodoc("ECL"); //Encomenda de cliente
                    myEnc.set_TipoEntidade("C"); //Client
                    if (payment.Equals("paypal")) myEnc.set_ModoPag("TRA"); //Online transfer
                    else myEnc.set_ModoPag("TRA"); //Online transfer
                    myEnc.set_CondPag("1"); //Request immediate payment.
                    myEnc.set_DataDoc(DateTime.Today);

                    myEnc.set_CodigoPostal(postal);
                    myEnc.set_CodPostalEntrega(postal);
                    myEnc.set_CodPostalLocalidadeEntrega(postal);

                    myEnc.set_Morada(address);
                    myEnc.set_MoradaEntrega(address);
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

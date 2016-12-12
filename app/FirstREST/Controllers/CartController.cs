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
                /*
                return Request.CreateResponse(HttpStatusCode.OK);

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
                        //myEnc.set_DataDoc(dv.Data);
                        myEnc.set_Entidade(userID);
                        myEnc.set_Tipodoc("ECL");
                        myEnc.set_TipoEntidade("C");
                        // Linhas do documento para a lista de linhas
                        //lstlindv = dv.LinhasDoc;
                        //PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                        PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc);
                        foreach (Model.LinhaDocVenda lin in lstlindv)
                        {
                            PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, "", "", lin.PrecoUnitario, lin.Desconto);
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
                 */
            }
            return Request.CreateResponse(HttpStatusCode.NotAcceptable);
        }
    }
}

// JavaScript source code
// JavaScript source code
var orders;
var orderStatus;

window.onload = getOrderHistoryRequest;
function getOrderHistoryRequest()
{
    // Currently logged in user.
    var codCliente = $("#codCliente_input").val();
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() 
    {
        if (this.readyState == 4 && this.status == 200) {
            orders = JSON.parse(xhttp.responseText);
            current_filtered_orders = JSON.parse(xhttp.responseText);
            processOrders(orders,0,8);
        }
    };
    xhttp.open("GET", "http://localhost:49822/api/DocVenda/"+codCliente, true);
    xhttp.setRequestHeader("Content-Type", "text/json");
    xhttp.send();
}
function processOrders(orders,start_index,end_index)
{

    var i;  
    var orderHolder = document.getElementById("order-holder");
    //For the modals.
    orderHolder.innerHTML = "";
    if(orders.length < end_index) end_index = orders.length;
    for(i=start_index;i<end_index;i++)
    {
        
        var orderID = orders[i].id.substring(1,orders[i].id.length-1);
        var entidade = orders[i].Entidade;
        var morada = orders[i].Morada;
        var codPostal = orders[i].CodPostal;        
        var data = orders[i].Data.substring(0,10);
        var totalMerc = orders[i].TotalMerc;
        var totalIva = orders[i].TotalIva;
        var totalDesc = orders[i].TotalDesc;
        var soldItems = orders[i].LinhasDoc;
        var final = totalMerc+totalIva;
        // totalMerc é o preço sem iva. totalIVA é IVA*totalMerc. Preço final = totalMerc + totalIVA.
        var pricewithoutIva=totalMerc;
        orderHolder.innerHTML += `<div class='col-md4 pro-1'>
						<div class='col-cs'>
							<a href='#' data-toggle='modal' data-target='#EncomendaModal' onclick="getOrderDetails('`+orderID+`');">
                                <h6 >ID:` + orderID + `</h6>
							</a>
								<div>
                                    <label>Cliente:</label><span>` + entidade + `</span><br>
                                    <label>Morada:</label><span>`+ morada+`, `+codPostal+`</span><br>
                                    <label>Order Date:</label><span>`+data+`</span><br>                      
                                    <br>
                                    <label>Cost Breakdown:</label>
									<p>+ Preço:€`+pricewithoutIva+`</p>
                                    <p>+ IVA:€`+totalIva+`</p>
                                    <p>- Desconto:€`+totalDesc+`</p>
                                    <p>-----------------------------------------</p>
                                    <p>= Total:€`+final+`</p>
									  <div class='block'>
										<div class='starbox small ghosting'> </div>
									</div>
								</div>
									
							</div>
						</div>
					</div>`;
    }
}

function getOrderDetails(orderID)
{
    $("#EncomendaModal .productListing").empty();
    var order;
    for(var c=0; c< orders.length; c++){
        if(orderID==orders[c].id.substring(1,orders[c].id.length-1)){
            order=orders[c];
        }
    }

  
    $("#EncomendaModal .productListing").html(`
    <table>
    <tr>    
    <th style="width: 5%;">Armazem</th>
    <th style="width: 15%;">Descrição</th> 
    <th style="width: 5%;">Quantidade</th>
    <th style="width: 5%;">Preço(Un)</th>
    <th style="width: 5%;">Iva</th>   
    </tr>
    `);
    
    
    for (var i= 0; i < order.LinhasDoc.length; i++) {
        $("#EncomendaModal .productListing").append(`
        <tr>
        <td style="width: 5.3%;">`+order.LinhasDoc[i].Armazem+`</td>
        <td style="width: 14%;">`+order.LinhasDoc[i].DescArtigo+`</td>
        <td style="width: 7%;">`+order.LinhasDoc[i].Quantidade+`x</td>
        <td style="width: 5.1%;">`+order.LinhasDoc[i].PrecoUnitario+`€</td>
        <td style="width: 5%;">`+order.LinhasDoc[i].TaxaIva+`%   </td>
        </tr>`);
    }
    $("#EncomendaModal .productListing").append(`</table>`);
    
    getOrderStatus(orderID);
	

}
function getOrderStatus(orderID)
{
    $("#EncomendaModal .orderStatus").empty();
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() 
    {
        if (this.readyState == 4 && this.status == 200) {
            orderStatus = JSON.parse(xhttp.responseText);
            if(orderStatus == null)
            {   
                $("#EncomendaModal .orderStatus").append("<p>Order not processed yet.</p>");
            }
            else
            {
               // $("#EncomendaModal .orderStatus").append(`<p>`+orderStatus.Estado+`</p>`);
                switch(orderStatus.Estado)
                {
                    case 'T':
                        if(orderStatus.Fechado==1){
                        $("#EncomendaModal .orderStatus").append("<p>Order closed.</p>");
                        break;
                        }
                            
                        if(orderStatus.Anulado==0)
                            $("#EncomendaModal .orderStatus").append("<p>Payment Confirmed, Shipped.</p>");
                        else $("#EncomendaModal .orderStatus").append("<p>Anulled</p>");
                        break;
                    case 'P':
                        if(orderStatus.Fechado==1){
                        $("#EncomendaModal .orderStatus").append("<p>Order closed.</p>");
                        break;
                        }
                        if(orderStatus.Anulado==0)
                            $("#EncomendaModal .orderStatus").append("<p>Approved Order,Pending to confirm payment</p>");
                        else $("#EncomendaModal .orderStatus").append("<p>Anulled</p>");
                        break;
                    case 'F':
                        if(orderStatus.Fechado==1)
                        $("#EncomendaModal .orderStatus").append("<p>Order closed.</p>");
                        break;
                    case 'R':
                        $("#EncomendaModal .orderStatus").append("<p>Rejected/Anulled</p>");
                        break;
                   
                }
            }
        }
    };
    xhttp.open("GET", "http://localhost:49822/api/OrderStatus/"+orderID, true);
    xhttp.setRequestHeader("Content-Type", "text/json");
    xhttp.send();
}


             


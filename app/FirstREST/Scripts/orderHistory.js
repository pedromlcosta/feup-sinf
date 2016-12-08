// JavaScript source code
// JavaScript source code
var orders;
var current_filtered_orders = new Array();
window.onload = getOrderHistoryRequest;
function getOrderHistoryRequest()
{
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() 
    {
        if (this.readyState == 4 && this.status == 200) {
            orders = JSON.parse(xhttp.responseText);
            current_filtered_orders = JSON.parse(xhttp.responseText);
            processOrders(orders,0,8);
        }
    };
    xhttp.open("GET", "http://localhost:49822/api/DocVenda/ALCAD", true);
    xhttp.setRequestHeader("Content-Type", "text/json");
    xhttp.send();
}
function processOrders(orders,start_index,end_index)
{
    var i;
    //alert(orders[4].CodArtigo);
    var orderHolder = document.getElementById("order-holder");
    //For the modals.
    var j=0;
    orderHolder.innerHTML = "";
    if(orders.length < end_index) end_index = orders.length;
    console.log(orders);
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
        //console.log(typeof(data));
        var final = (totalMerc-totalDesc)+totalIva;
        orderHolder.innerHTML += `<div class='col-md4 pro-1'>
						<div class='col-cs'>
							<a href='#' data-toggle='modal' data-target='#myModal`+j+`' onclick="getOrderStatus(`+j+`);">
                                <h6 ><a href='single.html'>ID:` + orderID + `</a> </h6>
							</a>
								<div class='mid-1'>
                                    <label>Cliente:</label><span>` + entidade + `</span><br>
                                    <label>Morada:</label><span>`+ morada+`, `+codPostal+`</span><br>
                                    <label>Order Date:</label><span>`+data+`</span><br>                      
                                    <br>
                                    <label>Cost Breakdown:</label>
									<p>+ Preço:€`+totalMerc+`</p>
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
		/*$("#myModal"+j+" h3").html(desc);
		$("#myModal"+j+" .quick").html("");
		$("#myModal"+j+" .reducedfrom").html("€"+final);
		$("#myModal"+j+" .in-para").html("");
		$("#myModal"+j+" .quick_desc").html("");*/

		j++;
    }
}

/*<table align="right">
<tr>
<th>|Armazem|</th>
<th>Descrição|</th> 
<th>Quantidade|</th>
<th>Preço(Un)|</th>
<th>Iva|</th>    
</tr>
for (var i= 0; i < soldItems.length; i++) {

 <tr>
 <th>`+soldItems[i].Armazem+`</th>
 <th>`+soldItems[i].DescArtigo+`</th>
 <th>`+soldItems[i].Quantidade+`</th>
 <th>`+soldItems[i].PrecoUnitario+`</th>
 <th>`+soldItems[i].TaxaIva+`</th>
 </tr>
  </table>
 
}*/
             
    /*
function filterorders(string)
{
	current_filtered_orders = [];
	for(var i=0;i<orders.length;i++)
    {
		var desc = orders[i].DescArtigo
		if(desc.toLowerCase().indexOf(string) !== -1)
	    {
			current_filtered_orders.push(orders[i]);
		}
	}
	if(string != "" ) processorders(current_filtered_orders,0,8);
	else processorders(orders,0,8);
}
function filterordersbyCategory(string)
{
    current_filtered_orders = [];
    for(var i=0;i<orders.length;i++)
    {
        var catg = orders[i].subFamiliaDesc;
        if(catg.toLowerCase().indexOf(string.toLowerCase()) !== -1)
        {
            console.log("match");
            current_filtered_orders.push(orders[i]);
        }
    }
    if(string != "" ) processorders(current_filtered_orders,0,8);
    else processorders(orders,0,8);
}
function getStorage(modal)
{
	var id = $("#myModal"+modal+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-id");
	getStorages(id,modal);
	

}
function getStorages(id,modal)
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function() 
	{
	    if (this.readyState == 4 && this.status == 200) {
    		storages = JSON.parse(xhttp.responseText);
    		$("#myModal"+modal+" .quick").html("");
    		if(storages == null)
	        {   
    		    $("#myModal"+modal+" .quick").append("<p>Out of Stock.</p>");
    		}
    		else
    		{
    		    if(storages.length > 0)
    		    {
    		        storages.forEach(function(item, index)
    		        {
    		            $("#myModal"+modal+" .quick").append("<p>"+item.Armazem+ "<span style='color:green;'>✔</span></p>");
    		        });
    		    }
    		    else
    		    {
    		        $("#myModal"+modal+" .quick").append("<p>Out of Stock.</p>");
    		    }
    		}
	    }
	};
	xhttp.open("GET", "http://localhost:49822/api/ArtigoArmazem/"+id, true);
	xhttp.setRequestHeader("Content-Type", "text/json");
	xhttp.send();
}
*/
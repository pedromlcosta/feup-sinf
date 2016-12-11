var articles;
var current_filtered_articles = new Array();
window.onload = getArticlesRequest;
function getArticlesRequest()
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function() 
	{
    if (this.readyState == 4 && this.status == 200) {
    		articles = JSON.parse(xhttp.responseText);
    		current_filtered_articles = JSON.parse(xhttp.responseText);
       		processArticles(articles,0,8);
    	}
	};
	xhttp.open("GET", "http://localhost:49822/api/artigos", true);
	xhttp.setRequestHeader("Content-Type", "text/json");
	xhttp.send();
}
function processArticles(articles,start_index,end_index)
{
	var i;
	//alert(articles[4].CodArtigo);
	var articleHolder = document.getElementById("article-holder");
	//For the modals.
	var j=0;
	articleHolder.innerHTML = "";
	if(articles.length < end_index) end_index = articles.length;
	for(i=start_index;i<end_index;i++)
	{
		var codArtigo = articles[i].CodArtigo
		var desc = articles[i].DescArtigo;
		var price = articles[i].PCPadrao;
		var marca = articles[i].Marca;
		var stock = articles[i].StockActual;
		var iva = articles[i].IVA;
		var withIVA = price + (iva/100)*price;
		var stars_div = "";
		for(var n=0;n<5;n++)
		{
		    stars_div += `<img src='../../../Images/star.jpg' style="width: 20px; align:left">`;
		}
		articleHolder.innerHTML += `<div class='col-md-3 pro-1'>
						<div class='col-m'>
							<a href='#' data-toggle='modal' data-target='#myModal`+j+`' class='offer-img'>
								<img src='../../../Images/i7.png' class='img-responsive' alt='' onclick="getStorage(`+j+`);" >
							</a>
                        
							<div class='mid-1'>
								<div class='women'>
									<h6><a href='single.html'>` + desc.substring(0,23) + `</a></h6>
                                     <div class="review" >` + stars_div + `</div>
								</div>
								<div class='mid-2'>
									<p ><label></label><em class='item_price'>€`+withIVA+`</em> </p>
                                        
									  <div class='block'>
										<div class='starbox small ghosting'> </div>
									</div>
                                   
								</div>
								 	
							</div>
						</div>
                       
					</div>`;
		$("#myModal"+j+" h3").html(desc);
		$("#myModal"+j+" .quick").html("");
		$("#myModal"+j+" .reducedfrom").html("€"+withIVA);
		$("#myModal"+j+" .in-para").html(`Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor 
        in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. `);
		$("#myModal"+j+" .quick_desc").html("");
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-id",codArtigo);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-name",desc);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-summary",desc);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-price",withIVA);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-quantity","1");
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-image","images/i7.png");

		j++;
	}
}
function filterArticles(string)
{
	current_filtered_articles = [];
	for(var i=0;i<articles.length;i++)
	{
		var desc = articles[i].DescArtigo
		if(desc.toLowerCase().indexOf(string) !== -1)
		{
			current_filtered_articles.push(articles[i]);
		}
	}
	if(string != "" ) processArticles(current_filtered_articles,0,8);
	else processArticles(articles,0,8);
}
function filterArticlesbyCategory(string)
{
    current_filtered_articles = [];
    for(var i=0;i<articles.length;i++)
    {
        var catg = articles[i].subFamiliaDesc;
        if(catg.toLowerCase().indexOf(string.toLowerCase()) !== -1)
        {
            console.log("match");
            current_filtered_articles.push(articles[i]);
        }
    }
    if(string != "" ) processArticles(current_filtered_articles,0,8);
    else processArticles(articles,0,8);
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
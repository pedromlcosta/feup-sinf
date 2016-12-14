var articles;
var families;
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
            getSubFamilies(articles);
    	}
	};
	xhttp.open("GET", "http://localhost:49822/api/artigos", true);
	xhttp.setRequestHeader("Content-Type", "text/json");
	xhttp.send();
}
function getSubFamilies(articles)
{
    families = [];
    for(var i=0;i<articles.length;i++)
    {
        if(!families.includes(articles[i].subFamiliaDesc)) families.push(articles[i].subFamiliaDesc);
    }
    var col1 = document.getElementById("categoryCol1");
    var col2 = document.getElementById("categoryCol2");
    col1.innerHTML = "";
    col2.innerHTML = "";
    for(var i=0;i<families.length;i++)
    {
        if(i> (families.length / 2))
        {
            col2.innerHTML +=   `<li><a onclick="getCategory('`+ families[i] + `');"><i class="fa fa-angle-right" aria-hidden="true"></i>`+ families[i] + `</a></li>`;
        }
        else
        {
            col1.innerHTML +=  `<li><a onclick="getCategory('`+ families[i] + `');"><i class="fa fa-angle-right" aria-hidden="true"></i>`+ families[i] + `</a></li>`;
        }
    }
   
}
function processArticles(articles,start_index,end_index)
{
    families = [];
	var i;
	//alert(articles[4].CodArtigo);
	var articleHolder = document.getElementById("article-holder");
	//For the modals.
	var j=0;
	if(articleHolder != null) articleHolder.innerHTML = "";
	if(articles.length < end_index) end_index = articles.length;
	for(i=start_index;i<end_index;i++)
	{
		var codArtigo = articles[i].CodArtigo
		var desc = articles[i].DescArtigo;
		var family = articles[i].subFamiliaDesc;
		var fullDesc = articles[i].FullDesc;
		var price = articles[i].PVP1;
		var flagIVA= articles[i].PVP1_IVA;
		var marca = articles[i].Marca;
		var stock = articles[i].StockActual;
		var iva;
		var withIVA;
		if(!flagIVA){
		  iva = articles[i].IVA;
		  withIVA = price + (iva/100)*price;
		}
		else
			withIVA=price;
		var currencySymbol = articles[i].moeadaSymbol;
		withIVA = withIVA.toFixed(2);
		var stars_div = "";
		for(var n=0;n<5;n++)
		{
		    stars_div += `<img src='../../../Images/star.jpg' style="width: 20px; align:left">`;
		}
		if(articleHolder != null) articleHolder.innerHTML += `<div class='col-md-3 pro-1'>
						<div class='col-m'>
							<a href='#' data-toggle='modal' data-target='#myModal`+j+`' class='offer-img'>
								<img src='../../../Images/i7.png' class='img-responsive' alt='' >
							</a>
                        
							<div class='mid-1'>
								<div class='women'>
									<h6><a href='single.html'>` + desc.substring(0,20) + `</a></h6>
                                     <div class="review" >` + stars_div + `</div>
								</div>
								<div class='mid-2'>
									<p ><label></label><em class='item_price'>`+currencySymbol+``+withIVA+`</em> </p>
                                        
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
		$("#myModal"+j+" .in-para").html(fullDesc);
		$("#myModal"+j+" .quick_desc").html("");
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-id",codArtigo);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-name",desc);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-summary",desc);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-price",withIVA);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-quantity","1");
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-image","../../../Images/i7.png");
        if(stock > 0) $("#myModal"+j+" .quick").append("<p>"+stock+ " units left <span style='color:green;'>✔</span></p>");
		else 
		{
            $("#myModal"+j+" .add-to").html("");
            $("#myModal"+j+" .quick").append("<p>Out of Stock.</p>");
		}
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
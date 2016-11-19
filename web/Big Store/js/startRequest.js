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
	var descriptionHolder = document.getElementById("myModal5");
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
		articleHolder.innerHTML += `<div class='col-md-3 pro-1'>
						<div class='col-m'>
							<a href='#' data-toggle='modal' data-target='#myModal`+j+`' class='offer-img'>
								<img src='images/of2.png' class='img-responsive' alt=''>
							</a>
							<div class='mid-1'>
								<div class='women'>
									<h6><a href='single.html'>` + desc + `</a></h6>
								</div>
								<div class='mid-2'>
									<p ><label></label><em class='item_price'>$`+price+`</em></p>
									  <div class='block'>
										<div class='starbox small ghosting'> </div>
									</div>
								</div>
									
							</div>
						</div>
					</div>`;
		$("#myModal"+i+" h3").html(desc);
		$("#myModal"+i+" .reducedfrom").html("$"+price);
		$("#myModal"+i+" .in-para").html("");
		$("#myModal"+i+" .quick_desc").html("");
		$("#myModal"+i+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-id",codArtigo);
		$("#myModal"+i+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-name",desc);
		$("#myModal"+i+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-summary",desc);
		$("#myModal"+i+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-price",price);
		$("#myModal"+i+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-quantity","1");
		$("#myModal"+i+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-image","images/of28.png");
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
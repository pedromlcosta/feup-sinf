window.onload = getArticlesRequest;
function getArticlesRequest()
{
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function() 
	{
    if (this.readyState == 4 && this.status == 200) {
       		processArticles(xhttp.responseText);
    	}
	};
	xhttp.open("GET", "http://localhost:49822/api/artigos", true);
	xhttp.setRequestHeader("Content-Type", "text/json");
	xhttp.send();
}
function processArticles(response)
{
	var i;
	var articles = JSON.parse(response);
	//alert(articles[4].CodArtigo);
	var articleHolder = document.getElementById("article-holder");
	var descriptionHolder = document.getElementById("myModal5");
	for(i=0;i<8;i++)
	{
		var codArtigo = articles[i].CodArtigo
		var desc = articles[i].DescArtigo;
		var price = articles[i].PCPadrao;
		var marca = articles[i].Marca;
		var stock = articles[i].StockActual;
		articleHolder.innerHTML += `<div class='col-md-3 pro-1'>
						<div class='col-m'>
							<a href='#' data-toggle='modal' data-target='#myModal`+i+`' class='offer-img'>
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
	}
}
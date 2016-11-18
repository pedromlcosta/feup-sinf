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
	for(i=0;i<8;i++)
	{
		var codArtigo = articles[i].CodArtigo
		var desc = articles[i].DescArtigo;
		articleHolder.innerHTML += `<div class='col-md-3 pro-1'>
								<div class='col-m'>
									<a href='#' data-toggle='modal' data-target='#myModal24' class='offer-img'>
										<img src='' class='img-responsive' alt=''>
									</a>
									<div class='mid-1'>
										<div class='women'>
											<h6><a href='single.html'>` + codArtigo + `</a></h6>
										</div>
										<div class='mid-2'>
											<p ><label>$1.00</label><em class='item_price'>$0.80</em></p>
											  <div class='block'>
												<div class='starbox small ghosting'> </div>
											</div>
										</div>
											<div class='add'>
										   <button class='btn btn-danger my-cart-btn my-cart-b' data-id='1' data-name='product 1' data-summary='summary 1' data-price='0.80' data-quantity='1' data-image='images/of23.png'>Add to Cart</button>
										</div>
									</div>
								</div>
							</div>`
	}
}
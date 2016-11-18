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
	var descriptionHolder = document.getElementById("parentID");
	for(i=0;i<8;i++)
	{
		var codArtigo = articles[i].CodArtigo
		var desc = articles[i].DescArtigo;
		articleHolder.innerHTML += `<div class='col-md-3 pro-1'>
								<div class='col-m'>
									<a href='#' data-toggle='modal' data-target='#myModal24' class='offer-img'>
										<img src='images/of2.png' class='img-responsive' alt=''>
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
	document.body.insertAdjacentHTML('beforeend',`<div class="modal fade" id="myModal24" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
				<div class="modal-dialog" role="document">
					<div class="modal-content modal-info">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>						
						</div>
						<div class="modal-body modal-spa">
								<div class="col-md-5 span-2">
											<div class="item">
												<img src="images/of24.png" class="img-responsive" alt="">
											</div>
								</div>
								<div class="col-md-7 span-1 ">
									<h3>Wheat(500 g)</h3>
									<p class="in-para"> There are many variations of passages of Lorem Ipsum.</p>
									<div class="price_single">
									  <span class="reducedfrom "><del>$2.00</del>$1.50</span>
									
									 <div class="clearfix"></div>
									</div>
									<h4 class="quick">Quick Overview:</h4>
									<p class="quick_desc"> Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Typi non habent claritatem insitam; es</p>
									 <div class="add-to">
										   <button class="btn btn-danger my-cart-btn my-cart-btn1 " data-id="24" data-name="Wheat" data-summary="summary 24" data-price="1.50" data-quantity="1" data-image="images/of24.png">Add to Cart</button>
										</div>
								</div>
								<div class="clearfix"> </div>
							</div>
						</div>
					</div>
				</div>`);
}
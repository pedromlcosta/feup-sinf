var articles;
var families;
var current_filtered_articles = new Array();

function clearModalErrors() {
    $("#image_success").empty();
    $("#image_failure").empty();
}

$(document).ready(function() {  
    $('.imageUpload').on('submit', function() {
        event.preventDefault();
        event.stopPropagation();
        event.stopImmediatePropagation();
       
        console.log($(this).find(".productID").val());
        var form = $(this);
        /*
        var data = new FormData(this); 
        var files = $(this).find("#imageUpload").get(0).files;
        console.log(data);

        // Add the uploaded image content to the form data collection  
        if (files.length > 0) {  
            data.append("UploadedImage", files[0]);  
        }  
         */
        var root = location.protocol + '//' + location.host + '/';

        $.ajax({
            url: root + 'api/imageUpload',
            type: 'POST',
            data: new FormData(this),
            contentType: false,  
            processData: false,
            success: function (data, textStatus, jqXHR) {
                if (typeof data.error === 'undefined') {
                    console.log(data.imageURL);
                    if (data.imageURL !== 'undefined') {
                        console.log("ImageURL is: " + data.imageURL);
                        console.log(form.parent().find("#productImage"));
                        form.parent().find("#productImage").attr("src",'/Images/' + data.imageURL);
                        console.log("banana:" + form.find('.productID').val());
                        $("#img_" + form.find('.productID').val()).attr("src",'/Images/' + data.imageURL);
                        console.log(form.parent().find("#productImage"));
                        clearModalErrors();
                        //location.reload();
                        //window.location.href = root;
                        //window.location.replace(root);
                    } else {
                        clearModalErrors();
                        console.log(data.error);
                        $("#image_failure").prepend("Could not save image.");
                    }
                } else {
                    // Handle errors here
                    console.log('ERRORS: ' + data.error);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // Handle errors here
                console.log('ERRORS: ' + jqXHR.status + " - " + errorThrown);
                clearModalErrors();
                if (jqXHR.status == 400)
                    $("#image_failure").prepend("Could not save image.");
                else
                    $("#image_failure").prepend("Could not save image.");
            }   
        });


        
    });  
});  


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
		var imageURL = articles[i].imageURL;
		var reviewsInfo= articles[i].reviewInfo;

		

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
		var reviews_div="";
	    
        
	    //REVIEW VARIABLES TO INSERT INTO MODAL

		
		if(reviewsInfo.average==-1)
		    var averageReview = 0;
		else var averageReview = Math.floor(reviewsInfo.average);  //Will round down to 3, so 3 stars
        var nReviews = reviewsInfo.count;
		var arrayReviews = reviewsInfo.reviews;
		console.log(reviewsInfo);
	    //REVIEW VARIABLES TO INSERT INTO MODAL
 	 
		for(var n=0;n<averageReview;n++)
		{
		    stars_div += `<img src='../../../Images/star.jpg' style="width: 20px; align:left">`;
		}
		 
			if(reviewsInfo.average!=-1)
		for(var n=0;n<reviewsInfo.reviews.length;n++){
			reviews_div+="<div class='review'>"+reviewsInfo.reviews[n]+" </div>";
		}
		stars_div += `<p style="display: inline;">(`+nReviews+`)</p> `;
	
		if(articleHolder != null) articleHolder.innerHTML += "<div class='col-md-3 pro-1'>"+
						"<div class='col-m'>"+
							"<a href='#' data-toggle='modal' data-target='#myModal"+j+"' class='offer-img'>"+
								"<img id='img_" + codArtigo + "' src='../../../Images/" + imageURL + "'" + "class='img-responsive' alt='' >" +
							"</a>" +
							"<div class='mid-1'>" +
								"<div class='women'>" +
									"<h6><a href='single.html'>" + desc.substring(0,20) + "</a></h6>" +
                                 
								"</div>" +
								"<div class='mid-2'>" +
									"<p ><label></label><em class='item_price'>"  +currencySymbol+"" +withIVA+"</em> </p>" +                                        
									  "<div class='block'>" +
										"<div class='starbox small ghosting'> </div>" +
									"</div>" +                                   
								"</div>" +							 	
							"</div>" +
						"</div>" +
					"</div>";


            	if(reviewsInfo.average!=-1)      
var reviewsModal= "<div class='modal fade' id='myModal" + j + '_' + j + "' tabindex='-1' role='dialog' aria-labelledby='myModalLabel'>"+
                          " <div class='modal-dialog' role='document'>"+
                         " <div class='modal-content modal-info'>"+
                          "  <div class='modal-header'>"+
                            " <button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>"+
                             " </div>"+
                              " <div class='modal-body modal-spa'>"+
                               " <h4>Reviews</h4><br/>"+
                                    "   <div class=reviews>"+
                                    reviews_div+
                                    "  </div>"+
                               " <div class='clearfix'> </div>"+
                           " </div>"+
                         "  </div>"+
                      " </div>"
                  " </div>"
 
		$("#myModal"+j+" h3").html(desc);
		$("#myModal"+j+" .score").html(stars_div);
			if(reviewsInfo.average!=-1){
		$("#myModal"+j).append(reviewsModal);
		$("#myModal"+j+" .score").append("<a href='#' data-toggle='modal' data-target='#myModal" + j + '_' + j + "' class='offer-img'>Reviews</a>");
}
		$("#myModal"+j+" .quick").html("");
		$("#myModal"+j+" .reducedfrom").html("€"+withIVA);
		$("#myModal"+j+" .in-para").html(fullDesc);
		$("#myModal"+j+" .quick_desc").html("");
		$("#myModal"+j+" .imageUpload .productID").val(codArtigo);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-id",codArtigo);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-name",desc);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-summary",desc);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-price",withIVA);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-price-original",price);
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-quantity","1");
		$("#myModal"+j+" .btn.btn-danger.my-cart-btn.my-cart-btn1").attr("data-image","../../../Images/" +imageURL);
		$("#myModal"+j+" #productImage").attr("src","../../../Images/" +imageURL);
		
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
            
            current_filtered_articles.push(articles[i]);
        }
    }
    if(string != "" ) processArticles(current_filtered_articles,0,8);
    else processArticles(articles,0,8);
}
function getReviews(productID)
{
    
    var root = location.protocol + '//' + location.host + '/';
    //make request with it all
    $.ajax({
        url: root + 'api/review?codArt='+productID,
        type: 'GET',
        success: function (data, textStatus, jqXHR) {
            console.log(JSON.parse(jqXHR.responseText));
            console.log("success");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("error");
        }
    });
    
}
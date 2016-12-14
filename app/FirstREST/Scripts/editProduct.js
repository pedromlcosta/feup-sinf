$(document).ready(function () {

$('#descriptionHolder').on('blur','.in-para.productDescEdit',function(event) {
   
//  alert('Handler for .change() called.'+$(this).parent);
  //console.log("qualquer coisa");
  //.attr('data-id')
  var productID=$(this).siblings( ".add-to").children("button.my-cart-btn").attr("data-id");
  var newDesc = $(this).html();
     $.ajax({
            url: root + 'api/clientRegister',
            type: 'POST',
            data:
            {
                productID: productID,
                newDesc: newDesc
            },
            success: function (data, textStatus, jqXHR) {
                if (typeof data.error === 'undefined') {
                    console.log("here");

                    if (data.changed == 'true') {
                       location.reload();
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
                    $("#register_failure").prepend("Error registering.");
            }
        });
    
});
    });
 
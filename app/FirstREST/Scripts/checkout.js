
function getCart() {
    var list = [];
    cartList = [];
    var tr = $("#my-cart-table tbody").children();
    for (var i = 0; i < tr.length - 2; i++)
    {
        var title = tr[i].getAttribute("title");
        var id = tr[i].getAttribute("data-id");
        var price = tr[i].getAttribute("data-price");
        var td = tr[i].children;
        var input = td[3].children;
        var quantity = input[0].getAttribute("value");

        var data = {
            id: id,
            quantity: quantity,
            price : price
        };
        list.push(data);
    }
    localStorage.cartList= JSON.stringify(list);
    return list;
}
function makeOrder()
{
    
    var order_email = $("#checkoutEmail").val();
    var order_name = $("#checkoutName").val();
    var order_nif = $("input[name='checkoutNif']").val();
    var order_address = $("input[name='checkoutAddress']").val();
    var order_postal = $("input[name='checkoutPostal']").val();
    var order_payment = $('#checkoutPayment input:radio:checked').val()
    var root = location.protocol + '//' + location.host + '/';

    //Get relevant user info
    var list = JSON.parse(localStorage.cartList);

    var get_id = $("#codCliente_input").val();
    var get_email= $("#email_input").val();
    var get_name = $("#name_input").val();
    //make request with it all
    $.ajax({
        url: root + 'api/cart',
        type: 'POST',
        data:
        {
            id: get_id,
            name: get_name,
            email: get_email,
            nif: order_nif,
            address: order_address,
            postal: order_postal,
            payment: order_payment,
            products: list
        },
        success: function (data, textStatus, jqXHR) {
            $("#checkout_failure").empty();
            $("#checkout_failure").prepend("Sucess.");
            cleanCart();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#checkout_failure").empty();
            $("#checkout_failure").prepend("Error making order.");
        }
    });
    
}
function cleanCart()
{
    var closes = document.getElementsByClassName("btn btn-xs btn-danger my-product-remove");
    for (var i = 0; i < closes.length; i++) {
        closes[i].click();
    }
    document.getElementById("close_button").click();
}
function autoFill()
{
    var get_email = $("#email_input").val();
    var get_name = $("#name_input").val();

    $("#checkoutEmail").val(get_email);
    $("#checkoutName").val(get_name);
    
}
$(document).ready(function () {

    // Login form submition
    $('#checkoutSubmit').on('submit', sendCheckout());
});
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
function sendCheckout()
{
    var root = location.protocol + '//' + location.host + '/';
    //Get relevant user info
    var list = JSON.parse(localStorage.cartList);
    var gandalf = "gandalf";
    var codCliente = $("#codCliente_input").val();
    //make request with it all
    $.ajax({
        url: root + 'api/cart',
        type: 'POST',
        data:
        {
            id: codCliente,
            date: "parsemebaby",
            address: "rua esc",
            products: list
        },
        success: function (data, textStatus, jqXHR) {
            console.log("tudo ok");
            var closes = document.getElementsByClassName("btn btn-xs btn-danger my-product-remove");
            for (var i = 0; i < closes.length; i++) {
                closes[i].click();
            }
            document.getElementById("close_button").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("fail");
            alert("Error making order.");
        }
    });
}
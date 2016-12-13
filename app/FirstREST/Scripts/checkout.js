function getCart() {
    var list = [];
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
    return list;
}
function sendCheckout()
{
    var root = location.protocol + '//' + location.host + '/';
    //Get relevant user info
    var list = getCart();
    var gandalf = "gandalf";
    //make request with it all
    $.ajax({
        url: root + 'api/cart',
        type: 'POST',
        data:
        {
            id: "ALCAD",
            date: "parsemebaby",
            address: "rua escura",
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
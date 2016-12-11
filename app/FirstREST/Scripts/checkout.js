function getCart() {
    var list = [];
    var tr = $("#my-cart-table tbody").children();
    for (var i = 0; i < tr.length - 2; i++)
    {
        var title = tr[i].getAttribute("title");
        var id = tr[i].getAttribute("data-id");
        console.log(id);
        var td = tr[i].children;
        var input = td[3].children;
        var quantity = input[0].getAttribute("value");
        console.log(quantity);
        var data = {
            'id': id,
            'quantity': quantity 
        };
        list.push(data);
    }
    return list;
}
   
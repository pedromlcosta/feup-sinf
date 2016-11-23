$("checkout_button").click(function(){
    $.get("http://localhost:49822/api/artigos", function(data, status){
        alert("Data: " + data + "\nStatus: " + status);
    });
});
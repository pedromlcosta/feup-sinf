function requestArticles()
{
        var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
	    if (xhr.readyState == XMLHttpRequest.DONE) {
	        alert(xhr.responseText);
	    }
	 	xhr.open('GET', 'http://localhost:49822/api/artigos', true);
        xhr.setRequestHeader("Content-type", "application/json");
	  	xhr.send();
		}
}
window.onload = requestArticles;
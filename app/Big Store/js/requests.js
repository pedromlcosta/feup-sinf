function requestArticles()
{
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function() {
    if (xhr.readyState == XMLHttpRequest.DONE) {
        alert(xhr.responseText);
    }
}
  xhr.open('GET', 'http://example.com', true);
  xhr.send(null);
}

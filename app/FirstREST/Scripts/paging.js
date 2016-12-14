var currentIndex = 1;
var startIndex = 0;
var endIndex = 8;
function pageToLeft(purpose)
{
	if(currentIndex > 1) 
	{
		currentIndex--;
		endIndex = startIndex;
		startIndex -= 8;
	}
	document.getElementById("pageIndex").innerHTML = currentIndex;
    if(purpose=='artigo')
	processArticles(current_filtered_articles,startIndex,endIndex);
    if(purpose=='encomenda')
    processOrders(orders,startIndex,endIndex)
}
function pageToRight(purpose)
{
    if(purpose=='artigo')
	    if(startIndex+8 < current_filtered_articles.length)
	    {
		    currentIndex++;
		    startIndex = endIndex
		    endIndex += 8;
	    }
    if (purpose == 'encomenda')
        if (startIndex + 8 < orders.length)
        {
            currentIndex++;
            startIndex = endIndex
            endIndex += 8;
        }
	document.getElementById("pageIndex").innerHTML = currentIndex;
	
	if (purpose == 'artigo')
	    processArticles(current_filtered_articles, startIndex, endIndex);
	if (purpose == 'encomenda')
	    processOrders(orders, startIndex, endIndex)
}
function resetPaging()
{
	currentIndex = 1;
    startIndex = 0;
    endIndex = 8;
    document.getElementById("pageIndex").innerHTML = currentIndex;
}
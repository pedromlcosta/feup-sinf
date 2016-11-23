var currentIndex = 1;
var startIndex = 0;
var endIndex = 8;
function pageToLeft()
{
	if(currentIndex > 1) 
	{
		currentIndex--;
		endIndex = startIndex;
		startIndex -= 8;
	}
	document.getElementById("pageIndex").innerHTML = currentIndex;
	processArticles(current_filtered_articles,startIndex,endIndex);

}
function pageToRight()
{
	if(startIndex+8 < current_filtered_articles.length)
	{
		currentIndex++;
		startIndex = endIndex
		endIndex += 8;
	}
	document.getElementById("pageIndex").innerHTML = currentIndex;
	processArticles(current_filtered_articles,startIndex,endIndex);
}
function resetPaging()
{
	currentIndex = 1;
    startIndex = 0;
    endIndex = 8;
    document.getElementById("pageIndex").innerHTML = currentIndex;
}
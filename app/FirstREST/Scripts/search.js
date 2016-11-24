function searchFunction() {
	var x = document.forms["search-bar"]["Search"].value;
    filterArticles(x);
}
function getCategory(category)
{
    filterArticlesbyCategory(category);
}
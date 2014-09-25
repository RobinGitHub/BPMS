$(function() {
			// 搜索处理
			var searchInput = $("#searchInput");
			var emptyStr = "请输入搜索条件…";
			searchInput.focus(function() {
						if (searchInput.val() == emptyStr) {
							searchInput.val("");
						}
						searchInput.css("color", "#000000");
					}).click(function() {
						searchInput.focus();
					}).blur(function() {
						if ($.trim(searchInput.val()) == "") {
							searchInput.val(emptyStr).css("color", "#cccccc");
						}
					}).keyup(function(e) {
				if (e.keyCode == 13) {
					if (searchInput.val() == ""
							|| searchInput.val() == emptyStr) {
						alert(emptyStr);
						return false;
					} else {
						window.location ="/search/?kw=" + encodeURI(searchInput.val());
					}

				}
			});
			searchInput.blur();
			// 搜索点击处理
			$("#searchButton").click(function() {
						if (searchInput.val() == ""|| searchInput.val() == emptyStr) {
							alert(emptyStr);
							return false;
						} else {
							$(this).attr("href","/search/?kw=" + encodeURI(searchInput.val()));
						}
					});
			// 高级搜索点击处理
			$("#advancedSearchButton").click(function() {
				if (searchInput.val() == ""|| searchInput.val() == emptyStr) {
					alert(emptyStr);
					return false;
				} else {
					var kkkk=encodeURI(searchInput.val());
					$(this).attr("href","http://www.baidu.com/baidu?keyword="+kkkk+"&search_type=art&step=2&search_in=http%3A%2F%2Fwww.baidu.com%2Fbaidu&word="+kkkk+"&tn=bds&cl=3&ct=2097152&si=www.verydemo.com&q="+kkkk+"&domains=www.verydemo.com&sitesearch=www.verydemo.com&forid=1&ie=utf8&oe=utf8&hl=cn&x=28&y=13");
				}
			});

			// 导航处理
			$("#navUl li[class!='active']").mouseover(function() {
					$(this).addClass("hover");
			}).mouseout(function() {
					$(this).removeClass("hover");
			});
});
var _hmt = _hmt || [];
(function() {
  var hm = document.createElement("script");
  hm.src = "//hm.baidu.com/hm.js?9b238846a07d7a339bd98b95c0e61213";
  var s = document.getElementsByTagName("script")[0]; 
  s.parentNode.insertBefore(hm, s);
})();

var dUrl=window.location.href;
if(dUrl.indexOf("verydemo.com")<0){
	window.location="http://www.verydemo.com";
}
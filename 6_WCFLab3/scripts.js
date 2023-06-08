function DoAjax(method, url, fn) {
	var rq;
	if(window.XMLHttpRequest) rq = new XMLHttpRequest();
	else rq = new ActiveXObject("Microsoft.XMLHTTP");
	rq.open(method, url, true);
	var handler = function(rq) {
		return function() {
			if(rq.readyState != 4) return;
			if(rq.status == 200) fn(rq);
			else alert(rq.readyState + " " + rq.status + " " + rq.statusText);
			};
		}
	rq.onreadystatechange = handler(rq);
	rq.send();
	}

function HandleClick() {
	var fn = function(rq) {
		//alert(rq.responseText);
		//		     tag           #text         zwartosc
		var e = document.getElementById("wynik");
		e.innerHTML = rq.responseXML.childNodes[0].childNodes[0].nodeValue;
		e = document.getElementById("resp");
		e.innerHTML = rq.responseText;
		};
	var a, b;
	a = document.getElementById("a").value;
	b = document.getElementById("b").value;

	DoAjax("POST", "Dodaj/" + encodeURI(a) + "/" + encodeURI(b), fn);
	}

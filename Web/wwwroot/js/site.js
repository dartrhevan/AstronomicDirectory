// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function onpladd(viewName = "PlanetEditor") {
    var inp = document.getElementById('newPlanetName');
    var g = document.getElementById('g');
    var n = document.getElementById('n');

    var err = document.getElementById("err");
    if (inp.value == "") {
        err.innerText = "*The name mustn't be empty";
        inp.focus();
        //window.location.
    }
    else if (n.value == "") {
        err.innerText = "*Star name mustn't be empty";
        n.focus();
    } else if (g.value == "") {
        err.innerText = "*Galaxy name mustn't be empty";
        g.focus();
    }
    else {
        err.innerText = "";
        var d = document.getElementById('d');
        //d.innerHTML = d.innerHTML.concat('<br />' + inp.value);
        d.innerHTML = "<li class = \"list-group-item list-group-item-info\">" + inp.value + '</li>' + d.innerHTML;/*
        a.href = "PlanetEditor?N=" + inp.value;
        a.target = "_blank";
        a.method = "GET";*/
        //document.location
        window.open(viewName + "?name=" + inp.value + "&owner=" + n.value + "&gal=" + g.value, '_blank');
    }
}

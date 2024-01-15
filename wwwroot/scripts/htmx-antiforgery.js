document.addEventListener("htmx:configRequest", (evt) => {
    let httpVerb = evt.detail.verb.toUpperCase();
    if (httpVerb === 'GET') return;

    let antiforgeryResponse =  httpGet("/antiforgery").slice(1,-1);
        
    const parts = antiforgeryResponse.split("|");

    let formFieldName = parts[0];
    let antiforgeryToken = parts[1];

    evt.detail.parameters[formFieldName] = antiforgeryToken;
});

function httpGet(url)
{
    let xmlHttp = new XMLHttpRequest();
    xmlHttp.open( "GET", url, false );
    xmlHttp.send( null );
    return xmlHttp.responseText;
}
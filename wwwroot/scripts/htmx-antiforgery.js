document.addEventListener("htmx:configRequest", (evt) => {
    const httpVerb = evt.detail.verb.toUpperCase();
    if (httpVerb === 'GET') return;

    const afResponse = httpGet("/antiforgery").slice(1,-1);
        
    const parts = afResponse.split("|");

    const formFieldName = parts[0];
    const afToken = parts[1];

    evt.detail.parameters[formFieldName] = afToken;
});

function httpGet(url)
{
    const xmlHttp = new XMLHttpRequest();
    xmlHttp.open( "GET", url, false );
    xmlHttp.send( null );
    return xmlHttp.responseText;
}